using FEMEE.Domain.Enums;
namespace FEMEE.Application.DTOs.Partida
{
    /// <summary>
    /// DTO para criar uma nova partida.
    /// </summary>
    public class CreatePartidaDto
    {
        /// <summary>
        /// ID do campeonato.
        /// </summary>
        public int CampeonatoId { get; set; }

        /// <summary>
        /// ID do time A (mandante).
        /// </summary>
        public int TimeAId { get; set; }

        /// <summary>
        /// ID do time B (visitante).
        /// </summary>
        public int TimeBId { get; set; }

        /// <summary>
        /// Data e hora da partida.
        /// </summary>
        public DateTime DataHora { get; set; }

        /// <summary>
        /// Local da partida.
        /// </summary>
        public string Local { get; set; } = null!;

        /// <summary>
        /// Fase da partida (Classificatória, Semifinal, Final, etc).
        /// </summary>
        public Fase Fase { get; set; }
    }
}
