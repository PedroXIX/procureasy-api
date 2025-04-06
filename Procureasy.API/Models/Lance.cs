using System;
using System.Collections.Generic;

namespace Procureasy.API.Models;

public partial class Lance
{
    public int Id { get; set; }

    public decimal Valor { get; set; }

    public bool Vencedor { get; set; }

    public string? Observacao { get; set; }

    public int UsuarioId { get; set; }

    public int LeilaoId { get; set; }

    public DateTime DataCriacao { get; set; }

    public virtual Leilao Leilao { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
