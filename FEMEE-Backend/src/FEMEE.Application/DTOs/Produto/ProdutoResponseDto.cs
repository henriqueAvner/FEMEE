namespace FEMEE.Application.DTOs.Produto
{
    /// <summary>
    /// DTO para resposta de produto.
    /// </summary>
    public class ProdutoResponseDto
    {
        /// <summary>
        /// ID único do produto.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome do produto.
        /// </summary>
            public string Nome { get; set; } = null!;

        /// <summary>
        /// Descrição.
        /// </summary>
            public string Descricao { get; set; } = null!;

        /// <summary>
        /// Preço.
        /// </summary>
        public decimal Preco { get; set; }

        /// <summary>
        /// URL da imagem.
        /// </summary>
            public string ImagemUrl { get; set; } = null!;

        /// <summary>
        /// Categoria.
        /// </summary>
            public string Categoria { get; set; } = null!;

        /// <summary>
        /// Quantidade em estoque.
        /// </summary>
        public int Estoque { get; set; }

        /// <summary>
        /// Se o produto está ativo.
        /// </summary>
        public bool Ativo { get; set; }
    }
}
