using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Procureasy.API.Data;
using Procureasy.API.Dtos;
using Procureasy.API.Helpers;
using Procureasy.API.Models;
using Procureasy.API.Services.Interfaces;

namespace Procureasy.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly ProcurEasyContext _context;
        private readonly IConfiguration _configuration;

        public TokenService(ProcurEasyContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string GenerateToken(LoginDto loginDto)
        {
            var usuarioDataBase = _context.Usuarios.FirstOrDefault(u => u.Email == loginDto.Email);
            if (usuarioDataBase == null || loginDto.Email!= usuarioDataBase.Email|| !PasswordHelper.VerifyPassword(loginDto.Senha, usuarioDataBase.Senha))
            {
                return String.Empty;
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty));
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var signingCredentials = new SigningCredentials(secretKey, algorithm: SecurityAlgorithms.HmacSha256);
            
            var tokenOptions = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims:
                [
                    new Claim(type: ClaimTypes.NameIdentifier, usuarioDataBase.Id.ToString()),
                    new Claim(type: ClaimTypes.Name, usuarioDataBase.Nome),
                    new Claim(type: ClaimTypes.Email, usuarioDataBase.Email),
                    new Claim(type: ClaimTypes.Role, usuarioDataBase.TipoUsuario.ToString()),
                ],
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signingCredentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return token;
        }
    }
}