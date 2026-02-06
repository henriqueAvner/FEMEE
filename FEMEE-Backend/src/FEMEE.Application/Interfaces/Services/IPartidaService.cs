using FEMEE.Application.DTOs.Partida;

namespace FEMEE.Application.Interfaces.Services
{
    /// <summary>
    /// Interface para o serviço de partidas.
    /// </summary>
    public interface IPartidaService
    {
        Task<PartidaResponseDto> GetPartidaByIdAsync(int id);
        Task<IEnumerable<PartidaResponseDto>> GetAllPartidasAsync();
        Task<IEnumerable<PartidaResponseDto>> GetPartidasByCampeonatoAsync(int campeonatoId);
        Task<IEnumerable<PartidaResponseDto>> GetPartidasByTimeAsync(int timeId);
        Task<PartidaResponseDto> CreatePartidaAsync(CreatePartidaDto dto);
        Task<PartidaResponseDto> UpdatePartidaAsync(int id, UpdatePartidaDto dto);
        Task<PartidaResponseDto> FinishPartidaAsync(int id, int timeVencedorId, int placarA, int placarB);
        Task DeletePartidaAsync(int id);
    }
}
