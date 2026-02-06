using AutoMapper;
using FEMEE.Application.DTOs.Produto;
using FEMEE.Application.Interfaces.Repositories;
using FEMEE.Application.Interfaces.Services;
using FEMEE.Application.Logging;
using FEMEE.Domain.Entities.Store;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace FEMEE.Application.Services
{
    /// <summary>
    /// Serviço de produtos.
    /// </summary>
    public class ProdutoService : IProdutoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ProdutoService> _logger;

        public ProdutoService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProdutoService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ProdutoResponseDto> GetProdutoByIdAsync(int id)
        {
            using (StructuredLogging.BeginOperationScope("GetProdutoById", id))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Buscando produto com ID: {ProdutoId}", id);
                    var produto = await _unitOfWork.Produtos.GetByIdAsync(id);

                    if (produto == null)
                    {
                        _logger.LogWarning("Produto não encontrado: {ProdutoId}", id);
                        throw new KeyNotFoundException($"Produto com ID {id} não encontrado");
                    }

                    stopwatch.Stop();
                    _logger.LogInformation("Produto encontrado em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    return _mapper.Map<ProdutoResponseDto>(produto);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao buscar produto em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<IEnumerable<ProdutoResponseDto>> GetAllProdutosAsync()
        {
            using (StructuredLogging.BeginOperationScope("GetAllProdutos"))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Buscando todos os produtos");
                    var produtos = await _unitOfWork.Produtos.GetAllAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Total de produtos encontrados: {Count} em {ElapsedMilliseconds}ms",
                        produtos.Count(), stopwatch.ElapsedMilliseconds);

                    return _mapper.Map<IEnumerable<ProdutoResponseDto>>(produtos);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao buscar todos os produtos em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<ProdutoResponseDto> CreateProdutoAsync(CreateProdutoDto dto)
        {
            using (StructuredLogging.BeginOperationScope("CreateProduto"))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Criando novo produto: {Nome}", dto.Nome);

                    var produto = _mapper.Map<Produto>(dto);
                    produto.CreatedAt = DateTime.UtcNow;
                    produto.UpdatedAt = DateTime.UtcNow;

                    await _unitOfWork.Produtos.AddAsync(produto);
                    await _unitOfWork.SaveChangesAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Produto criado com sucesso em {ElapsedMilliseconds}ms: {ProdutoId}",
                        stopwatch.ElapsedMilliseconds, produto.Id);

                    return _mapper.Map<ProdutoResponseDto>(produto);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao criar produto em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<ProdutoResponseDto> UpdateProdutoAsync(int id, UpdateProdutoDto dto)
        {
            using (StructuredLogging.BeginOperationScope("UpdateProduto", id))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Atualizando produto: {ProdutoId}", id);

                    var produto = await _unitOfWork.Produtos.GetByIdAsync(id);
                    if (produto == null)
                    {
                        _logger.LogWarning("Produto não encontrado: {ProdutoId}", id);
                        throw new KeyNotFoundException($"Produto com ID {id} não encontrado");
                    }

                    _mapper.Map(dto, produto);
                    produto.UpdatedAt = DateTime.UtcNow;

                    await _unitOfWork.Produtos.UpdateAsync(produto);
                    await _unitOfWork.SaveChangesAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Produto atualizado com sucesso em {ElapsedMilliseconds}ms",
                        stopwatch.ElapsedMilliseconds);

                    return _mapper.Map<ProdutoResponseDto>(produto);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao atualizar produto em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task DeleteProdutoAsync(int id)
        {
            using (StructuredLogging.BeginOperationScope("DeleteProduto", id))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Deletando produto: {ProdutoId}", id);

                    var produto = await _unitOfWork.Produtos.GetByIdAsync(id);
                    if (produto == null)
                    {
                        _logger.LogWarning("Produto não encontrado: {ProdutoId}", id);
                        throw new KeyNotFoundException($"Produto com ID {id} não encontrado");
                    }

                    await _unitOfWork.Produtos.DeleteAsync(id);
                    await _unitOfWork.SaveChangesAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Produto deletado com sucesso em {ElapsedMilliseconds}ms",
                        stopwatch.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao deletar produto em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }
    }
}
