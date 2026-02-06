namespace FEMEE.Application.DTOs.Conquista
{
    /// <summary>
    /// DTO para atualizar uma conquista existente.
    /// </summary>
    public class UpdateConquistaDto
    {
        /// <summary>
        /// ID do time que conquistou.
        /// </summary>
        public int? TimeId { get; set; }

        /// <summary>
        /// ID do campeonato (opcional).
        /// </summary>
        public int? CampeonatoId { get; set; }

        /// <summary>
        /// Título da conquista.
        /// </summary>
        public string? Titulo { get; set; }

        /// <summary>
        /// Descrição da conquista.
        /// </summary>
        public string? Descricao { get; set; }

        /// <summary>
        /// Posição no campeonato (1º, 2º, 3º...).
        /// </summary>
        public int? Posicao { get; set; }

        /// <summary>
        /// Data da conquista.
        /// </summary>
        public DateTime? DataConquista { get; set; }

        /// <summary>
        /// URL do ícone/troféu.
        /// </summary>
        public string? IconeTitulo { get; set; }
    }
}
