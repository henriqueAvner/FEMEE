using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FEMEE.Domain.Entities.Campeonatos;
using FEMEE.Domain.Enums;

namespace FEMEE.Domain.Entities.Principal
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(256)]
        [Required]
        [Column("USUARIO")]
        public string? Nome { get; set; }
        [Required]
        [MaxLength(256)]
        [Column("EMAIL")]
        public string? Email { get; set; }

        /// <summary>Hash da senha (ex.: BCrypt). Nunca armazenar senha em texto plano.</summary>
        [Required]
        [Column("SENHA")]
        [MaxLength(256)]
        public string? Senha { get; set; }
        [MaxLength(20)]
        [Required]
        [Column("TELEFONE")]
        public string? Telefone { get; set; }
        [Column("DATA_CRIACAO")]
        public DateTime DataCriacao { get; set; }

        [Column("DATA_ATUALIZACAO")]
        public DateTime DataAtualizacao { get; set; }
        [Column("TIPO_USUARIO")]
        public TipoUsuario TipoUsuario { get; set; }

        public ICollection<Noticia>? Noticias { get; set; } = new List<Noticia>();

        public ICollection<InscricaoCampeonato>? InscricoesCampeonatos { get; set; } = new List<InscricaoCampeonato>();
    }
}