using FEMEE.Domain.Enums;

namespace FEMEE.Application.DTOs.InscricaoCampeonato
{
    /// <summary>
    /// DTO para resposta de inscrição em campeonato.
    /// </summary>
    public class InscricaoCampeonatoResponseDto
    {
        /// <summary>
        /// ID único da inscrição.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID do campeonato.
        /// </summary>
        public int CampeonatoId { get; set; }

        /// <summary>
        /// ID do time.
        /// </summary>
        public int TimeId { get; set; }

        /// <summary>
        /// ID do capitão.
        /// </summary>
        public int CapitaoId { get; set; }

        /// <summary>
        /// Data da inscrição.
        /// </summary>
        public DateTime DataInscricao { get; set; }

        /// <summary>
        /// Status da inscrição.
        /// </summary>
        public StatusInscricao StatusInscricao { get; set; }

        /// <summary>
        /// Observações sobre a inscrição.
        /// </summary>
        public string Observacoes { get; set; } = null!;
    }
}
