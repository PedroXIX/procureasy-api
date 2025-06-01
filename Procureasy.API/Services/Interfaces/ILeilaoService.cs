using Procureasy.API.Dtos.Leilao;

namespace Procureasy.API.Services.Interfaces
{
    public interface ILeilaoService
    {
        Task<List<LeilaoDto>> GetAllAsync();
        Task<LeilaoDto?> GetByIdAsync(int id);
        Task<(bool Success, string? Message)> CreateAsync(LeilaoCreateDto dto);
        Task<(bool Success, string? Message)> UpdateAsync(int id, LeilaoUpdateDto dto);
        Task<(bool Success, string? Message)> AddFornecedoresAsync(int leilaoId, List<int> usuariosIds);
        Task<bool> DeleteAsync(int id);
    }
}
