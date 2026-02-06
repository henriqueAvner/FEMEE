using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FEMEE.Domain.Entities.Campeonatos;

namespace FEMEE.Domain.Entities.Principal
{
    public class Conquista
    {
        [Key]
        public int Id { get; set; }
        [Column("TIME_ID")]
        public int TimeId { get; set; }
        [Column("CAMPEONATO_ID")]
        public int? CampeonatoId { get; set; }
        [Required]
        [MaxLength(256)]
        [Column("TITULO")]
        public string? Titulo { get; set; }
        [MaxLength(500)]
        [Column("DESCRICAO")]
        public string? Descricao { get; set; }
        [Column("POSICAO")]
        public int Posicao { get; set; }
        [Column("DATA_CONQUISTA")]
        public DateTime DataConquista { get; set; }
        [MaxLength(256)]
        [Column("ICONE_TITULO")]
        public string? IconeTitulo { get; set; }

        public Time? Time { get; set; }
        public Campeonato? Campeonato { get; set; }
    }
}