using FEMEE.Application.DTOs.Conquista;

namespace FEMEE.Application.Interfaces.Services
{
    /// <summary>
    /// Interface do serviço de conquistas.
    /// </summary>
    public interface IConquistaService
    {
        /// <summary>
        /// Obtém uma conquista pelo ID.
        /// </summary>
        Task<ConquistaResponseDto> GetConquistaByIdAsync(int id);

        /// <summary>
        /// Obtém todas as conquistas.
        /// </summary>
        Task<IEnumerable<ConquistaResponseDto>> GetAllConquistasAsync();

        /// <summary>
        /// Obtém conquistas de um time específico.
        /// </summary>
        Task<IEnumerable<ConquistaResponseDto>> GetConquistasByTimeIdAsync(int timeId);

        /// <summary>
        /// Obtém conquistas de um campeonato específico.
        /// </summary>
        Task<IEnumerable<ConquistaResponseDto>> GetConquistasByCampeonatoIdAsync(int campeonatoId);

        /// <summary>
        /// Cria uma nova conquista.
        /// </summary>
        Task<ConquistaResponseDto> CreateConquistaAsync(CreateConquistaDto dto);

        /// <summary>
        /// Atualiza uma conquista existente.
        /// </summary>
        Task<ConquistaResponseDto> UpdateConquistaAsync(int id, UpdateConquistaDto dto);

        /// <summary>
        /// Deleta uma conquista.
        /// </summary>
        Task DeleteConquistaAsync(int id);
    }
}
