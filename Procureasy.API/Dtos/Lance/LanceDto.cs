using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Procureasy.API.Dtos.Lance
{
    public class LanceDto
    {
        public int Id { get; set; }
        public decimal Valor { get; set; }
        public bool Vencedor { get; set; }
        public string? Observacao { get; set; }
        public int UsuarioId { get; set; }
        public int LeilaoId { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}