using System;
using System.Collections.Generic;

namespace Procureasy.API.Models;

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

    public string Status { get; set; } = null!;

    public int ProdutoId { get; set; }

    public int UsuarioId { get; set; }

    public DateTime DataCriacao { get; set; }

    public DateTime DataAtualizacao { get; set; }

    public virtual ICollection<Lance> Lances { get; set; } = new List<Lance>();

    public virtual Produto Produto { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
