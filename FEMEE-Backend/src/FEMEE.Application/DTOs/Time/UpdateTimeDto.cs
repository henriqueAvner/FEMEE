namespace FEMEE.Application.DTOs.Time
{
    /// <summary>
    /// DTO para atualizar um time existente.
    /// </summary>
    public class UpdateTimeDto
    {
        /// <summary>
        /// Novo nome do time.
        /// </summary>
        public string Nome { get; set; } = null!;

        /// <summary>
        /// Slug do time (URL-friendly).
        /// Exemplo: "thunder-gaming"
        /// </summary>
        public string Slug { get; set; } = null!;

        /// <summary>
        /// Nova descrição do time.
        /// </summary>
        public string Descricao { get; set; } = null!;

        /// <summary>
        /// Novo URL do logo.
        /// </summary>
        public string LogoUrl { get; set; } = null!;
    }
}
