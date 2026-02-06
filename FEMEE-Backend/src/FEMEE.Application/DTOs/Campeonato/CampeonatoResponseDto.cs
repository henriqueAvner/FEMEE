using FEMEE.Application.DTOs.Jogo;
using FEMEE.Domain.Enums;

namespace FEMEE.Application.DTOs.Campeonato
{
    /// <summary>
    /// DTO para resposta de campeonato.
    /// </summary>
    public class CampeonatoResponseDto
    {
        /// <summary>
        /// ID único do campeonato.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Título do campeonato.
        /// </summary>
        public string? Titulo { get; set; }

        /// <summary>
        /// Descrição do campeonato.
        /// </summary>
        public string? Descricao { get; set; }

        /// <summary>
        /// Data de início.
        /// </summary>
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// Data de término.
        /// </summary>
        public DateTime DataFim { get; set; }

        /// <summary>
        /// Local do campeonato.
        /// </summary>
        public string? Local { get; set; }

        /// <summary>
        /// Cidade.
        /// </summary>
        public string? Cidade { get; set; }

        /// <summary>
        /// Estado.
        /// </summary>
        public string? Estado { get; set; }

        /// <summary>
        /// Premiação total.
        /// </summary>
        public decimal Premiacao { get; set; }

        /// <summary>
        /// Número de vagas.
        /// </summary>
        public int NumeroVagas { get; set; }

        /// <summary>
        /// Número de times inscritos.
        /// </summary>
        public int NumeroInscritos { get; set; }

        /// <summary>
        /// Status do campeonato.
        /// </summary>
        public StatusCampeonato Status { get; set; }

        /// <summary>
        /// Informações do jogo.
        /// </summary>
        public JogoResponseDto? Jogo { get; set; }
    }
}
