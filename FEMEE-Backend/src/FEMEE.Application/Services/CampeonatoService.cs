using AutoMapper;
using FEMEE.Application.DTOs.Campeonato;
using FEMEE.Application.DTOs.Common;
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
    /// Serviço de campeonatos.
    /// </summary>
    public class CampeonatoService : ICampeonatoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CampeonatoService> _logger;

        public CampeonatoService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CampeonatoService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CampeonatoResponseDto> GetCampeonatoByIdAsync(int id)
        {
            using (StructuredLogging.BeginOperationScope("GetCampeonatoById", id))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Buscando campeonato com ID: {CampeonatoId}", id);
                    var campeonato = await _unitOfWork.Campeonatos.GetByIdAsync(id);

                    if (campeonato == null)
                    {
                        _logger.LogWarning("Campeonato não encontrado: {CampeonatoId}", id);
                        throw new KeyNotFoundException($"Campeonato com ID {id} não encontrado");
                    }

                    stopwatch.Stop();
                    _logger.LogInformation("Campeonato encontrado em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    return _mapper.Map<CampeonatoResponseDto>(campeonato);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao buscar campeonato em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<IEnumerable<CampeonatoResponseDto>> GetAllCampeonatosAsync()
        {
            using (StructuredLogging.BeginOperationScope("GetAllCampeonatos"))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Buscando todos os campeonatos");
                    var campeonatos = await _unitOfWork.Campeonatos.GetAllAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Total de campeonatos encontrados: {Count} em {ElapsedMilliseconds}ms",
                        campeonatos.Count(), stopwatch.ElapsedMilliseconds);

                    return _mapper.Map<IEnumerable<CampeonatoResponseDto>>(campeonatos);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao buscar todos os campeonatos em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<PagedResult<CampeonatoResponseDto>> GetCampeonatosPagedAsync(PaginationParams pagination, StatusCampeonato? status = null)
        {
            using (StructuredLogging.BeginOperationScope("GetCampeonatosPaged"))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Buscando campeonatos paginados. Página: {Page}, Tamanho: {PageSize}, Status: {Status}",
                        pagination.Page, pagination.PageSize, status?.ToString() ?? "Todos");

                    var allCampeonatos = await _unitOfWork.Campeonatos.GetAllAsync();
                    var query = allCampeonatos.AsQueryable();

                    // Filtra por status se fornecido
                    if (status.HasValue)
                    {
                        query = query.Where(c => c.Status == status.Value);
                    }

                    // Aplica busca se fornecida
                    if (!string.IsNullOrWhiteSpace(pagination.Search))
                    {
                        query = query.Where(c =>
                            (c.Titulo != null && c.Titulo.Contains(pagination.Search, StringComparison.OrdinalIgnoreCase)) ||
                            (c.Descricao != null && c.Descricao.Contains(pagination.Search, StringComparison.OrdinalIgnoreCase)));
                    }

                    // Aplica ordenação
                    query = pagination.SortBy?.ToLower() switch
                    {
                        "titulo" => pagination.IsDescending ? query.OrderByDescending(c => c.Titulo) : query.OrderBy(c => c.Titulo),
                        "nome" => pagination.IsDescending ? query.OrderByDescending(c => c.Titulo) : query.OrderBy(c => c.Titulo),
                        "datainicio" => pagination.IsDescending ? query.OrderByDescending(c => c.DataInicio) : query.OrderBy(c => c.DataInicio),
                        "status" => pagination.IsDescending ? query.OrderByDescending(c => c.Status) : query.OrderBy(c => c.Status),
                        _ => query.OrderByDescending(c => c.DataInicio)
                    };

                    var totalCount = query.Count();
                    var items = query.Skip(pagination.Skip).Take(pagination.PageSize).ToList();

                    stopwatch.Stop();
                    _logger.LogInformation("Campeonatos paginados obtidos em {ElapsedMilliseconds}ms. Total: {TotalCount}",
                        stopwatch.ElapsedMilliseconds, totalCount);

                    return PagedResult<CampeonatoResponseDto>.Create(
                        _mapper.Map<IEnumerable<CampeonatoResponseDto>>(items),
                        totalCount,
                        pagination.Page,
                        pagination.PageSize);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao buscar campeonatos paginados em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<IEnumerable<CampeonatoResponseDto>> GetCampeonatosByStatusAsync(StatusCampeonato status)
        {
            using (StructuredLogging.BeginOperationScope("GetCampeonatosByStatus"))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Buscando campeonatos com status: {Status}", status);
                    var campeonatos = await _unitOfWork.Campeonatos.GetAllAsync();
                    var filtered = campeonatos.Where(c => c.Status == status).ToList();

                    stopwatch.Stop();
                    _logger.LogInformation("Total de campeonatos encontrados: {Count} em {ElapsedMilliseconds}ms",
                        filtered.Count, stopwatch.ElapsedMilliseconds);

                    return _mapper.Map<IEnumerable<CampeonatoResponseDto>>(filtered);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao buscar campeonatos por status em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        /// <summary>
        /// Obtém campeonatos ativos (Open ou InProgress).
        /// </summary>
        public async Task<IEnumerable<CampeonatoResponseDto>> GetCampeonatosAtivosAsync()
        {
            using (StructuredLogging.BeginOperationScope("GetCampeonatosAtivos"))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Buscando campeonatos ativos (Open ou InProgress)");
                    var campeonatos = await _unitOfWork.Campeonatos.GetAllAsync();
                    var ativos = campeonatos
                        .Where(c => c.Status == StatusCampeonato.Open || c.Status == StatusCampeonato.InProgress)
                        .OrderByDescending(c => c.DataInicio)
                        .ToList();

                    stopwatch.Stop();
                    _logger.LogInformation("Total de campeonatos ativos encontrados: {Count} em {ElapsedMilliseconds}ms",
                        ativos.Count, stopwatch.ElapsedMilliseconds);

                    return _mapper.Map<IEnumerable<CampeonatoResponseDto>>(ativos);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao buscar campeonatos ativos em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<CampeonatoResponseDto> CreateCampeonatoAsync(CreateCampeonatoDto dto)
        {
            using (StructuredLogging.BeginOperationScope("CreateCampeonato"))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Criando novo campeonato: {Titulo}", dto.Titulo);

                    var campeonato = _mapper.Map<Campeonato>(dto);
                    campeonato.CreatedAt = DateTime.UtcNow;
                    campeonato.DataInicio = DateTime.UtcNow;
                    campeonato.Status = StatusCampeonato.Open;

                    await _unitOfWork.Campeonatos.AddAsync(campeonato);
                    await _unitOfWork.SaveChangesAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Campeonato criado com sucesso em {ElapsedMilliseconds}ms: {CampeonatoId}",
                        stopwatch.ElapsedMilliseconds, campeonato.Id);

                    return _mapper.Map<CampeonatoResponseDto>(campeonato);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao criar campeonato em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<CampeonatoResponseDto> UpdateCampeonatoAsync(int id, UpdateCampeonatoDto dto)
        {
            using (StructuredLogging.BeginOperationScope("UpdateCampeonato", id))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Atualizando campeonato: {CampeonatoId}", id);

                    var campeonato = await _unitOfWork.Campeonatos.GetByIdAsync(id);
                    if (campeonato == null)
                    {
                        _logger.LogWarning("Campeonato não encontrado: {CampeonatoId}", id);
                        throw new KeyNotFoundException($"Campeonato com ID {id} não encontrado");
                    }

                    _mapper.Map(dto, campeonato);
                    campeonato.UpdatedAt = DateTime.UtcNow;

                    await _unitOfWork.Campeonatos.UpdateAsync(campeonato);
                    await _unitOfWork.SaveChangesAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Campeonato atualizado com sucesso em {ElapsedMilliseconds}ms",
                        stopwatch.ElapsedMilliseconds);

                    return _mapper.Map<CampeonatoResponseDto>(campeonato);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao atualizar campeonato em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task DeleteCampeonatoAsync(int id)
        {
            using (StructuredLogging.BeginOperationScope("DeleteCampeonato", id))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Deletando campeonato: {CampeonatoId}", id);

                    var campeonato = await _unitOfWork.Campeonatos.GetByIdAsync(id);
                    if (campeonato == null)
                    {
                        _logger.LogWarning("Campeonato não encontrado: {CampeonatoId}", id);
                        throw new KeyNotFoundException($"Campeonato com ID {id} não encontrado");
                    }

                    await _unitOfWork.Campeonatos.DeleteAsync(id);
                    await _unitOfWork.SaveChangesAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Campeonato deletado com sucesso em {ElapsedMilliseconds}ms",
                        stopwatch.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao deletar campeonato em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }
    }
}
