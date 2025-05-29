using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Procureasy.API.Models;
using Procureasy.API.Dtos.Usuario;

namespace Procureasy.API.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDto>> GetAllAsync();
        Task<UsuarioDto?> GetByIdAsync(int id);
        Task<(bool Success, string? Message, Usuario? Created)> CreateAsync(UsuarioCreateDto dto);
        Task<(bool Success, string? Message)> UpdateAsync(int id, UsuarioUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ToggleStatusAsync(int id, bool ativo);
    }
}