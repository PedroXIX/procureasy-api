using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Procureasy.API.Dtos
{
    public record LoginDto
    {
        public required string Email { get; set; }
        public required string Senha { get; set; }
    }
}