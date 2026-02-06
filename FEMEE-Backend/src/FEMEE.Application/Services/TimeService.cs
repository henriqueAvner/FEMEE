using AutoMapper;
using FEMEE.Application.DTOs.Common;
using FEMEE.Application.DTOs.Time;
using FEMEE.Application.Interfaces.Repositories;
using FEMEE.Application.Interfaces.Services;
using FEMEE.Application.Logging;
using FEMEE.Domain.Entities.Campeonatos;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace FEMEE.Application.Services
{
    /// <summary>
    /// Serviço de times.
    /// Implementa as operações de CRUD e lógica de negócio para times.
    /// </summary>
    public class TimeService : ITimeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<TimeService> _logger;

        public TimeService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TimeService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TimeResponseDto> GetTimeByIdAsync(int id)
        {
            using (StructuredLogging.BeginOperationScope("GetTimeById", id))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Buscando time com ID: {TimeId}", id);
                    var time = await _unitOfWork.Times.GetByIdAsync(id);

                    if (time == null)
                    {
                        _logger.LogWarning("Time não encontrado: {TimeId}", id);
                        throw new KeyNotFoundException($"Time com ID {id} não encontrado");
                    }

                    stopwatch.Stop();
                    _logger.LogInformation("Time encontrado em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    return _mapper.Map<TimeResponseDto>(time);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao buscar time em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<IEnumerable<TimeResponseDto>> GetAllTimesAsync()
        {
            using (StructuredLogging.BeginOperationScope("GetAllTimes"))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Buscando todos os times");
                    var times = await _unitOfWork.Times.GetAllAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Total de times encontrados: {Count} em {ElapsedMilliseconds}ms",
                        times.Count(), stopwatch.ElapsedMilliseconds);

                    return _mapper.Map<IEnumerable<TimeResponseDto>>(times);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao buscar todos os times em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<PagedResult<TimeResponseDto>> GetTimesPagedAsync(PaginationParams pagination)
        {
            using (StructuredLogging.BeginOperationScope("GetTimesPaged"))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Buscando times paginados. Página: {Page}, Tamanho: {PageSize}",
                        pagination.Page, pagination.PageSize);

                    var allTimes = await _unitOfWork.Times.GetAllAsync();
                    var query = allTimes.AsQueryable();

                    // Aplica busca se fornecida
                    if (!string.IsNullOrWhiteSpace(pagination.Search))
                    {
                        query = query.Where(t =>
                            (t.Nome != null && t.Nome.Contains(pagination.Search, StringComparison.OrdinalIgnoreCase)) ||
                            (t.Slug != null && t.Slug.Contains(pagination.Search, StringComparison.OrdinalIgnoreCase)));
                    }

                    // Aplica ordenação
                    query = pagination.SortBy?.ToLower() switch
                    {
                        "nome" => pagination.IsDescending ? query.OrderByDescending(t => t.Nome) : query.OrderBy(t => t.Nome),
                        "pontos" => pagination.IsDescending ? query.OrderByDescending(t => t.Pontos) : query.OrderBy(t => t.Pontos),
                        "ranking" => query.OrderByDescending(t => t.Pontos).ThenByDescending(t => t.Vitorias),
                        _ => query.OrderBy(t => t.Nome)
                    };

                    var totalCount = query.Count();
                    var items = query.Skip(pagination.Skip).Take(pagination.PageSize).ToList();

                    stopwatch.Stop();
                    _logger.LogInformation("Times paginados obtidos em {ElapsedMilliseconds}ms. Total: {TotalCount}",
                        stopwatch.ElapsedMilliseconds, totalCount);

                    return PagedResult<TimeResponseDto>.Create(
                        _mapper.Map<IEnumerable<TimeResponseDto>>(items),
                        totalCount,
                        pagination.Page,
                        pagination.PageSize);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao buscar times paginados em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<TimeResponseDto> GetTimeBySlugAsync(string slug)
        {
            using (StructuredLogging.BeginOperationScope("GetTimeBySlug"))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    if (string.IsNullOrWhiteSpace(slug))
                    {
                        _logger.LogWarning("Slug vazio fornecido");
                        throw new ArgumentException("Slug não pode ser vazio", nameof(slug));
                    }

                    _logger.LogInformation("Buscando time por slug: {Slug}", slug);
                    // Otimizado: usa query direta ao invés de carregar todos os times
                    var time = await _unitOfWork.Times.GetBySlugAsync(slug);

                    if (time == null)
                    {
                        _logger.LogWarning("Time não encontrado: {Slug}", slug);
                        throw new KeyNotFoundException($"Time com slug {slug} não encontrado");
                    }

                    stopwatch.Stop();
                    _logger.LogInformation("Time encontrado por slug em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    return _mapper.Map<TimeResponseDto>(time);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao buscar time por slug em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<IEnumerable<TimeResponseDto>> GetTimesByRankingAsync()
        {
            using (StructuredLogging.BeginOperationScope("GetTimesByRanking"))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Buscando times ordenados por ranking");
                    var times = await _unitOfWork.Times.GetAllAsync();
                    var orderedTimes = times.OrderByDescending(t => t.Pontos)
                                           .ThenByDescending(t => t.Vitorias)
                                           .ToList();

                    stopwatch.Stop();
                    _logger.LogInformation("Ranking obtido em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    return _mapper.Map<IEnumerable<TimeResponseDto>>(orderedTimes);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao buscar ranking em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<TimeResponseDto> CreateTimeAsync(CreateTimeDto dto)
        {
            using (StructuredLogging.BeginOperationScope("CreateTime"))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Criando novo time: {TimeName}", dto.Nome);

                    var time = _mapper.Map<Time>(dto);
                    time.DataFundacao = DateTime.UtcNow;
                    time.CreatedAt = DateTime.UtcNow;
                    time.Pontos = 0;
                    time.Vitorias = 0;
                    time.Derrotas = 0;
                    time.Empates = 0;
                    time.PosicaoRanking = 0;

                    await _unitOfWork.Times.AddAsync(time);
                    await _unitOfWork.SaveChangesAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Time criado com sucesso em {ElapsedMilliseconds}ms: {TimeId}",
                        stopwatch.ElapsedMilliseconds, time.Id);

                    return _mapper.Map<TimeResponseDto>(time);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao criar time em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<TimeResponseDto> UpdateTimeAsync(int id, UpdateTimeDto dto)
        {
            using (StructuredLogging.BeginOperationScope("UpdateTime", id))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Atualizando time: {TimeId}", id);

                    var time = await _unitOfWork.Times.GetByIdAsync(id);
                    if (time == null)
                    {
                        _logger.LogWarning("Time não encontrado: {TimeId}", id);
                        throw new KeyNotFoundException($"Time com ID {id} não encontrado");
                    }

                    _mapper.Map(dto, time);
                    time.UpdatedAt = DateTime.UtcNow;

                    await _unitOfWork.Times.UpdateAsync(time);
                    await _unitOfWork.SaveChangesAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Time atualizado com sucesso em {ElapsedMilliseconds}ms",
                        stopwatch.ElapsedMilliseconds);

                    return _mapper.Map<TimeResponseDto>(time);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao atualizar time em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task DeleteTimeAsync(int id)
        {
            using (StructuredLogging.BeginOperationScope("DeleteTime", id))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Deletando time: {TimeId}", id);

                    var time = await _unitOfWork.Times.GetByIdAsync(id);
                    if (time == null)
                    {
                        _logger.LogWarning("Time não encontrado: {TimeId}", id);
                        throw new KeyNotFoundException($"Time com ID {id} não encontrado");
                    }

                    await _unitOfWork.Times.DeleteAsync(id);
                    await _unitOfWork.SaveChangesAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Time deletado com sucesso em {ElapsedMilliseconds}ms",
                        stopwatch.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao deletar time em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task UpdateRankingAsync()
        {
            using (StructuredLogging.BeginOperationScope("UpdateRanking"))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Atualizando ranking de times");

                    var times = await _unitOfWork.Times.GetAllAsync();
                    var orderedTimes = times.OrderByDescending(t => t.Pontos)
                                           .ThenByDescending(t => t.Vitorias)
                                           .ToList();

                    for (int i = 0; i < orderedTimes.Count; i++)
                    {
                        orderedTimes[i].PosicaoRanking = i + 1;
                        await _unitOfWork.Times.UpdateAsync(orderedTimes[i]);
                    }

                    await _unitOfWork.SaveChangesAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Ranking atualizado em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao atualizar ranking em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }
    }
}
