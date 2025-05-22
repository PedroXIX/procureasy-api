using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Procureasy.API.Dtos.Usuario;
using Procureasy.API.Models;
using Procureasy.API.Services;

namespace Procureasy.API.Controllers;

[Authorize(Roles = "ADMINISTRADOR")]
[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly UsuarioService _service;

    public UsuarioController(UsuarioService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsuarios()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<UsuarioDto>> GetUsuario(int id)
    {
        var usuario = await _service.GetByIdAsync(id);
        if (usuario == null) return NotFound();
        return Ok(usuario);
    }

    [HttpPost]
    public async Task<IActionResult> PostUsuario([FromBody] UsuarioCreateDto dto)
    {
        var (success, message, created) = await _service.CreateAsync(dto);
        if (!success)
            return Conflict(new { message });

        return CreatedAtAction(nameof(GetUsuario), new { id = created!.Id }, created);
    }


    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchUsuario(int id, [FromBody] UsuarioPatchDto dto)
    {
        var (success, message) = await _service.PatchAsync(id, dto);
        if (!success)
            return BadRequest(new { message });

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUsuario(int id)
        => await _service.DeleteAsync(id) ? NoContent() : NotFound();

    [HttpPatch("{id}/ativar")]
    public async Task<IActionResult> AtivarUsuario(int id)
        => await _service.ToggleStatusAsync(id, true) ? NoContent() : NotFound();

    [HttpPatch("{id}/desativar")]
    public async Task<IActionResult> DesativarUsuario(int id)
        => await _service.ToggleStatusAsync(id, false) ? NoContent() : NotFound();
}
