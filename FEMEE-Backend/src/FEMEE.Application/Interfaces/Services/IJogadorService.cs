using FEMEE.Application.DTOs.Jogador;

namespace FEMEE.Application.Interfaces.Services
{
    /// <summary>
    /// Interface para o serviço de jogadores.
    /// </summary>
    public interface IJogadorService
    {
        Task<JogadorResponseDto> GetJogadorByIdAsync(int id);
        Task<IEnumerable<JogadorResponseDto>> GetAllJogadoresAsync();
        Task<IEnumerable<JogadorResponseDto>> GetJogadoresByTimeAsync(int timeId);
        Task<JogadorResponseDto> CreateJogadorAsync(CreateJogadorDto dto);
        Task<JogadorResponseDto> UpdateJogadorAsync(int id, UpdateJogadorDto dto);
        Task DeleteJogadorAsync(int id);
    }
}
