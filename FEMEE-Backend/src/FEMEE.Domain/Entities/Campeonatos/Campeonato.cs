using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FEMEE.Domain.Entities.Principal;
using FEMEE.Domain.Enums;

namespace FEMEE.Domain.Entities.Campeonatos
{
    public class Campeonato
    {
        [Key]
        public int Id { get; set; }
        
        public int JogoId { get; set; }

        [Required]
        [MaxLength(256)]
        [Column("TITULO")]
        public string? Titulo { get; set; }
        [MaxLength(1000)]
        public string? Descricao { get; set; }
        [Column("DATA_INICIO")]
        public DateTime DataInicio { get; set; }
        [Column("DATA_FIM")]
        public DateTime DataFim { get; set; }

        [MaxLength(512)]
        [Column("BANNER_CAMPEONATO_URL")]
        public string? ImagemUrl { get; set; }

        [Required]
        [Column("DATA_LIMITE_INSCRICAO")]
        public DateTime DataLimiteInscricao { get; set; }

        [Required]
        [MaxLength(256)]
        [Column("LOCAL")]
        public string? Local { get; set; }

        [MaxLength(128)]
        [Column("CIDADE")]
        public string? Cidade { get; set; }

        [MaxLength(2)]
        [Column("ESTADO")]
        public string? Estado { get; set; }
        
        [Column("PREMIACAO")]
        //Certificar de que o DTO de saida formate como string 
        // para bater com o front-end (R$ 1.000,00)
        public decimal Premiacao { get; set; }

        [Column("NUMERO_VAGAS")]
        public int NumeroVagas { get; set; }

        [Column("NUMERO_INSCRITOS")]
        public int NumeroInscritos { get; set; }

        [Column("STATUS_CAMPEONATO")]
        public StatusCampeonato Status { get; set; }
        [Column("FASE_CAMPEONATO")]
        public Fase FaseCampeonato { get; set; }
        [MaxLength(512)]
        [Column("REGULAMENTO_URL")]
        public string? RegulamentoUrl { get; set; }
        [Column("CREATED_AT")]
        public DateTime CreatedAt { get; set; }
        [Column("UPDATED_AT")]
        public DateTime UpdatedAt { get; set; }

        public Jogo? Jogo { get; set; }
        public ICollection<Partida> Partidas { get; set; } = new List<Partida>();
        public ICollection<InscricaoCampeonato> InscricoesCampeonatos { get; set; } = new List<InscricaoCampeonato>();

        public ICollection<Conquista> Conquistas { get; set; } = new List<Conquista>();
    }
}