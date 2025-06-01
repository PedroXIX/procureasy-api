using Microsoft.EntityFrameworkCore;
using Procureasy.API.Data;
using Procureasy.API.Dtos.Produto;
using Procureasy.API.Models;
using Procureasy.API.Services.Interfaces;

namespace Procureasy.API.Services;

public class ProdutoService : IProdutoService
{
    private readonly ProcurEasyContext _context;

    public ProdutoService(ProcurEasyContext context)
    {
        _context = context;
    }

    public async Task<List<ProdutoDto>> GetAllAsync()
    {
        return await _context.Produtos
            .Select(p => new ProdutoDto
            {
                Id = p.Id,
                CodigoProduto = p.CodigoProduto,
                Nome = p.Nome,
                Quantidade = p.Quantidade,
                Valor = p.Valor,
                Descricao = p.Descricao,
                Area = p.Area,
                Ativo = p.Ativo,
                DataCriacao = p.DataCriacao,
                DataAtualizacao = p.DataAtualizacao
            }).ToListAsync();
    }

    public async Task<ProdutoDto?> GetByIdAsync(int id)
    {
        var p = await _context.Produtos.FindAsync(id);
        if (p == null) return null;

        return new ProdutoDto
        {
            Id = p.Id,
            CodigoProduto = p.CodigoProduto,
            Nome = p.Nome,
            Quantidade = p.Quantidade,
            Valor = p.Valor,
            Descricao = p.Descricao,
            Area = p.Area,
            Ativo = p.Ativo,
            DataCriacao = p.DataCriacao,
            DataAtualizacao = p.DataAtualizacao
        };
    }

    public async Task<(bool Success, string? Message)> CreateAsync(ProdutoCreateDto dto)
    {
        if (dto.Quantidade < 0)
            return (false, "A quantidade não pode ser negativa.");

        if (dto.Valor < 0)
            return (false, "O valor não pode ser negativo.");

        //Verificar unicidade do CodigoProduto
        if (await _context.Produtos.AnyAsync(p => p.CodigoProduto == dto.CodigoProduto))
            return (false, $"O Código do Produto '{dto.CodigoProduto}' já existe.");

        var produto = new Produto
        {
            CodigoProduto = dto.CodigoProduto,
            Nome = dto.Nome,
            Quantidade = dto.Quantidade,
            Valor = dto.Valor,
            Descricao = dto.Descricao,
            Area = dto.Area,
            Ativo = true,
            DataCriacao = DateTime.UtcNow,
            DataAtualizacao = DateTime.UtcNow
        };

        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();

        return (true, null);
    }

    public async Task<(bool Success, string? Message)> UpdateAsync(string codigoProduto, ProdutoUpdateDto dto)
    {
        var produto = await _context.Produtos
            .FirstOrDefaultAsync(p => p.CodigoProduto == codigoProduto);

        if (produto == null)
            return (false, "Produto não encontrado.");

        if (dto.Quantidade < 0)
            return (false, "A quantidade não pode ser negativa.");

        if (dto.Valor < 0)
            return (false, "O valor não pode ser negativo.");

        // Atualiza os campos permitidos
        produto.Nome = dto.Nome;
        produto.Quantidade = dto.Quantidade;
        produto.Valor = dto.Valor;
        produto.Descricao = dto.Descricao;
        produto.Area = dto.Area;
        produto.DataAtualizacao = DateTime.UtcNow;

        _context.Entry(produto).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return (true, null);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto == null)
            return false;

        produto.Ativo = false;
        produto.DataAtualizacao = DateTime.UtcNow;

        _context.Entry(produto).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return true;
    }
}

