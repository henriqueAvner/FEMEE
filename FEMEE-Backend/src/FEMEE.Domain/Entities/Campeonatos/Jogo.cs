using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FEMEE.Domain.Enums;

namespace FEMEE.Domain.Entities.Campeonatos
{
    public class Jogo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(256)]
        [Column("SLUG")]
        public string? Slug { get; set; }
        [Required]
        [MaxLength(256)]
        [Column("NOME")]
        public string? Nome { get; set; }
        [MaxLength(1000)]
        [Column("DESCRICAO")]
        public string? Descricao { get; set; }
        [MaxLength(512)]
        [Column("IMAGEM_URL")]
        public string? ImagemUrl { get; set; }
        [Column("ATIVO")]
        public bool Ativo { get; set; }
        [Column("CATEGORIA_JOGO")]
        public CategoriaJogo CategoriaJogo { get; set; }

        public ICollection<Campeonato> Campeonatos { get; set; } = new List<Campeonato>();
    }
}