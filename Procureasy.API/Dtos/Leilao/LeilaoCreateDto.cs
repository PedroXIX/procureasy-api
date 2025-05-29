using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Procureasy.API.Dtos.Leilao
{
    public class LeilaoCreateDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal PrecoInicial { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
        public DateTime DataEntrega { get; set; }
        public int ProdutoId { get; set; }
        public int UsuarioId { get; set; }
    }
}