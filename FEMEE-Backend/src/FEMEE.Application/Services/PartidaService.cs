using AutoMapper;
using FEMEE.Application.DTOs.Partida;
using FEMEE.Application.Interfaces.Repositories;
using FEMEE.Application.Interfaces.Services;
using FEMEE.Application.Logging;
using FEMEE.Domain.Entities.Campeonatos;
using FEMEE.Domain.Enums;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace FEMEE.Application.Services
{
    /// <summary>
    /// Serviço de partidas.
    /// </summary>
    public class PartidaService : IPartidaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITimeService _timeService;
        private readonly ILogger<PartidaService> _logger;

        public PartidaService(IUnitOfWork unitOfWork, IMapper mapper, ITimeService timeService, ILogger<PartidaService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PartidaResponseDto> GetPartidaByIdAsync(int id)
        {
            using (StructuredLogging.BeginOperationScope("GetPartidaById", id))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Buscando partida com ID: {PartidaId}", id);
                    var partida = await _unitOfWork.Partidas.GetByIdAsync(id);

                    if (partida == null)
                    {
                        _logger.LogWarning("Partida não encontrada: {PartidaId}", id);
                        throw new KeyNotFoundException($"Partida com ID {id} não encontrada");
                    }

                    stopwatch.Stop();
                    _logger.LogInformation("Partida encontrada em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    return _mapper.Map<PartidaResponseDto>(partida);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao buscar partida em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<IEnumerable<PartidaResponseDto>> GetAllPartidasAsync()
        {
            using (StructuredLogging.BeginOperationScope("GetAllPartidas"))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Buscando todas as partidas");
                    var partidas = await _unitOfWork.Partidas.GetAllAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Total de partidas encontradas: {Count} em {ElapsedMilliseconds}ms",
                        partidas.Count(), stopwatch.ElapsedMilliseconds);

                    return _mapper.Map<IEnumerable<PartidaResponseDto>>(partidas);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao buscar todas as partidas em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<IEnumerable<PartidaResponseDto>> GetPartidasByCampeonatoAsync(int campeonatoId)
        {
            using (StructuredLogging.BeginOperationScope("GetPartidasByCampeonato", campeonatoId))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Buscando partidas do campeonato: {CampeonatoId}", campeonatoId);
                    var partidas = await _unitOfWork.Partidas.GetAllAsync();
                    var filtered = partidas.Where(p => p.CampeonatoId == campeonatoId).ToList();

                    stopwatch.Stop();
                    _logger.LogInformation("Total de partidas encontradas: {Count} em {ElapsedMilliseconds}ms",
                        filtered.Count, stopwatch.ElapsedMilliseconds);

                    return _mapper.Map<IEnumerable<PartidaResponseDto>>(filtered);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao buscar partidas por campeonato em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<IEnumerable<PartidaResponseDto>> GetPartidasByTimeAsync(int timeId)
        {
            using (StructuredLogging.BeginOperationScope("GetPartidasByTime", timeId))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Buscando partidas do time: {TimeId}", timeId);
                    var partidas = await _unitOfWork.Partidas.GetAllAsync();
                    var filtered = partidas.Where(p => p.TimeAId == timeId || p.TimeBId == timeId).ToList();

                    stopwatch.Stop();
                    _logger.LogInformation("Total de partidas encontradas: {Count} em {ElapsedMilliseconds}ms",
                        filtered.Count, stopwatch.ElapsedMilliseconds);

                    return _mapper.Map<IEnumerable<PartidaResponseDto>>(filtered);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao buscar partidas por time em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<PartidaResponseDto> CreatePartidaAsync(CreatePartidaDto dto)
        {
            using (StructuredLogging.BeginOperationScope("CreatePartida"))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Criando nova partida");

                    var partida = _mapper.Map<Partida>(dto);
                    partida.Status = StatusPartida.Agendada;

                    await _unitOfWork.Partidas.AddAsync(partida);
                    await _unitOfWork.SaveChangesAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Partida criada com sucesso em {ElapsedMilliseconds}ms: {PartidaId}",
                        stopwatch.ElapsedMilliseconds, partida.Id);

                    return _mapper.Map<PartidaResponseDto>(partida);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao criar partida em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<PartidaResponseDto> UpdatePartidaAsync(int id, UpdatePartidaDto dto)
        {
            using (StructuredLogging.BeginOperationScope("UpdatePartida", id))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Atualizando partida: {PartidaId}", id);

                    var partida = await _unitOfWork.Partidas.GetByIdAsync(id);
                    if (partida == null)
                    {
                        _logger.LogWarning("Partida não encontrada: {PartidaId}", id);
                        throw new KeyNotFoundException($"Partida com ID {id} não encontrada");
                    }

                    _mapper.Map(dto, partida);


                    await _unitOfWork.Partidas.UpdateAsync(partida);
                    await _unitOfWork.SaveChangesAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Partida atualizada com sucesso em {ElapsedMilliseconds}ms",
                        stopwatch.ElapsedMilliseconds);

                    return _mapper.Map<PartidaResponseDto>(partida);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao atualizar partida em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<PartidaResponseDto> FinishPartidaAsync(int id, int timeVencedorId, int placarA, int placarB)
        {
            using (StructuredLogging.BeginOperationScope("FinishPartida", id))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Finalizando partida: {PartidaId}", id);

                    var partida = await _unitOfWork.Partidas.GetByIdAsync(id);
                    if (partida == null)
                    {
                        _logger.LogWarning("Partida não encontrada: {PartidaId}", id);
                        throw new KeyNotFoundException($"Partida com ID {id} não encontrada");
                    }

                    partida.Status = StatusPartida.Finalizada;
                    partida.TimeVencedorId = timeVencedorId;
                    partida.PlacarTimeA = placarA;
                    partida.PlacarTimeB = placarB;

                    // Atualizar estatísticas dos times
                    var timeA = await _unitOfWork.Times.GetByIdAsync(partida.TimeAId);
                    var timeB = await _unitOfWork.Times.GetByIdAsync(partida.TimeBId);

                    if (placarA > placarB)
                    {
                        timeA.Vitorias++;
                        timeA.Pontos += 3;
                        timeB.Derrotas++;
                    }
                    else if (placarB > placarA)
                    {
                        timeB.Vitorias++;
                        timeB.Pontos += 3;
                        timeA.Derrotas++;
                    }
                    else
                    {
                        timeA.Empates++;
                        timeA.Pontos += 1;
                        timeB.Empates++;
                        timeB.Pontos += 1;
                    }

                    await _unitOfWork.Partidas.UpdateAsync(partida);
                    await _unitOfWork.Times.UpdateAsync(timeA);
                    await _unitOfWork.Times.UpdateAsync(timeB);
                    await _unitOfWork.SaveChangesAsync();

                    // Atualizar ranking
                    await _timeService.UpdateRankingAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Partida finalizada com sucesso em {ElapsedMilliseconds}ms",
                        stopwatch.ElapsedMilliseconds);

                    return _mapper.Map<PartidaResponseDto>(partida);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao finalizar partida em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task DeletePartidaAsync(int id)
        {
            using (StructuredLogging.BeginOperationScope("DeletePartida", id))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Deletando partida: {PartidaId}", id);

                    var partida = await _unitOfWork.Partidas.GetByIdAsync(id);
                    if (partida == null)
                    {
                        _logger.LogWarning("Partida não encontrada: {PartidaId}", id);
                        throw new KeyNotFoundException($"Partida com ID {id} não encontrada");
                    }

                    await _unitOfWork.Partidas.DeleteAsync(id);
                    await _unitOfWork.SaveChangesAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Partida deletada com sucesso em {ElapsedMilliseconds}ms",
                        stopwatch.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao deletar partida em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }
    }
}
