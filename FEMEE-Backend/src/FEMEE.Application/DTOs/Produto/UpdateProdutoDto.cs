namespace FEMEE.Application.DTOs.Produto
{
    /// <summary>
    /// DTO para atualizar um produto existente.
    /// </summary>
    public class UpdateProdutoDto
    {
        /// <summary>
        /// Novo nome.
        /// </summary>
        public string Nome { get; set; } = null!;

        /// <summary>
        /// Nova descrição.
        /// </summary>
        public string Descricao { get; set; } = null!;

        /// <summary>
        /// Novo preço.
        /// </summary>
        public decimal Preco { get; set; }

        /// <summary>
        /// Nova imagem.
        /// </summary>
        public string ImagemUrl { get; set; } = null!;

        /// <summary>
        /// Novo estoque.
        /// </summary>
        public int Estoque { get; set; }
    }
}
