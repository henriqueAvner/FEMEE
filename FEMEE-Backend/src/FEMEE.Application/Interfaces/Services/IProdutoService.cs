using FEMEE.Application.DTOs.Produto;

namespace FEMEE.Application.Interfaces.Services
{
    /// <summary>
    /// Interface para o serviço de produtos.
    /// </summary>
    public interface IProdutoService
    {
        Task<ProdutoResponseDto> GetProdutoByIdAsync(int id);
        Task<IEnumerable<ProdutoResponseDto>> GetAllProdutosAsync();
        Task<ProdutoResponseDto> CreateProdutoAsync(CreateProdutoDto dto);
        Task<ProdutoResponseDto> UpdateProdutoAsync(int id, UpdateProdutoDto dto);
        Task DeleteProdutoAsync(int id);
    }
}
