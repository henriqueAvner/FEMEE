using FEMEE.Application.DTOs.Common;
using FEMEE.Application.DTOs.Time;

namespace FEMEE.Application.Interfaces.Services
{
    /// <summary>
    /// Interface para o serviço de times.
    /// Define as operações disponíveis para gerenciar times.
    /// </summary>
    public interface ITimeService
    {
        /// <summary>
        /// Obtém um time pelo ID.
        /// </summary>
        Task<TimeResponseDto> GetTimeByIdAsync(int id);

        /// <summary>
        /// Obtém todos os times.
        /// </summary>
        Task<IEnumerable<TimeResponseDto>> GetAllTimesAsync();

        /// <summary>
        /// Obtém times com paginação.
        /// </summary>
        Task<PagedResult<TimeResponseDto>> GetTimesPagedAsync(PaginationParams pagination);

        /// <summary>
        /// Obtém um time pelo slug.
        /// </summary>
        Task<TimeResponseDto> GetTimeBySlugAsync(string slug);

        /// <summary>
        /// Obtém times ordenados por ranking.
        /// </summary>
        Task<IEnumerable<TimeResponseDto>> GetTimesByRankingAsync();

        /// <summary>
        /// Cria um novo time.
        /// </summary>
        Task<TimeResponseDto> CreateTimeAsync(CreateTimeDto dto);

        /// <summary>
        /// Atualiza um time existente.
        /// </summary>
        Task<TimeResponseDto> UpdateTimeAsync(int id, UpdateTimeDto dto);

        /// <summary>
        /// Deleta um time.
        /// </summary>
        Task DeleteTimeAsync(int id);

        /// <summary>
        /// Atualiza o ranking de todos os times.
        /// </summary>
        Task UpdateRankingAsync();
    }
}
