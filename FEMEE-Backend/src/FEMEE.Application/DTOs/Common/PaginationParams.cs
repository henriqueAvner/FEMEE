namespace FEMEE.Application.DTOs.Common
{
    /// <summary>
    /// DTO para parâmetros de paginação nas requisições.
    /// </summary>
    public class PaginationParams
    {
        private const int MaxPageSize = 100;
        private const int DefaultPageSize = 10;
        private int _pageSize = DefaultPageSize;
        private int _page = 1;

        /// <summary>
        /// Número da página (1-based). Padrão: 1.
        /// </summary>
        public int Page
        {
            get => _page;
            set => _page = value < 1 ? 1 : value;
        }

        /// <summary>
        /// Quantidade de itens por página. Padrão: 10. Máximo: 100.
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : (value < 1 ? DefaultPageSize : value);
        }

        /// <summary>
        /// Campo para ordenação (opcional).
        /// </summary>
        public string? SortBy { get; set; }

        /// <summary>
        /// Direção da ordenação: "asc" ou "desc". Padrão: "asc".
        /// </summary>
        public string SortDirection { get; set; } = "asc";

        /// <summary>
        /// Termo de busca (opcional).
        /// </summary>
        public string? Search { get; set; }

        /// <summary>
        /// Indica se a ordenação é descendente.
        /// </summary>
        public bool IsDescending => SortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Calcula quantos itens pular para a paginação.
        /// </summary>
        public int Skip => (Page - 1) * PageSize;
    }
}
