using Microsoft.EntityFrameworkCore;
using Procureasy.API.Data;
using Procureasy.API.Dtos.Leilao;
using Procureasy.API.Models;
using Procureasy.API.Services.Interfaces;

namespace Procureasy.API.Services;

public class LeilaoService : ILeilaoService
{
    private readonly ProcurEasyContext _context;

    public LeilaoService(ProcurEasyContext context)
    {
        _context = context;
    }

    public async Task<List<LeilaoDto>> GetAllAsync()
    {
        return await _context.Leiloes
            .Select(l => new LeilaoDto
            {
                Id = l.Id,
                Titulo = l.Titulo,
                Descricao = l.Descricao,
                DataInicio = l.DataInicio,
                DataFim = l.DataFim,
                Ativo = l.Ativo,
                UsuarioId = l.UsuarioId
            }).ToListAsync();
    }

    public async Task<LeilaoDto?> GetByIdAsync(int id)
    {
        var l = await _context.Leiloes.FindAsync(id);
        if (l == null) return null;

        return new LeilaoDto
        {
            Id = l.Id,
            Titulo = l.Titulo,
            Descricao = l.Descricao,
            DataInicio = l.DataInicio,
            DataFim = l.DataFim,
            Ativo = l.Ativo,
            UsuarioId = l.UsuarioId
        };
    }

    public async Task<(bool Success, string? Message)> CreateAsync(LeilaoCreateDto dto)
{
    if (!await _context.Usuarios.AnyAsync(u => u.Id == dto.UsuarioId))
        return (false, "Usuário não encontrado.");

    if (!await _context.Produtos.AnyAsync(p => p.Id == dto.ProdutoId))
        return (false, "Produto não encontrado.");

    if (dto.DataTermino <= dto.DataInicio)
        return (false, "Data de término deve ser após a data de início.");

    var leilao = new Leilao
    {
        Titulo = dto.Titulo,
        Descricao = dto.Descricao,
        PrecoInicial = dto.PrecoInicial,
        PrecoFinal = null,
        DataInicio = dto.DataInicio,
        DataTermino = dto.DataTermino,
        DataEntrega = dto.DataEntrega,
        Status = StatusLeilao.ABERTO,
        UsuarioId = dto.UsuarioId,
        ProdutoId = dto.ProdutoId,
        DataCriacao = DateTime.UtcNow,
        DataAtualizacao = DateTime.UtcNow
    };

    _context.Leiloes.Add(leilao);
    await _context.SaveChangesAsync();

    return (true, null);
}

public async Task<(bool Success, string? Message)> UpdateAsync(int id, LeilaoUpdateDto dto)
{
    var leilao = await _context.Leiloes.FindAsync(id);
    if (leilao == null)
        return (false, "Leilão não encontrado.");

    if (dto.DataTermino <= dto.DataInicio)
        return (false, "Data de término deve ser após a data de início.");

    leilao.Titulo = dto.Titulo;
    leilao.Descricao = dto.Descricao;
    leilao.PrecoInicial = dto.PrecoInicial;
    leilao.DataInicio = dto.DataInicio;
    leilao.DataTermino = dto.DataTermino;
    leilao.DataEntrega = dto.DataEntrega;
    leilao.DataAtualizacao = DateTime.UtcNow;

    _context.Entry(leilao).State = EntityState.Modified;
    await _context.SaveChangesAsync();

    return (true, null);
}
    public async Task<bool> DeleteAsync(int id)
    {
        var leilao = await _context.Leiloes.FindAsync(id);
        if (leilao == null)
            return false;

        _context.Leiloes.Remove(leilao);
        await _context.SaveChangesAsync();
        return true;
    }
}
