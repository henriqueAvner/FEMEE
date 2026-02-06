namespace FEMEE.Application.DTOs.Jogo
{
    /// <summary>
    /// DTO para resposta de jogo.
    /// </summary>
    public class JogoResponseDto
    {
        /// <summary>
        /// ID único do jogo.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome do jogo.
        /// </summary>
            public string Nome { get; set; } = null!;

        /// <summary>
        /// Slug do jogo.
        /// </summary>
            public string Slug { get; set; } = null!;

        /// <summary>
        /// Descrição do jogo.
        /// </summary>
            public string Descricao { get; set; } = null!;

        /// <summary>
        /// URL da imagem do jogo.
        /// </summary>
            public string ImagemUrl { get; set; } = null!;

        /// <summary>
        /// Se o jogo está ativo.
        /// </summary>
        public bool Ativo { get; set; }
    }
}
