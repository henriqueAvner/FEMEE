using FEMEE.Application.DTOs.Noticia;

namespace FEMEE.Application.Interfaces.Services
{
    /// <summary>
    /// Interface para o serviço de notícias.
    /// </summary>
    public interface INoticiaService
    {
        Task<NoticiaResponseDto> GetNoticiaByIdAsync(int id);
        Task<IEnumerable<NoticiaResponseDto>> GetAllNoticiasAsync(int page = 1, int pageSize = 10);
        Task<NoticiaResponseDto> CreateNoticiaAsync(CreateNoticiaDto dto);
        Task<NoticiaResponseDto> UpdateNoticiaAsync(int id, UpdateNoticiaDto dto);
        Task DeleteNoticiaAsync(int id);
    }
}
