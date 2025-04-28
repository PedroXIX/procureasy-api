using System;
using System.Collections.Generic;
using Procureasy.API.Models.Enums;

namespace Procureasy.API.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public string Cnpj { get; set; }  = null!;

    public string Cpf { get; set; }  = null!;

    public TipoUsuario TipoUsuario { get; set; } 

    public bool Ativo { get; set; }

    public DateTime DataCriacao { get; set; }

    public virtual ICollection<Lance> Lances { get; set; } = new List<Lance>();

    public virtual ICollection<Leilao> Leiloes { get; set; } = new List<Leilao>();

    public Usuario(){

    }

    public Usuario(int id, string nome, string email, string cnpj, string cpf, TipoUsuario tipoUsuario,
    bool ativo, DateTime dataCriacao, ICollection<Lance> lances, ICollection<Leilao> leiloes){
        Id = id;
        Nome = nome;
        Email = email;
        Cnpj = cnpj;
        TipoUsuario = tipoUsuario;
        Ativo = ativo;
        DataCriacao = dataCriacao;
        Lances = lances;
        Leiloes = leiloes;
    }

}
