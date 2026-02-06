namespace FEMEE.Application.DTOs.Conquista
{
    /// <summary>
    /// DTO para resposta de conquista.
    /// </summary>
    public class ConquistaResponseDto
    {
        /// <summary>
        /// ID único da conquista.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Título da conquista.
        /// </summary>
        public string Titulo { get; set; } = null!;

        /// <summary>
        /// Descrição da conquista.
        /// </summary>
        public string Descricao { get; set; } = null!;

        /// <summary>
        /// Posição/ranking da conquista.
        /// </summary>
        public int Posicao { get; set; }

        /// <summary>
        /// Data da conquista.
        /// </summary>
        public DateTime DataConquista { get; set; }

        /// <summary>
        /// URL do ícone/badge.
        /// </summary>
        public string IconeTitulo { get; set; } = null!;
    }
}
