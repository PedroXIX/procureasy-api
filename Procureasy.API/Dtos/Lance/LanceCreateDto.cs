using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Procureasy.API.Dtos.Lance
{
    public class LanceCreateDto
    {
        public decimal Valor { get; set; }
        public string? Observacao { get; set; }
        public int UsuarioId { get; set; }
        public int LeilaoId { get; set; }
    }
}