using System;
using System.Collections.Generic;

namespace Procureasy.API.Models;

public partial class Produto
{
    public int Id { get; set; }
    public required string CodigoProduto { get; set; }

    public string Nome { get; set; } = null!;

    public int Quantidade { get; set; }

    public decimal Valor { get; set; }

    public string Descricao { get; set; } = null!;

    public string Area { get; set; } = null!;

    public bool Ativo { get; set; }

    public DateTime DataCriacao { get; set; }

    public DateTime DataAtualizacao { get; set; }

    public virtual ICollection<Leilao> Leiloes { get; set; } = new List<Leilao>();
}
