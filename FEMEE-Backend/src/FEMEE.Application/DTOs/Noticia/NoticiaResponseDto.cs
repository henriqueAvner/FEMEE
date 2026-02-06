using FEMEE.Application.DTOs.User;

namespace FEMEE.Application.DTOs.Noticia
{
    /// <summary>
    /// DTO para resposta de notícia.
    /// </summary>
    public class NoticiaResponseDto
    {
        /// <summary>
        /// ID único da notícia.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Título da notícia.
        /// </summary>
        public string Titulo { get; set; } = null!;

        /// <summary>
        /// Slug da notícia.
        /// </summary>
        public string Slug { get; set; } = null!;

        /// <summary>
        /// Resumo da notícia.
        /// </summary>
        public string Resumo { get; set; } = null!;

        /// <summary>
        /// Conteúdo completo.
        /// </summary>
        public string Conteudo { get; set; } = null!;

        /// <summary>
        /// Categoria.
        /// </summary>
        public string Categoria { get; set; } = null!;

        /// <summary>
        /// URL da imagem.
        /// </summary>
        public string ImagemUrl { get; set; } = null!;

        /// <summary>
        /// Data de publicação.
        /// </summary>
        public DateTime DataPublicacao { get; set; }

        /// <summary>
        /// Número de visualizações.
        /// </summary>
        public int Visualizacoes { get; set; }

        /// <summary>
        /// Informações do autor.
        /// </summary>
        public UserResponseDto Autor { get; set; } = null!;
    }
}
