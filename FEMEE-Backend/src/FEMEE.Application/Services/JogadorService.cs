using AutoMapper;
using FEMEE.Application.DTOs.Jogador;
using FEMEE.Application.Interfaces.Repositories;
using FEMEE.Application.Interfaces.Services;
using FEMEE.Application.Logging;
using FEMEE.Domain.Entities.Principal;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace FEMEE.Application.Services
{
    /// <summary>
    /// Serviço de jogadores.
    /// </summary>
    public class JogadorService : IJogadorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<JogadorService> _logger;

        public JogadorService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<JogadorService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<JogadorResponseDto> GetJogadorByIdAsync(int id)
        {
            using (StructuredLogging.BeginOperationScope("GetJogadorById", id))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Buscando jogador com ID: {JogadorId}", id);
                    var jogador = await _unitOfWork.Jogadores.GetByIdAsync(id);

                    if (jogador == null)
                    {
                        _logger.LogWarning("Jogador não encontrado: {JogadorId}", id);
                        throw new KeyNotFoundException($"Jogador com ID {id} não encontrado");
                    }

                    stopwatch.Stop();
                    _logger.LogInformation("Jogador encontrado em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    return _mapper.Map<JogadorResponseDto>(jogador);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao buscar jogador em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<IEnumerable<JogadorResponseDto>> GetAllJogadoresAsync()
        {
            using (StructuredLogging.BeginOperationScope("GetAllJogadores"))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Buscando todos os jogadores");
                    var jogadores = await _unitOfWork.Jogadores.GetAllAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Total de jogadores encontrados: {Count} em {ElapsedMilliseconds}ms",
                        jogadores.Count(), stopwatch.ElapsedMilliseconds);

                    return _mapper.Map<IEnumerable<JogadorResponseDto>>(jogadores);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao buscar todos os jogadores em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<IEnumerable<JogadorResponseDto>> GetJogadoresByTimeAsync(int timeId)
        {
            using (StructuredLogging.BeginOperationScope("GetJogadoresByTime", timeId))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Buscando jogadores do time: {TimeId}", timeId);
                    var jogadores = await _unitOfWork.Jogadores.GetAllAsync();
                    var filtered = jogadores.Where(j => j.TimeId == timeId).ToList();

                    stopwatch.Stop();
                    _logger.LogInformation("Total de jogadores encontrados: {Count} em {ElapsedMilliseconds}ms",
                        filtered.Count, stopwatch.ElapsedMilliseconds);

                    return _mapper.Map<IEnumerable<JogadorResponseDto>>(filtered);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao buscar jogadores por time em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<JogadorResponseDto> CreateJogadorAsync(CreateJogadorDto dto)
        {
            using (StructuredLogging.BeginOperationScope("CreateJogador"))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Criando novo jogador: {NickName}", dto.NickName);

                    var jogador = _mapper.Map<Jogador>(dto);
                    jogador.DataEntradaTime = DateTime.UtcNow;
                    jogador.Status = Domain.Enums.StatusJogador.Ativo;

                    await _unitOfWork.Jogadores.AddAsync(jogador);
                    await _unitOfWork.SaveChangesAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Jogador criado com sucesso em {ElapsedMilliseconds}ms: {JogadorId}",
                        stopwatch.ElapsedMilliseconds, jogador.Id);

                    return _mapper.Map<JogadorResponseDto>(jogador);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao criar jogador em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<JogadorResponseDto> UpdateJogadorAsync(int id, UpdateJogadorDto dto)
        {
            using (StructuredLogging.BeginOperationScope("UpdateJogador", id))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Atualizando jogador: {JogadorId}", id);

                    var jogador = await _unitOfWork.Jogadores.GetByIdAsync(id);
                    if (jogador == null)
                    {
                        _logger.LogWarning("Jogador não encontrado: {JogadorId}", id);
                        throw new KeyNotFoundException($"Jogador com ID {id} não encontrado");
                    }

                    // Atualiza apenas campos fornecidos
                    if (!string.IsNullOrEmpty(dto.NickName))
                        jogador.NickName = dto.NickName;
                    if (dto.FotoUrl != null)
                        jogador.FotoUrl = dto.FotoUrl;
                    if (dto.Funcao.HasValue)
                        jogador.Funcao = dto.Funcao.Value;
                    if (dto.Status.HasValue)
                        jogador.Status = dto.Status.Value;
                    if (dto.TimeId.HasValue)
                    {
                        jogador.TimeId = dto.TimeId;
                        jogador.DataEntradaTime = DateTime.UtcNow;
                    }

                    await _unitOfWork.Jogadores.UpdateAsync(jogador);
                    await _unitOfWork.SaveChangesAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Jogador atualizado com sucesso em {ElapsedMilliseconds}ms",
                        stopwatch.ElapsedMilliseconds);

                    return _mapper.Map<JogadorResponseDto>(jogador);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao atualizar jogador em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task DeleteJogadorAsync(int id)
        {
            using (StructuredLogging.BeginOperationScope("DeleteJogador", id))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Deletando jogador: {JogadorId}", id);

                    var jogador = await _unitOfWork.Jogadores.GetByIdAsync(id);
                    if (jogador == null)
                    {
                        _logger.LogWarning("Jogador não encontrado: {JogadorId}", id);
                        throw new KeyNotFoundException($"Jogador com ID {id} não encontrado");
                    }

                    await _unitOfWork.Jogadores.DeleteAsync(id);
                    await _unitOfWork.SaveChangesAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Jogador deletado com sucesso em {ElapsedMilliseconds}ms",
                        stopwatch.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao deletar jogador em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }
    }
}
