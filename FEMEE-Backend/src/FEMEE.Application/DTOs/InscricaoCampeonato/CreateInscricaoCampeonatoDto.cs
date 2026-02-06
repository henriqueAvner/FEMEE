namespace FEMEE.Application.DTOs.InscricaoCampeonato
{
    /// <summary>
    /// DTO para criar inscrição em campeonato.
    /// </summary>
    public class CreateInscricaoCampeonatoDto
    {
        /// <summary>
        /// ID do campeonato.
        /// </summary>
        public int CampeonatoId { get; set; }

        /// <summary>
        /// ID do time a inscrever.
        /// </summary>
        public int TimeId { get; set; }
    }
}
