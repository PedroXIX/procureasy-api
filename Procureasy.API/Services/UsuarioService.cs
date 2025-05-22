using Microsoft.EntityFrameworkCore;
using Procureasy.API.Data;
using Procureasy.API.Dtos;
using Procureasy.API.Helpers;
using Procureasy.API.Models;
using Procureasy.API.Models.Enums;
using Procureasy.API.Dtos.Usuario;

namespace Procureasy.API.Services;

public class UsuarioService
{
    private readonly ProcurEasyContext _context;

    public UsuarioService(ProcurEasyContext context)
    {
        _context = context;
    }

    public async Task<List<UsuarioDto>> GetAllAsync()
    {
        return await _context.Usuarios
            .Select(u => new UsuarioDto
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email,
                TipoUsuario = u.TipoUsuario,
                Cpf = u.Cpf,
                Cnpj = u.Cnpj,
                Ativo = u.Ativo,
                DataCriacao = u.DataCriacao
            }).ToListAsync();
    }

    public async Task<UsuarioDto?> GetByIdAsync(int id)
    {
        var u = await _context.Usuarios.FindAsync(id);
        if (u == null) return null;

        return new UsuarioDto
        {
            Id = u.Id,
            Nome = u.Nome,
            Email = u.Email,
            TipoUsuario = u.TipoUsuario,
            Cpf = u.Cpf,
            Cnpj = u.Cnpj,
            Ativo = u.Ativo,
            DataCriacao = u.DataCriacao
        };
    }

    public async Task<(bool Success, string? Message, Usuario? Created)> CreateAsync(UsuarioCreateDto dto)
    {
        if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
            return (false, "Este email já está em uso.", null);

        if (dto.TipoUsuario == TipoUsuario.CONSUMIDOR && string.IsNullOrWhiteSpace(dto.Cpf))
            return (false, "CPF é obrigatório para usuários do tipo Consumidor.", null);

        if (dto.TipoUsuario == TipoUsuario.FORNECEDOR && string.IsNullOrWhiteSpace(dto.Cnpj))
            return (false, "CNPJ é obrigatório para usuários do tipo Fornecedor.", null);

        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Senha = PasswordHelper.HashPassword(dto.Senha),
            Cpf = dto.Cpf ?? string.Empty,
            Cnpj = dto.Cnpj ?? string.Empty,
            TipoUsuario = dto.TipoUsuario,
            DataCriacao = DateTime.UtcNow,
            Ativo = true
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return (true, null, usuario);
    }

    public async Task<(bool Success, string? Message)> PatchAsync(int id, UsuarioPatchDto dto)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
            return (false, "Usuário não encontrado.");

        if (!string.IsNullOrWhiteSpace(dto.Email))
        {
            var emailExists = await _context.Usuarios.AnyAsync(u => u.Email == dto.Email && u.Id != id);
            if (emailExists)
                return (false, "Este email já está em uso.");
            usuario.Email = dto.Email;
        }

        if (!string.IsNullOrWhiteSpace(dto.Nome))
            usuario.Nome = dto.Nome;

        if (!string.IsNullOrWhiteSpace(dto.Senha))
            usuario.Senha = PasswordHelper.HashPassword(dto.Senha);

        if (dto.TipoUsuario.HasValue)
        {
            usuario.TipoUsuario = dto.TipoUsuario.Value;

            if (dto.TipoUsuario == TipoUsuario.CONSUMIDOR && string.IsNullOrWhiteSpace(dto.Cpf))
                return (false, "CPF é obrigatório para usuários do tipo Consumidor.");

            if (dto.TipoUsuario == TipoUsuario.FORNECEDOR && string.IsNullOrWhiteSpace(dto.Cnpj))
                return (false, "CNPJ é obrigatório para usuários do tipo Fornecedor.");
        }

        if (dto.Cpf != null)
            usuario.Cpf = dto.Cpf;

        if (dto.Cnpj != null)
            usuario.Cnpj = dto.Cnpj;

        _context.Entry(usuario).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return (true, null);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) return false;

        bool hasRelacionamentos = await _context.Leiloes.AnyAsync(l => l.UsuarioId == id) ||
                                  await _context.Lances.AnyAsync(l => l.UsuarioId == id);

        if (hasRelacionamentos)
        {
            usuario.Ativo = false;
            _context.Entry(usuario).State = EntityState.Modified;
        }
        else
        {
            _context.Usuarios.Remove(usuario);
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleStatusAsync(int id, bool ativo)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) return false;

        usuario.Ativo = ativo;
        _context.Entry(usuario).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return true;
    }
}
