using FEMEE.Application.DTOs.InscricaoCampeonato;
using FEMEE.Domain.Enums;

namespace FEMEE.Application.Interfaces.Services
{
    /// <summary>
    /// Interface do serviço de inscrições de campeonato.
    /// </summary>
    public interface IInscricaoCampeonatoService
    {
        /// <summary>
        /// Obtém uma inscrição pelo ID.
        /// </summary>
        Task<InscricaoCampeonatoResponseDto> GetInscricaoByIdAsync(int id);

        /// <summary>
        /// Obtém todas as inscrições.
        /// </summary>
        Task<IEnumerable<InscricaoCampeonatoResponseDto>> GetAllInscricoesAsync();

        /// <summary>
        /// Obtém inscrições de um campeonato específico.
        /// </summary>
        Task<IEnumerable<InscricaoCampeonatoResponseDto>> GetInscricoesByCampeonatoIdAsync(int campeonatoId);

        /// <summary>
        /// Obtém inscrições de um time específico.
        /// </summary>
        Task<IEnumerable<InscricaoCampeonatoResponseDto>> GetInscricoesByTimeIdAsync(int timeId);

        /// <summary>
        /// Obtém inscrições por status.
        /// </summary>
        Task<IEnumerable<InscricaoCampeonatoResponseDto>> GetInscricoesByStatusAsync(StatusInscricao status);

        /// <summary>
        /// Cria uma nova inscrição.
        /// </summary>
        Task<InscricaoCampeonatoResponseDto> CreateInscricaoAsync(CreateInscricaoCampeonatoDto dto);

        /// <summary>
        /// Atualiza uma inscrição existente.
        /// </summary>
        Task<InscricaoCampeonatoResponseDto> UpdateInscricaoAsync(int id, UpdateInscricaoCampeonatoDto dto);

        /// <summary>
        /// Aprova uma inscrição.
        /// </summary>
        Task<InscricaoCampeonatoResponseDto> AprovarInscricaoAsync(int id);

        /// <summary>
        /// Rejeita uma inscrição.
        /// </summary>
        Task<InscricaoCampeonatoResponseDto> RejeitarInscricaoAsync(int id, string? motivo = null);

        /// <summary>
        /// Deleta uma inscrição.
        /// </summary>
        Task DeleteInscricaoAsync(int id);
    }
}
