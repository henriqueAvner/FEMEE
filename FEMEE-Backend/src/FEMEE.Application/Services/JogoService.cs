using AutoMapper;
using FEMEE.Application.DTOs.Jogo;
using FEMEE.Application.Interfaces.Repositories;
using FEMEE.Application.Interfaces.Services;
using FEMEE.Application.Logging;
using FEMEE.Domain.Entities.Campeonatos;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace FEMEE.Application.Services
{
    /// <summary>
    /// Serviço de jogos.
    /// </summary>
    public class JogoService : IJogoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<JogoService> _logger;

        public JogoService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<JogoService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<JogoResponseDto> GetJogoByIdAsync(int id)
        {
            using (StructuredLogging.BeginOperationScope("GetJogoById", id))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Buscando jogo com ID: {JogoId}", id);
                    var jogo = await _unitOfWork.Jogos.GetByIdAsync(id);

                    if (jogo == null)
                    {
                        _logger.LogWarning("Jogo não encontrado: {JogoId}", id);
                        throw new KeyNotFoundException($"Jogo com ID {id} não encontrado");
                    }

                    stopwatch.Stop();
                    _logger.LogInformation("Jogo encontrado em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    return _mapper.Map<JogoResponseDto>(jogo);
                }
                catch (Exception ex) when (ex is not KeyNotFoundException)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao buscar jogo em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<JogoResponseDto> GetJogoBySlugAsync(string slug)
        {
            using (StructuredLogging.BeginOperationScope("GetJogoBySlug"))
            {
                _logger.LogInformation("Buscando jogo com slug: {Slug}", slug);
                var jogos = await _unitOfWork.Jogos.GetAllAsync();
                var jogo = jogos.FirstOrDefault(j => j.Slug == slug);

                if (jogo == null)
                {
                    _logger.LogWarning("Jogo não encontrado com slug: {Slug}", slug);
                    throw new KeyNotFoundException($"Jogo com slug '{slug}' não encontrado");
                }

                return _mapper.Map<JogoResponseDto>(jogo);
            }
        }

        public async Task<IEnumerable<JogoResponseDto>> GetAllJogosAsync()
        {
            using (StructuredLogging.BeginOperationScope("GetAllJogos"))
            {
                _logger.LogInformation("Buscando todos os jogos");
                var jogos = await _unitOfWork.Jogos.GetAllAsync();
                _logger.LogInformation("Total de jogos encontrados: {Count}", jogos.Count());
                return _mapper.Map<IEnumerable<JogoResponseDto>>(jogos);
            }
        }

        public async Task<IEnumerable<JogoResponseDto>> GetJogosAtivosAsync()
        {
            using (StructuredLogging.BeginOperationScope("GetJogosAtivos"))
            {
                _logger.LogInformation("Buscando jogos ativos");
                var jogos = await _unitOfWork.Jogos.GetAllAsync();
                var jogosAtivos = jogos.Where(j => j.Ativo);
                _logger.LogInformation("Total de jogos ativos: {Count}", jogosAtivos.Count());
                return _mapper.Map<IEnumerable<JogoResponseDto>>(jogosAtivos);
            }
        }

        public async Task<JogoResponseDto> CreateJogoAsync(CreateJogoDto dto)
        {
            using (StructuredLogging.BeginOperationScope("CreateJogo"))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Criando novo jogo: {Nome}", dto.Nome);

                    // Verificar se slug já existe
                    var jogos = await _unitOfWork.Jogos.GetAllAsync();
                    if (jogos.Any(j => j.Slug == dto.Slug))
                    {
                        throw new InvalidOperationException($"Já existe um jogo com o slug '{dto.Slug}'");
                    }

                    var jogo = _mapper.Map<Jogo>(dto);
                    await _unitOfWork.Jogos.AddAsync(jogo);
                    await _unitOfWork.SaveChangesAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Jogo criado com ID {JogoId} em {ElapsedMilliseconds}ms",
                        jogo.Id, stopwatch.ElapsedMilliseconds);

                    return _mapper.Map<JogoResponseDto>(jogo);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao criar jogo em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<JogoResponseDto> UpdateJogoAsync(int id, UpdateJogoDto dto)
        {
            using (StructuredLogging.BeginOperationScope("UpdateJogo", id))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Atualizando jogo com ID: {JogoId}", id);
                    var jogo = await _unitOfWork.Jogos.GetByIdAsync(id);

                    if (jogo == null)
                    {
                        _logger.LogWarning("Jogo não encontrado para atualização: {JogoId}", id);
                        throw new KeyNotFoundException($"Jogo com ID {id} não encontrado");
                    }

                    // Verificar se novo slug já existe
                    if (dto.Slug != null && dto.Slug != jogo.Slug)
                    {
                        var jogos = await _unitOfWork.Jogos.GetAllAsync();
                        if (jogos.Any(j => j.Slug == dto.Slug && j.Id != id))
                        {
                            throw new InvalidOperationException($"Já existe um jogo com o slug '{dto.Slug}'");
                        }
                    }

                    // Atualizar propriedades
                    if (dto.Nome != null) jogo.Nome = dto.Nome;
                    if (dto.Slug != null) jogo.Slug = dto.Slug;
                    if (dto.Descricao != null) jogo.Descricao = dto.Descricao;
                    if (dto.ImagemUrl != null) jogo.ImagemUrl = dto.ImagemUrl;
                    if (dto.Ativo.HasValue) jogo.Ativo = dto.Ativo.Value;
                    if (dto.CategoriaJogo.HasValue) jogo.CategoriaJogo = dto.CategoriaJogo.Value;

                    await _unitOfWork.Jogos.UpdateAsync(jogo);
                    await _unitOfWork.SaveChangesAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Jogo atualizado em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);

                    return _mapper.Map<JogoResponseDto>(jogo);
                }
                catch (Exception ex) when (ex is not KeyNotFoundException && ex is not InvalidOperationException)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao atualizar jogo em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task DeleteJogoAsync(int id)
        {
            using (StructuredLogging.BeginOperationScope("DeleteJogo", id))
            {
                _logger.LogInformation("Deletando jogo com ID: {JogoId}", id);
                var jogo = await _unitOfWork.Jogos.GetByIdAsync(id);

                if (jogo == null)
                {
                    _logger.LogWarning("Jogo não encontrado para deleção: {JogoId}", id);
                    throw new KeyNotFoundException($"Jogo com ID {id} não encontrado");
                }

                await _unitOfWork.Jogos.DeleteAsync(jogo.Id);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Jogo deletado com sucesso: {JogoId}", id);
            }
        }
    }
}
