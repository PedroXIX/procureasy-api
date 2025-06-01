using Procureasy.API.Dtos.Produto;

namespace Procureasy.API.Services.Interfaces
{
    public interface IProdutoService
    {
        Task<List<ProdutoDto>> GetAllAsync();
        Task<ProdutoDto?> GetByIdAsync(int id);
        Task<(bool Success, string? Message)> CreateAsync(ProdutoCreateDto dto);
        Task<(bool Success, string? Message)> UpdateAsync(string codigoProduto, ProdutoUpdateDto dto);
        Task<bool> DeleteAsync(int id); // Inativa o produto
    }
}
