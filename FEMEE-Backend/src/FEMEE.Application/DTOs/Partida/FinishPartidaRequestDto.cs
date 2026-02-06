namespace FEMEE.Application.DTOs.Partida
{
    /// <summary>
    /// DTO para finalizar uma partida com placar.
    /// </summary>
    public class FinishPartidaRequestDto
    {
        /// <summary>
        /// ID do time vencedor da partida.
        /// </summary>
        public int TimeVencedorId { get; set; }

        /// <summary>
        /// Placar do time A (mandante).
        /// </summary>
        public int PlacarA { get; set; }

        /// <summary>
        /// Placar do time B (visitante).
        /// </summary>
        public int PlacarB { get; set; }
    }
}
