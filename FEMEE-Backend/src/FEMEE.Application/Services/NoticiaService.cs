using AutoMapper;
using FEMEE.Application.DTOs.Noticia;
using FEMEE.Application.Interfaces.Repositories;
using FEMEE.Application.Interfaces.Services;
using FEMEE.Application.Logging;
using FEMEE.Domain.Entities.Principal;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace FEMEE.Application.Services
{
    /// <summary>
    /// Serviço de notícias.
    /// </summary>
    public class NoticiaService : INoticiaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<NoticiaService> _logger;

        public NoticiaService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<NoticiaService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<NoticiaResponseDto> GetNoticiaByIdAsync(int id)
        {
            using (StructuredLogging.BeginOperationScope("GetNoticiaById", id))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Buscando notícia com ID: {NoticiaId}", id);
                    var noticia = await _unitOfWork.Noticias.GetByIdAsync(id);

                    if (noticia == null)
                    {
                        _logger.LogWarning("Notícia não encontrada: {NoticiaId}", id);
                        throw new KeyNotFoundException($"Notícia com ID {id} não encontrada");
                    }

                    stopwatch.Stop();
                    _logger.LogInformation("Notícia encontrada em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    return _mapper.Map<NoticiaResponseDto>(noticia);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao buscar notícia em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<IEnumerable<NoticiaResponseDto>> GetAllNoticiasAsync(int page = 1, int pageSize = 10)
        {
            using (StructuredLogging.BeginOperationScope("GetAllNoticias"))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Buscando notícias - Página: {Page}, Tamanho: {PageSize}", page, pageSize);
                    var noticias = await _unitOfWork.Noticias.GetAllAsync();

                    var paginatedNoticias = noticias
                        .OrderByDescending(n => n.DataPublicacao)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    stopwatch.Stop();
                    _logger.LogInformation("Total de notícias encontradas: {Count} em {ElapsedMilliseconds}ms",
                        paginatedNoticias.Count, stopwatch.ElapsedMilliseconds);

                    return _mapper.Map<IEnumerable<NoticiaResponseDto>>(paginatedNoticias);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao buscar todas as notícias em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<NoticiaResponseDto> CreateNoticiaAsync(CreateNoticiaDto dto)
        {
            using (StructuredLogging.BeginOperationScope("CreateNoticia"))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Criando nova notícia: {Titulo}", dto.Titulo);

                    var noticia = _mapper.Map<Noticia>(dto);
                    noticia.DataPublicacao = DateTime.UtcNow;

                    await _unitOfWork.Noticias.AddAsync(noticia);
                    await _unitOfWork.SaveChangesAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Notícia criada com sucesso em {ElapsedMilliseconds}ms: {NoticiaId}",
                        stopwatch.ElapsedMilliseconds, noticia.Id);

                    return _mapper.Map<NoticiaResponseDto>(noticia);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao criar notícia em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<NoticiaResponseDto> UpdateNoticiaAsync(int id, UpdateNoticiaDto dto)
        {
            using (StructuredLogging.BeginOperationScope("UpdateNoticia", id))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Atualizando notícia: {NoticiaId}", id);

                    var noticia = await _unitOfWork.Noticias.GetByIdAsync(id);
                    if (noticia == null)
                    {
                        _logger.LogWarning("Notícia não encontrada: {NoticiaId}", id);
                        throw new KeyNotFoundException($"Notícia com ID {id} não encontrada");
                    }

                    _mapper.Map(dto, noticia);
                    noticia.DataPublicacao = DateTime.UtcNow;

                    await _unitOfWork.Noticias.UpdateAsync(noticia);
                    await _unitOfWork.SaveChangesAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Notícia atualizada com sucesso em {ElapsedMilliseconds}ms",
                        stopwatch.ElapsedMilliseconds);

                    return _mapper.Map<NoticiaResponseDto>(noticia);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao atualizar notícia em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task DeleteNoticiaAsync(int id)
        {
            using (StructuredLogging.BeginOperationScope("DeleteNoticia", id))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Deletando notícia: {NoticiaId}", id);

                    var noticia = await _unitOfWork.Noticias.GetByIdAsync(id);
                    if (noticia == null)
                    {
                        _logger.LogWarning("Notícia não encontrada: {NoticiaId}", id);
                        throw new KeyNotFoundException($"Notícia com ID {id} não encontrada");
                    }

                    await _unitOfWork.Noticias.DeleteAsync(id);
                    await _unitOfWork.SaveChangesAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Notícia deletada com sucesso em {ElapsedMilliseconds}ms",
                        stopwatch.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao deletar notícia em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }
    }
}
