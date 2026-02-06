using FEMEE.Application.DTOs.Jogo;

namespace FEMEE.Application.Interfaces.Services
{
    /// <summary>
    /// Interface do serviço de jogos.
    /// </summary>
    public interface IJogoService
    {
        /// <summary>
        /// Obtém um jogo pelo ID.
        /// </summary>
        Task<JogoResponseDto> GetJogoByIdAsync(int id);

        /// <summary>
        /// Obtém um jogo pelo slug.
        /// </summary>
        Task<JogoResponseDto> GetJogoBySlugAsync(string slug);

        /// <summary>
        /// Obtém todos os jogos.
        /// </summary>
        Task<IEnumerable<JogoResponseDto>> GetAllJogosAsync();

        /// <summary>
        /// Obtém apenas jogos ativos.
        /// </summary>
        Task<IEnumerable<JogoResponseDto>> GetJogosAtivosAsync();

        /// <summary>
        /// Cria um novo jogo.
        /// </summary>
        Task<JogoResponseDto> CreateJogoAsync(CreateJogoDto dto);

        /// <summary>
        /// Atualiza um jogo existente.
        /// </summary>
        Task<JogoResponseDto> UpdateJogoAsync(int id, UpdateJogoDto dto);

        /// <summary>
        /// Deleta um jogo.
        /// </summary>
        Task DeleteJogoAsync(int id);
    }
}
