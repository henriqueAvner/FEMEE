namespace FEMEE.Application.DTOs.Common
{
    /// <summary>
    /// DTO genérico para resultados paginados.
    /// Usado para retornar listas com informações de paginação.
    /// </summary>
    /// <typeparam name="T">Tipo dos itens na lista</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// Itens da página atual.
        /// </summary>
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

        /// <summary>
        /// Total de itens em todas as páginas.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Número da página atual (1-based).
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Quantidade de itens por página.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total de páginas calculado.
        /// </summary>
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 0;

        /// <summary>
        /// Indica se há página anterior.
        /// </summary>
        public bool HasPreviousPage => Page > 1;

        /// <summary>
        /// Indica se há próxima página.
        /// </summary>
        public bool HasNextPage => Page < TotalPages;

        /// <summary>
        /// Construtor padrão.
        /// </summary>
        public PagedResult()
        {
        }

        /// <summary>
        /// Construtor com parâmetros.
        /// </summary>
        public PagedResult(IEnumerable<T> items, int totalCount, int page, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            Page = page;
            PageSize = pageSize;
        }

        /// <summary>
        /// Cria um PagedResult a partir de uma query e parâmetros de paginação.
        /// </summary>
        public static PagedResult<T> Create(IEnumerable<T> items, int totalCount, int page, int pageSize)
        {
            return new PagedResult<T>(items, totalCount, page, pageSize);
        }
    }
}
