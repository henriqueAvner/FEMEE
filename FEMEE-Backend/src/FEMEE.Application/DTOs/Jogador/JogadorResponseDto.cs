using FEMEE.Application.DTOs.User;
using FEMEE.Domain.Enums;

namespace FEMEE.Application.DTOs.Jogador
{
    /// <summary>
    /// DTO para resposta de jogador.
    /// </summary>
    public class JogadorResponseDto
    {
        /// <summary>
        /// ID único do jogador.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nickname do jogador.
        /// </summary>
        public string NickName { get; set; } = null!;

        /// <summary>
        /// Nome completo do jogador.
        /// </summary>
        public string NomeCompleto { get; set; } = null!;

        /// <summary>
        /// Função do jogador.
        /// </summary>
        public FuncaoJogador Funcao { get; set; }

        /// <summary>
        /// Data de entrada no time.
        /// </summary>
        public DateTime DataEntradaTime { get; set; }

        /// <summary>
        /// Status do jogador.
        /// </summary>
        public StatusJogador Status { get; set; }

        /// <summary>
        /// Informações do usuário associado.
        /// </summary>
        public UserResponseDto User { get; set; } = null!;
    }
}
