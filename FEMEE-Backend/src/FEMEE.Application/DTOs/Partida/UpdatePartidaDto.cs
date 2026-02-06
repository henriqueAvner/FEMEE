using FEMEE.Domain.Enums;

namespace FEMEE.Application.DTOs.Partida
{
    /// <summary>
    /// DTO para atualizar uma partida existente.
    /// </summary>
    public class UpdatePartidaDto
    {
        /// <summary>
        /// Novo placar do time A.
        /// </summary>
        public int PlacarTimeA { get; set; }

        /// <summary>
        /// Novo placar do time B.
        /// </summary>
        public int PlacarTimeB { get; set; }

        /// <summary>
        /// ID do time vencedor (se houver).
        /// </summary>
        public int? TimeVencedorId { get; set; }

        /// <summary>
        /// Data e hora da partida.
        /// </summary>
        public DateTime DataHora { get; set; }

        /// <summary>
        /// Fase da partida (Classificatória, Semifinal, Final, etc).
        /// </summary>
        public Fase Fase { get; set; }

        /// <summary>
        /// Novo status da partida.
        /// </summary>
        public StatusPartida Status { get; set; }

        /// <summary>
        /// URL da transmissão (se houver).
        /// </summary>
        public string TransmissaoUrl { get; set; } = null!;
    }
}
