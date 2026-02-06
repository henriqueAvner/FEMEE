using FEMEE.Application.DTOs.Campeonato;
using FEMEE.Application.DTOs.Common;
using FEMEE.Domain.Enums;

namespace FEMEE.Application.Interfaces.Services
{
    /// <summary>
    /// Interface para o serviço de campeonatos.
    /// </summary>
    public interface ICampeonatoService
    {
        Task<CampeonatoResponseDto> GetCampeonatoByIdAsync(int id);
        Task<IEnumerable<CampeonatoResponseDto>> GetAllCampeonatosAsync();
        Task<PagedResult<CampeonatoResponseDto>> GetCampeonatosPagedAsync(PaginationParams pagination, StatusCampeonato? status = null);
        Task<IEnumerable<CampeonatoResponseDto>> GetCampeonatosByStatusAsync(StatusCampeonato status);
        /// <summary>
        /// Obtém campeonatos ativos (Open ou InProgress).
        /// </summary>
        Task<IEnumerable<CampeonatoResponseDto>> GetCampeonatosAtivosAsync();
        Task<CampeonatoResponseDto> CreateCampeonatoAsync(CreateCampeonatoDto dto);
        Task<CampeonatoResponseDto> UpdateCampeonatoAsync(int id, UpdateCampeonatoDto dto);
        Task DeleteCampeonatoAsync(int id);
    }
}
