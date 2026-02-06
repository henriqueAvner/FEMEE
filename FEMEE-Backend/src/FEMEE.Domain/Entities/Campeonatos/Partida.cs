using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FEMEE.Domain.Enums;

namespace FEMEE.Domain.Entities.Campeonatos
{
    public class Partida
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("CAMPEONATO_ID")]
        public int CampeonatoId { get; set; }

        [Required]
        [Column("TIME_A_ID")]
        public int TimeAId { get; set; }

        [Required]
        [Column("TIME_B_ID")]
        public int TimeBId { get; set; }

        [Column("TIME_VENCEDOR_ID")]
        public int? TimeVencedorId { get; set; }

        [Required]
        [Column("DATA_HORA")]
        public DateTime DataHora { get; set; }

        [MaxLength(256)]
        [Column("LOCAL")]
        public string? Local { get; set; }

        [Required]
        [Column("FASE")]
        public Fase Fase { get; set; }

        [Column("PLACAR_TIME_A")]
        public int PlacarTimeA { get; set; }

        [Column("PLACAR_TIME_B")]
        public int PlacarTimeB { get; set; }

        [Column("STATUS_PARTIDA")]
        public StatusPartida Status { get; set; }

        [MaxLength(512)]
        [Column("TRANSMISSAO_URL")]
        public string? TransmissaoUrl { get; set; }

        // Propriedades de navegação (sem [Required] pois EF Core gerencia isso)
        public Campeonato? Campeonato { get; set; }
        public Time? TimeA { get; set; }
        public Time? TimeB { get; set; }
        public Time? TimeVencedor { get; set; }
    }
}