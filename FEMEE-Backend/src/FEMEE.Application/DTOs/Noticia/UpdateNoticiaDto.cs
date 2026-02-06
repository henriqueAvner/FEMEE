namespace FEMEE.Application.DTOs.Noticia
{
    /// <summary>
    /// DTO para atualizar uma notícia existente.
    /// </summary>
    public class UpdateNoticiaDto
    {
        /// <summary>
        /// Novo título.
        /// </summary>
        public string Titulo { get; set; } = null!;

        /// <summary>
        /// Novo resumo.
        /// </summary>
        public string Resumo { get; set; } = null!;

        /// <summary>
        /// Novo conteúdo.
        /// </summary>
        public string Conteudo { get; set; } = null!;

        /// <summary>
        /// Nova categoria.
        /// </summary>
        public string Categoria { get; set; } = null!;

        /// <summary>
        /// Nova imagem.
        /// </summary>
        public string ImagemUrl { get; set; } = null!;
    }
}
