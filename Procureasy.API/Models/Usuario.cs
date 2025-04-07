using System;
using System.Collections.Generic;

namespace Procureasy.API.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public string? Cnpj { get; set; }

    public string? Cpf { get; set; }

    public string TipoUsuario { get; set; } = null!;

    public bool Ativo { get; set; }

    public DateTime DataCriacao { get; set; }

    public virtual ICollection<Lance> Lances { get; set; } = new List<Lance>();

    public virtual ICollection<Leilao> Leiloes { get; set; } = new List<Leilao>();
}
