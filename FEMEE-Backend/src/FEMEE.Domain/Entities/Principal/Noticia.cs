using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FEMEE.Domain.Entities.Principal
{
    public class Noticia
    {
        [Key]
        public int Id { get; set; }

        [Column("AUTOR_ID")]
        public int AutorId { get; set; }
        [Required]
        [MaxLength(512)]
        [Column("TITULO")]
        public string? Titulo { get; set; }
        [Required]
        [MaxLength(512)]
        [Column("SLUG")]
        public string? Slug { get; set; }


        [MaxLength(1024)]
        [Column("RESUMO")]
        //Garantir que ele irá alimentar os cards da Home
        public string? Resumo { get; set; }

        [Column("CONTEUDO")]
        public string? Conteudo { get; set; }
        [MaxLength(128)]
        [Column("CATEGORIA")]
        public string? Categoria { get; set; }

        [Column("NOTICIA_URL")]
        [MaxLength(512)]
        public string? ImagemUrl { get; set; }

        [Column("DATA_PUBLICACAO")]
        public DateTime DataPublicacao { get; set; }
        [Column("NUMERO_COMENTARIOS")]
        public int NumeroComentarios { get; set; }
        [Column("VISUALIZACOES")]
        public int Visualizacoes { get; set; }
        /// <summary>Indica se a notícia está publicada e visível.</summary>
        [Column("PUBLICADA")]
        public bool Publicada { get; set; }

        public User? Autor { get; set; }

    }
}