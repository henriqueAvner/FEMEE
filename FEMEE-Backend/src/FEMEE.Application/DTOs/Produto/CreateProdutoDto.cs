namespace FEMEE.Application.DTOs.Produto
{
    /// <summary>
    /// DTO para criar um novo produto.
    /// </summary>
    public class CreateProdutoDto
    {
        /// <summary>
        /// Nome do produto.
        /// </summary>
        public string Nome { get; set; } = null!;

        /// <summary>
        /// Descrição do produto.
        /// </summary>
        public string Descricao { get; set; } = null!;

        /// <summary>
        /// Preço do produto.
        /// </summary>
        public decimal Preco { get; set; }

        /// <summary>
        /// URL da imagem do produto.
        /// </summary>
        public string ImagemUrl { get; set; } = null!;

        /// <summary>
        /// Categoria do produto.
        /// </summary>
        public string Categoria { get; set; } = null!;

        /// <summary>
        /// Quantidade em estoque.
        /// </summary>
        public int Estoque { get; set; }
    }
}
