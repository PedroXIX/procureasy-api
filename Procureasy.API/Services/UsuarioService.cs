using Microsoft.EntityFrameworkCore;
using Procureasy.API.Data;
using Procureasy.API.Dtos.Usuario;
using Procureasy.API.Helpers;
using Procureasy.API.Models;
using Procureasy.API.Models.Enums;
using Procureasy.API.Services.Interfaces;

namespace Procureasy.API.Services;

public class UsuarioService : IUsuarioService
{
    private readonly ProcurEasyContext _context;
    private readonly EmailValidator _emailValidator;
    private readonly PasswordValidator _passwordValidator;
    private readonly DocumentNormalizer _documentNormalizer;

    public UsuarioService(
        ProcurEasyContext context,
        EmailValidator emailValidator,
        PasswordValidator passwordValidator,
        DocumentNormalizer documentNormalizer)
    {
        _context = context;
        _emailValidator = emailValidator;
        _passwordValidator = passwordValidator;
        _documentNormalizer = documentNormalizer;
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
        if (!_emailValidator.IsValid(dto.Email))
            return (false, "Email inválido.", null);

        if (!_passwordValidator.IsStrong(dto.Senha))
            return (false, "Senha fraca. Use no mínimo 8 caracteres, com letra maiúscula, minúscula, número e símbolo.", null);

        if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
            return (false, "Este email já está em uso.", null);

        var cpf = _documentNormalizer.Normalize(dto.Cpf);
        var cnpj = _documentNormalizer.Normalize(dto.Cnpj);

        if (dto.TipoUsuario == TipoUsuario.CONSUMIDOR && string.IsNullOrWhiteSpace(cpf))
            return (false, "CPF é obrigatório para consumidores.", null);

        if (dto.TipoUsuario == TipoUsuario.FORNECEDOR && string.IsNullOrWhiteSpace(cnpj))
            return (false, "CNPJ é obrigatório para fornecedores.", null);

        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Senha = PasswordHelper.HashPassword(dto.Senha),
            Cpf = cpf,
            Cnpj = cnpj,
            TipoUsuario = dto.TipoUsuario,
            DataCriacao = DateTime.UtcNow,
            Ativo = true
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return (true, null, usuario);
    }

    public async Task<(bool Success, string? Message)> UpdateAsync(int id, UsuarioUpdateDto dto)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
            return (false, "Usuário não encontrado.");

        if (!_emailValidator.IsValid(dto.Email))
            return (false, "Email inválido.");

        if (!_passwordValidator.IsStrong(dto.Senha))
            return (false, "Senha fraca. Use no mínimo 8 caracteres, com letra maiúscula, minúscula, número e símbolo.");

        var emailExists = await _context.Usuarios.AnyAsync(u => u.Email == dto.Email && u.Id != id);
        if (emailExists)
            return (false, "Este email já está em uso.");

        var cpf = _documentNormalizer.Normalize(dto.Cpf);
        var cnpj = _documentNormalizer.Normalize(dto.Cnpj);

        if (dto.TipoUsuario == TipoUsuario.CONSUMIDOR && string.IsNullOrWhiteSpace(cpf))
            return (false, "CPF é obrigatório para consumidores.");

        if (dto.TipoUsuario == TipoUsuario.FORNECEDOR && string.IsNullOrWhiteSpace(cnpj))
            return (false, "CNPJ é obrigatório para fornecedores.");

        usuario.Nome = dto.Nome;
        usuario.Email = dto.Email;
        usuario.Senha = PasswordHelper.HashPassword(dto.Senha);
        usuario.Cpf = cpf;
        usuario.Cnpj = cnpj;
        usuario.TipoUsuario = dto.TipoUsuario;

        _context.Entry(usuario).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return (true, null);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) return false;

        var hasRelacionamentos = await _context.Leiloes.AnyAsync(l => l.UsuarioId == id) ||
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