using System;
using System.Collections.Generic;
using Procureasy.API.Models.Enums;

namespace Procureasy.API.Models
{
    public partial class Leilao
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = null!;

        public string Descricao { get; set; } = null!;

        public decimal PrecoInicial { get; set; }

        public decimal? PrecoFinal { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataTermino { get; set; }

        public DateTime DataEntrega { get; set; }

        // Definir o Status como um Enum para garantir valores válidos
        public StatusLeilao Status { get; set; }

        public int ProdutoId { get; set; }

        public int UsuarioId { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime DataAtualizacao { get; set; }

        // Relacionamento com Lances (um Leilao pode ter vários Lances)
        public virtual ICollection<Lance> Lances { get; set; } = new List<Lance>();

        // Relacionamento com Produto (um Leilao tem um Produto)
        public virtual Produto Produto { get; set; } = null!;

        // Relacionamento com Usuario (um Leilao é criado por um Usuario)
        public virtual Usuario Usuario { get; set; } = null!;
    }

}
