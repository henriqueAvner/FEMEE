
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FEMEE.Domain.Entities.Campeonatos;
using FEMEE.Domain.Enums;

namespace FEMEE.Domain.Entities.Principal
{
    /// <summary>
    /// Entidade Jogador - usa composição com User em vez de herança.
    /// Um Jogador É associado a um User (conta), mas não É um User.
    /// </summary>
    public class Jogador
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// FK para a conta de usuário associada.
        /// </summary>
        [Required]
        [Column("USER_ID")]
        public int UserId { get; set; }

        /// <summary>
        /// Navegação para o usuário.
        /// </summary>
        public User? User { get; set; }

        [Required]
        [MaxLength(256)]
        [Column("NICKNAME")]
        public string NickName { get; set; } = null!;

        [MaxLength(512)]
        [Column("FOTO_URL")]
        public string? FotoUrl { get; set; }

        [Required]
        [Column("FUNCAO")]
        public FuncaoJogador Funcao { get; set; }

        [Column("DATA_ENTRADA_TIME")]
        public DateTime DataEntradaTime { get; set; }

        [Column("DATA_SAIDA_TIME")]
        public DateTime? DataSaidaTime { get; set; }

        [Column("STATUS")]
        public StatusJogador Status { get; set; }

        [Column("TIME_ID")]
        public int? TimeId { get; set; }

        /// <summary>
        /// Time ao qual o jogador pertence.
        /// </summary>
        public Time? Time { get; set; }
    }
}


/*
Na aplicação, ao exibir ou cadastrar, você usa o enum correto conforme o jogo:

Se Jogo.CategoriaJogo == CategoriaJogo.LeagueOfLegends, use FuncaoJogadorLol.
Se Jogo.CategoriaJogo == CategoriaJogo.CounterStrike, use FuncaoJogadorCsgo.
Dessa forma, você mantém a flexibilidade para adicionar mais jogos e suas respectivas funções no futuro.

*/