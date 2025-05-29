using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Procureasy.API.Dtos.Produto
{
    public class ProdutoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}