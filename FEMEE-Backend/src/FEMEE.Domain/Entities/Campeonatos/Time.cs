using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FEMEE.Domain.Entities.Principal;

namespace FEMEE.Domain.Entities.Campeonatos
{
    public class Time
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(256)]
        [Column("TIME_NOME")]
        public string? Nome { get; set; }

        [Column("TITULOS_TIME")]
        [Required]
        public int TitulosTime { get; set; }

        [MaxLength(256)]
        [Column("SLUG")]
        [Required]
        public string? Slug { get; set; }
        [MaxLength(512)]
        [Column("LOGO_URL")]
        public string? LogoUrl { get; set; }
        [Column("DATA_FUNDACAO")]
        public DateTime DataFundacao { get; set; }
        [MaxLength(1000)]
        [Column("DESCRICAO")]
        public string? Descricao { get; set; }
        [Column("VITORIAS")]
        public int Vitorias { get; set; }
        [Column("DERROTAS")]
        public int Derrotas { get; set; }

        [Column("EMPATES")]
        public int Empates { get; set; }
        [Column("PONTOS")]
        public int Pontos { get; set; }
        [Column("POSICAO_RANKING")]
        public int PosicaoRanking { get; set; }
        [Column("POSICAO_ANTERIOR")]
        public int PosicaoAnterior { get; set; }
        [Column("CREATED_AT")]
        public DateTime CreatedAt { get; set; }
        [Column("UPDATED_AT")]
        public DateTime UpdatedAt { get; set; }

        public ICollection<Jogador> Jogadores { get; set; } = new List<Jogador>();
        public ICollection<Partida> PartidasCasaTeam { get; set; } = new List<Partida>();
        public ICollection<Partida> PartidasVisitanteTeam { get; set; } = new List<Partida>();
        public ICollection<InscricaoCampeonato> InscricoesCampeonatos { get; set; } = new List<InscricaoCampeonato>();
        public ICollection<Conquista> Conquistas { get; set; } = new List<Conquista>();
    }
}
