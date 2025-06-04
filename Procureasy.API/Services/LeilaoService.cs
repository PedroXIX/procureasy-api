using Microsoft.EntityFrameworkCore;
using Procureasy.API.Data;
using Procureasy.API.Dtos.Leilao;
using Procureasy.API.Models;
using Procureasy.API.Models.Enums;
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
                PrecoInicial = l.PrecoInicial,
                PrecoFinal = l.PrecoFinal,
                DataInicio = l.DataInicio,
                DataTermino = l.DataTermino,
                DataEntrega = l.DataEntrega,
                Status = l.Status,
                ProdutoId = l.ProdutoId,
                UsuarioId = l.UsuarioId,
                DataCriacao = l.DataCriacao,
                DataAtualizacao = l.DataAtualizacao
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
            PrecoInicial = l.PrecoInicial,
            PrecoFinal = l.PrecoFinal,
            DataInicio = l.DataInicio,
            DataTermino = l.DataTermino,
            DataEntrega = l.DataEntrega,
            Status = l.Status,
            ProdutoId = l.ProdutoId,
            UsuarioId = l.UsuarioId,
            DataCriacao = l.DataCriacao,
            DataAtualizacao = l.DataAtualizacao
        };
    }

    public async Task<(bool Success, string? Message, LeilaoDto? Leilao)> CreateAsync(LeilaoCreateDto dto)
    {
        // Validações iniciais (mantidas iguais)
        if (!await _context.Usuarios.AnyAsync(u => u.Id == dto.UsuarioId))
            return (false, "Usuário não encontrado.", null);

        if (!await _context.Produtos.AnyAsync(p => p.Id == dto.ProdutoId))
            return (false, "Produto não encontrado.", null);

        if (dto.DataTermino <= dto.DataInicio)
            return (false, "Data de término deve ser posterior à data de início.", null);

        // Criação do leilão (mantida igual)
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

        // Novo: Retorna o DTO do leilão criado
        var leilaoDto = new LeilaoDto
        {
            Id = leilao.Id,
            Titulo = leilao.Titulo,
            Descricao = leilao.Descricao,
            PrecoInicial = leilao.PrecoInicial,
            PrecoFinal = leilao.PrecoFinal,
            DataInicio = leilao.DataInicio,
            DataTermino = leilao.DataTermino,
            DataEntrega = leilao.DataEntrega,
            Status = leilao.Status,
            ProdutoId = leilao.ProdutoId,
            UsuarioId = leilao.UsuarioId,
            DataCriacao = leilao.DataCriacao,
            DataAtualizacao = leilao.DataAtualizacao
        };

        return (true, null, leilaoDto);
    }

    public async Task<(bool Success, string? Message)> AddFornecedoresAsync(int leilaoId, List<int> usuariosIds)
    {
        var leilao = await _context.Leiloes.FindAsync(leilaoId);
        if (leilao == null)
            return (false, "Leilão não encontrado.");

        // Validação: garantir que todos os usuários existem
        var usuarios = await _context.Usuarios
            .Where(u => usuariosIds.Contains(u.Id))
            .ToListAsync();

        if (usuarios.Count != usuariosIds.Count)
            return (false, "Um ou mais fornecedores não existem.");

        foreach (var usuario in usuarios)
        {
            // Evita duplicação
            bool jaExiste = await _context.LeilaoUsuarios
                .AnyAsync(lu => lu.LeilaoId == leilaoId && lu.UsuarioId == usuario.Id);

            if (!jaExiste)
            {
                _context.LeilaoUsuarios.Add(new LeilaoUsuario
                {
                    LeilaoId = leilaoId,
                    UsuarioId = usuario.Id
                });
            }
        }

        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<List<LeilaoDto>> GetLeiloesPorFornecedorAsync(int fornecedorId)
    {
        var leiloes = await _context.LeilaoUsuarios
            .Where(lu => lu.UsuarioId == fornecedorId)
            .Select(lu => new LeilaoDto
            {
                Id = lu.Leilao.Id,
                Titulo = lu.Leilao.Titulo,
                Descricao = lu.Leilao.Descricao,
                PrecoInicial = lu.Leilao.PrecoInicial,
                PrecoFinal = lu.Leilao.PrecoFinal,
                DataInicio = lu.Leilao.DataInicio,
                DataTermino = lu.Leilao.DataTermino,
                DataEntrega = lu.Leilao.DataEntrega,
                Status = lu.Leilao.Status
            })
            .ToListAsync();

        return leiloes;
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

    public async Task<bool> AtualizarStatusCanceladoAsync(int id)
    {
        var leilao = await _context.Leiloes.FirstOrDefaultAsync(l => l.Id == id);

        if (leilao == null)
            return false;

        leilao.Status = StatusLeilao.CANCELADO;
        leilao.DataAtualizacao = DateTime.UtcNow;

        _context.Entry(leilao).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return true;
    }

    //public Task<(bool Success, string? Message)> UpdateAsync(int id, LeilaoUpdateDto dto)
    //{
    //    throw new NotImplementedException();
    //}

}
