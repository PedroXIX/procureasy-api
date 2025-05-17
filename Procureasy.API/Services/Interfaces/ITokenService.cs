using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Procureasy.API.Dtos;
using Procureasy.API.Models;

namespace Procureasy.API.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(LoginDto usuario);
    }
}