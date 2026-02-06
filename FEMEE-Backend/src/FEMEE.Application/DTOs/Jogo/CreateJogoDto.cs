using FEMEE.Domain.Enums;

namespace FEMEE.Application.DTOs.Jogo
{
    /// <summary>
    /// DTO para criar um novo jogo.
    /// </summary>
    public class CreateJogoDto
    {
        /// <summary>
        /// Nome do jogo.
        /// </summary>
        public string? Nome { get; set; }

        /// <summary>
        /// Slug para URL amigável.
        /// </summary>
        public string? Slug { get; set; }

        /// <summary>
        /// Descrição do jogo.
        /// </summary>
        public string? Descricao { get; set; }

        /// <summary>
        /// URL da imagem do jogo.
        /// </summary>
        public string? ImagemUrl { get; set; }

        /// <summary>
        /// Se o jogo está ativo.
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// Categoria do jogo (FPS, MOBA, etc).
        /// </summary>
        public CategoriaJogo CategoriaJogo { get; set; }
    }
}
