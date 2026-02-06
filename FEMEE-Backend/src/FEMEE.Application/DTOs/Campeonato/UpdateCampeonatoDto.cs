namespace FEMEE.Application.DTOs.Campeonato
{
    /// <summary>
    /// DTO para atualizar um campeonato existente.
    /// </summary>
    public class UpdateCampeonatoDto
    {
        /// <summary>
        /// Novo título.
        /// </summary>
        public string? Titulo { get; set; }

        /// <summary>
        /// Nova descrição.
        /// </summary>
        public string? Descricao { get; set; }

        /// <summary>
        /// Data de início do campeonato.
        /// </summary>
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// Data de término do campeonato.
        /// </summary>
        public DateTime DataFim { get; set; }

        /// <summary>
        /// Valor total de premiação.
        /// </summary>
        public decimal Premiacao { get; set; }

        /// <summary>
        /// Número de vagas disponíveis.
        /// </summary>
        public int NumeroVagas { get; set; }

        /// <summary>
        /// Data limite para inscrição.
        /// </summary>
        public DateTime DataLimiteInscricao { get; set; }

        /// <summary>
        /// Novo local.
        /// </summary>
        public string? Local { get; set; }

        /// <summary>
        /// Novo regulamento URL.
        /// </summary>
        public string? RegulamentoUrl { get; set; }
    }
}
