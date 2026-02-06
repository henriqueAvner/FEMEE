using FEMEE.Domain.Enums;

namespace FEMEE.Application.DTOs.InscricaoCampeonato
{
    /// <summary>
    /// DTO para atualizar uma inscrição de campeonato.
    /// </summary>
    public class UpdateInscricaoCampeonatoDto
    {
        /// <summary>
        /// Status da inscrição.
        /// </summary>
        public StatusInscricao? StatusInscricao { get; set; }

        /// <summary>
        /// Telefone de contato.
        /// </summary>
        public string? TelefoneContato { get; set; }

        /// <summary>
        /// Email de contato.
        /// </summary>
        public string? EmailContato { get; set; }

        /// <summary>
        /// Nome do capitão.
        /// </summary>
        public string? NomeCapitao { get; set; }

        /// <summary>
        /// Nome do time.
        /// </summary>
        public string? NomeTime { get; set; }

        /// <summary>
        /// Observações adicionais.
        /// </summary>
        public string? Observacoes { get; set; }
    }
}
