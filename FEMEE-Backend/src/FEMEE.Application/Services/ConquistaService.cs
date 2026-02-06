using AutoMapper;
using FEMEE.Application.DTOs.Conquista;
using FEMEE.Application.Interfaces.Repositories;
using FEMEE.Application.Interfaces.Services;
using FEMEE.Application.Logging;
using FEMEE.Domain.Entities.Principal;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace FEMEE.Application.Services
{
    /// <summary>
    /// Serviço de conquistas.
    /// </summary>
    public class ConquistaService : IConquistaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ConquistaService> _logger;

        public ConquistaService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ConquistaService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ConquistaResponseDto> GetConquistaByIdAsync(int id)
        {
            using (StructuredLogging.BeginOperationScope("GetConquistaById", id))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Buscando conquista com ID: {ConquistaId}", id);
                    var conquista = await _unitOfWork.Conquistas.GetByIdAsync(id);

                    if (conquista == null)
                    {
                        _logger.LogWarning("Conquista não encontrada: {ConquistaId}", id);
                        throw new KeyNotFoundException($"Conquista com ID {id} não encontrada");
                    }

                    stopwatch.Stop();
                    _logger.LogInformation("Conquista encontrada em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    return _mapper.Map<ConquistaResponseDto>(conquista);
                }
                catch (Exception ex) when (ex is not KeyNotFoundException)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao buscar conquista em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<IEnumerable<ConquistaResponseDto>> GetAllConquistasAsync()
        {
            using (StructuredLogging.BeginOperationScope("GetAllConquistas"))
            {
                _logger.LogInformation("Buscando todas as conquistas");
                var conquistas = await _unitOfWork.Conquistas.GetAllAsync();
                _logger.LogInformation("Total de conquistas encontradas: {Count}", conquistas.Count());
                return _mapper.Map<IEnumerable<ConquistaResponseDto>>(conquistas);
            }
        }

        public async Task<IEnumerable<ConquistaResponseDto>> GetConquistasByTimeIdAsync(int timeId)
        {
            using (StructuredLogging.BeginOperationScope("GetConquistasByTimeId", timeId))
            {
                _logger.LogInformation("Buscando conquistas do time: {TimeId}", timeId);
                var conquistas = await _unitOfWork.Conquistas.GetAllAsync();
                var conquistasTime = conquistas.Where(c => c.TimeId == timeId);
                _logger.LogInformation("Conquistas encontradas: {Count}", conquistasTime.Count());
                return _mapper.Map<IEnumerable<ConquistaResponseDto>>(conquistasTime);
            }
        }

        public async Task<IEnumerable<ConquistaResponseDto>> GetConquistasByCampeonatoIdAsync(int campeonatoId)
        {
            using (StructuredLogging.BeginOperationScope("GetConquistasByCampeonatoId", campeonatoId))
            {
                _logger.LogInformation("Buscando conquistas do campeonato: {CampeonatoId}", campeonatoId);
                var conquistas = await _unitOfWork.Conquistas.GetAllAsync();
                var conquistasCampeonato = conquistas.Where(c => c.CampeonatoId == campeonatoId);
                _logger.LogInformation("Conquistas encontradas: {Count}", conquistasCampeonato.Count());
                return _mapper.Map<IEnumerable<ConquistaResponseDto>>(conquistasCampeonato);
            }
        }

        public async Task<ConquistaResponseDto> CreateConquistaAsync(CreateConquistaDto dto)
        {
            using (StructuredLogging.BeginOperationScope("CreateConquista"))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Criando nova conquista: {Titulo}", dto.Titulo);

                    // Verificar se time existe
                    var time = await _unitOfWork.Times.GetByIdAsync(dto.TimeId);
                    if (time == null)
                    {
                        throw new InvalidOperationException($"Time com ID {dto.TimeId} não encontrado");
                    }

                    // Verificar se campeonato existe (se informado)
                    if (dto.CampeonatoId.HasValue)
                    {
                        var campeonato = await _unitOfWork.Campeonatos.GetByIdAsync(dto.CampeonatoId.Value);
                        if (campeonato == null)
                        {
                            throw new InvalidOperationException($"Campeonato com ID {dto.CampeonatoId} não encontrado");
                        }
                    }

                    var conquista = _mapper.Map<Conquista>(dto);
                    await _unitOfWork.Conquistas.AddAsync(conquista);
                    await _unitOfWork.SaveChangesAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Conquista criada com ID {ConquistaId} em {ElapsedMilliseconds}ms",
                        conquista.Id, stopwatch.ElapsedMilliseconds);

                    return _mapper.Map<ConquistaResponseDto>(conquista);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao criar conquista em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<ConquistaResponseDto> UpdateConquistaAsync(int id, UpdateConquistaDto dto)
        {
            using (StructuredLogging.BeginOperationScope("UpdateConquista", id))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Atualizando conquista com ID: {ConquistaId}", id);
                    var conquista = await _unitOfWork.Conquistas.GetByIdAsync(id);

                    if (conquista == null)
                    {
                        _logger.LogWarning("Conquista não encontrada para atualização: {ConquistaId}", id);
                        throw new KeyNotFoundException($"Conquista com ID {id} não encontrada");
                    }

                    // Verificar se novo time existe
                    if (dto.TimeId.HasValue)
                    {
                        var time = await _unitOfWork.Times.GetByIdAsync(dto.TimeId.Value);
                        if (time == null)
                        {
                            throw new InvalidOperationException($"Time com ID {dto.TimeId} não encontrado");
                        }
                        conquista.TimeId = dto.TimeId.Value;
                    }

                    // Atualizar propriedades
                    if (dto.CampeonatoId.HasValue) conquista.CampeonatoId = dto.CampeonatoId;
                    if (dto.Titulo != null) conquista.Titulo = dto.Titulo;
                    if (dto.Descricao != null) conquista.Descricao = dto.Descricao;
                    if (dto.Posicao.HasValue) conquista.Posicao = dto.Posicao.Value;
                    if (dto.DataConquista.HasValue) conquista.DataConquista = dto.DataConquista.Value;
                    if (dto.IconeTitulo != null) conquista.IconeTitulo = dto.IconeTitulo;

                    await _unitOfWork.Conquistas.UpdateAsync(conquista);
                    await _unitOfWork.SaveChangesAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Conquista atualizada em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);

                    return _mapper.Map<ConquistaResponseDto>(conquista);
                }
                catch (Exception ex) when (ex is not KeyNotFoundException && ex is not InvalidOperationException)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao atualizar conquista em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task DeleteConquistaAsync(int id)
        {
            using (StructuredLogging.BeginOperationScope("DeleteConquista", id))
            {
                _logger.LogInformation("Deletando conquista com ID: {ConquistaId}", id);
                var conquista = await _unitOfWork.Conquistas.GetByIdAsync(id);

                if (conquista == null)
                {
                    _logger.LogWarning("Conquista não encontrada para deleção: {ConquistaId}", id);
                    throw new KeyNotFoundException($"Conquista com ID {id} não encontrada");
                }

                await _unitOfWork.Conquistas.DeleteAsync(conquista.Id);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Conquista deletada com sucesso: {ConquistaId}", id);
            }
        }
    }
}
