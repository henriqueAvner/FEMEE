using FEMEE.Domain.Enums;

namespace FEMEE.Application.DTOs.Jogador
{
    /// <summary>
    /// DTO para atualizar um jogador existente.
    /// </summary>
    public class UpdateJogadorDto
    {
        /// <summary>
        /// Novo nickname.
        /// </summary>
        public string? NickName { get; set; }

        /// <summary>
        /// URL da foto.
        /// </summary>
        public string? FotoUrl { get; set; }

        /// <summary>
        /// Nova função.
        /// </summary>
        public FuncaoJogador? Funcao { get; set; }

        /// <summary>
        /// Novo status.
        /// </summary>
        public StatusJogador? Status { get; set; }

        /// <summary>
        /// Novo time.
        /// </summary>
        public int? TimeId { get; set; }
    }
}
