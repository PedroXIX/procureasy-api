using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Procureasy.API.Data;
using Procureasy.API.Models.Enums;
using Procureasy.API.Models;

namespace Procureasy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly ProcurEasyContext _context;

        public UsuarioController(ProcurEasyContext context)
        {
            _context = context;
        }

        // GET: api/Usuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // GET: api/Usuario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound();

            return usuario;
        }

        // PUT: api/Usuario/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuarioAtualizado)
        {
            if (id != usuarioAtualizado.Id)
                return BadRequest(new { message = "ID da URL difere do ID do usuário." });

            var existingUser = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == usuarioAtualizado.Email && u.Id != id);

            if (existingUser != null)
                return Conflict(new { message = "Este email já está em uso." });

            // Validação de CPF ou CNPJ conforme o TipoUsuario
            if (usuarioAtualizado.TipoUsuario == TipoUsuario.CONSUMIDOR && string.IsNullOrEmpty(usuarioAtualizado.Cpf))
                return BadRequest(new { message = "CPF é obrigatório para usuários do tipo Consumidor." });

            if (usuarioAtualizado.TipoUsuario == TipoUsuario.FORNECEDOR && string.IsNullOrEmpty(usuarioAtualizado.Cnpj))
                return BadRequest(new { message = "CNPJ é obrigatório para usuários do tipo Fornecedor." });

            _context.Entry(usuarioAtualizado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Usuario
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario([FromBody]Usuario usuario)
        {
            var existingUser = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == usuario.Email);

            if (existingUser != null)
                return Conflict(new { message = "Este email já está em uso." });

            // Validação de CPF ou CNPJ conforme o TipoUsuario
            if (usuario.TipoUsuario == TipoUsuario.CONSUMIDOR && string.IsNullOrEmpty(usuario.Cpf))
                return BadRequest(new { message = "CPF é obrigatório para usuários do tipo Consumidor." });

            if (usuario.TipoUsuario == TipoUsuario.FORNECEDOR && string.IsNullOrEmpty(usuario.Cnpj))
                return BadRequest(new { message = "CNPJ é obrigatório para usuários do tipo Fornecedor." });

            usuario.DataCriacao = DateTime.UtcNow;
            usuario.Ativo = true;

            _context.Usuarios.Add(usuario);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { message = "Erro ao salvar o usuário: " + ex.InnerException?.Message });
            }

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        // DELETE: api/Usuario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            var hasLeiloes = await _context.Leiloes.AnyAsync(l => l.UsuarioId == id);
            var hasLances = await _context.Lances.AnyAsync(l => l.UsuarioId == id);

            if (hasLeiloes || hasLances)
            {
                usuario.Ativo = false;
                _context.Entry(usuario).State = EntityState.Modified;
            }
            else
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PATCH: api/Usuario/5/ativar
        [HttpPatch("{id}/ativar")]
        public async Task<IActionResult> AtivarUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            usuario.Ativo = true;
            _context.Entry(usuario).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PATCH: api/Usuario/5/desativar
        [HttpPatch("{id}/desativar")]
        public async Task<IActionResult> DesativarUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            usuario.Ativo = false;
            _context.Entry(usuario).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}