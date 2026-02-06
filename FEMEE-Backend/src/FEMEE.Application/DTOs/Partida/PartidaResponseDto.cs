using FEMEE.Application.DTOs.Time;
using FEMEE.Domain.Enums;

namespace FEMEE.Application.DTOs.Partida
{
    /// <summary>
    /// DTO para resposta de partida.
    /// </summary>
    public class PartidaResponseDto
    {
        /// <summary>
        /// ID único da partida.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID do campeonato.
        /// </summary>
        public int CampeonatoId { get; set; }

        /// <summary>
        /// Informações do time A (mandante).
        /// </summary>
        public TimeResponseDto TimeA { get; set; } = null!;

        /// <summary>
        /// Informações do time B (visitante).
        /// </summary>
        public TimeResponseDto TimeB { get; set; } = null!;

        /// <summary>
        /// Informações do time vencedor (se houver).
        /// </summary>
        public TimeResponseDto TimeVencedor { get; set; } = null!;

        /// <summary>
        /// Data e hora da partida.
        /// </summary>
        public DateTime DataHora { get; set; }

        /// <summary>
        /// Local da partida.
        /// </summary>
        public string Local { get; set; } = null!;

        /// <summary>
        /// Fase da partida.
        /// </summary>
        public Fase Fase { get; set; }

        /// <summary>
        /// Placar do time A.
        /// </summary>
        public int PlacarTimeA { get; set; }

        /// <summary>
        /// Placar do time B.
        /// </summary>
        public int PlacarTimeB { get; set; }

        /// <summary>
        /// Status da partida.
        /// </summary>
        public StatusPartida Status { get; set; }

        /// <summary>
        /// URL da transmissão.
        /// </summary>
        public string TransmissaoUrl { get; set; } = null!;
    }
}
