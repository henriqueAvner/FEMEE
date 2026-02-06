using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FEMEE.Domain.Entities.Store
{
    public class Produto
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public string? ImagemUrl { get; set; }
        public string? Categoria { get; set; }
        public int Estoque { get; set; }
        public bool Ativo { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}