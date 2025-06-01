using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Procureasy.API.Dtos.Lance;

namespace Procureasy.API.Services.Interfaces
{
    public interface ILanceService
    {
        Task<List<LanceDto>> GetAllAsync();
        Task<LanceDto?> GetByIdAsync(int id);
        Task<(bool Success, string? Message)> CreateAsync(LanceCreateDto dto);
        Task<bool> UpdateStatusAsync(int id, bool vencedor);
    }
}