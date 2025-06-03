using Procureasy.API.Dtos.Leilao;

namespace Procureasy.API.Services.Interfaces
{
    public interface ILeilaoService
    {
        Task<List<LeilaoDto>> GetAllAsync();
        Task<LeilaoDto?> GetByIdAsync(int id);
        Task<(bool Success, string? Message, LeilaoDto? Leilao)> CreateAsync(LeilaoCreateDto dto);
        Task<(bool Success, string? Message)> UpdateAsync(int id, LeilaoUpdateDto dto);
        Task<(bool Success, string? Message)> AddFornecedoresAsync(int leilaoId, List<int> usuariosIds);
        Task<List<LeilaoDto>> GetLeiloesPorFornecedorAsync(int fornecedorId);
        Task<bool> AtualizarStatusCanceladoAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
