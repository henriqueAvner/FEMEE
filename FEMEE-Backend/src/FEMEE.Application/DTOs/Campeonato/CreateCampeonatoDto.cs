namespace FEMEE.Application.DTOs.Campeonato
{
    /// <summary>
    /// DTO para criar um novo campeonato.
    /// </summary>
    public class CreateCampeonatoDto
    {
        /// <summary>
        /// ID do jogo associado ao campeonato.
        /// </summary>
        public int JogoId { get; set; }

        /// <summary>
        /// Título do campeonato.
        /// </summary>
        public string? Titulo { get; set; }

        /// <summary>
        /// Descrição detalhada do campeonato.
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
        /// Data limite para inscrição.
        /// </summary>
        public DateTime DataLimiteInscricao { get; set; }

        /// <summary>
        /// Local onde será realizado.
        /// </summary>
        public string? Local { get; set; }

        /// <summary>
        /// Cidade do campeonato.
        /// </summary>
        public string? Cidade { get; set; }

        /// <summary>
        /// Estado do campeonato.
        /// </summary>
        public string? Estado { get; set; }

        /// <summary>
        /// Valor total de premiação.
        /// </summary>
        public decimal Premiacao { get; set; }

        /// <summary>
        /// Número de vagas disponíveis.
        /// </summary>
        public int NumeroVagas { get; set; }

        /// <summary>
        /// URL do regulamento do campeonato.
        /// </summary>
        public string? RegulamentoUrl { get; set; }
    }
}
