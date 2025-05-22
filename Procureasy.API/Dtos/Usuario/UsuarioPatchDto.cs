using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Procureasy.API.Models.Enums;

namespace Procureasy.API.Dtos.Usuario
{
    // DTOs/UsuarioDto.cs
    public class UsuarioPatchDto
    {
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
        public string? Cpf { get; set; }
        public string? Cnpj { get; set; }
        public TipoUsuario? TipoUsuario { get; set; }
    }
}