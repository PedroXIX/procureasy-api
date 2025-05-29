using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Procureasy.API.Models.Enums;

namespace Procureasy.API.Dtos.Leilao
{
    public class LeilaoDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal PrecoInicial { get; set; }
        public decimal? PrecoFinal { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
        public DateTime DataEntrega { get; set; }
        public StatusLeilao Status { get; set; }
        public int ProdutoId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}