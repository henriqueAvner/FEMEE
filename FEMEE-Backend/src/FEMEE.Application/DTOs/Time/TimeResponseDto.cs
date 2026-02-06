namespace FEMEE.Application.DTOs.Time
{
    /// <summary>
    /// DTO para resposta de time.
    /// Contém informações públicas do time.
    /// </summary>
    public class TimeResponseDto
    {
        /// <summary>
        /// ID único do time.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome do time.
        /// </summary>
        public string Nome { get; set; } = null!;

        /// <summary>
        /// Slug do time.
        /// </summary>
        public string Slug { get; set; } = null!;

        /// <summary>
        /// URL do logo.
        /// </summary>
        public string LogoUrl { get; set; } = null!;

        /// <summary>
        /// Descrição do time.
        /// </summary>
        public string Descricao { get; set; } = null!;

        /// <summary>
        /// Número de vitórias.
        /// </summary>
        public int Vitorias { get; set; }

        /// <summary>
        /// Número de derrotas.
        /// </summary>
        public int Derrotas { get; set; }

        /// <summary>
        /// Número de empates.
        /// </summary>
        public int Empates { get; set; }

        /// <summary>
        /// Pontos totais do time.
        /// </summary>
        public int Pontos { get; set; }

        /// <summary>
        /// Posição no ranking.
        /// </summary>
        public int PosicaoRanking { get; set; }

        /// <summary>
        /// Data de criação do time.
        /// </summary>
        public DateTime DataCriacao { get; set; }
    }
}
