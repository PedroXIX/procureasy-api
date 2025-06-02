using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Procureasy.API.Dtos.Lance;
using Procureasy.API.Services.Interfaces;

namespace Procureasy.API.Controllers;

//[Authorize(Roles = "ADMINISTRADOR,CONSUMIDOR")]
[Route("api/[controller]")]
[ApiController]
public class LancesController : ControllerBase
{
    private readonly ILanceService _service;

    public LancesController(ILanceService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LanceDto>>> Get()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<LanceDto>> Get(int id)
    {
        var lance = await _service.GetByIdAsync(id);
        return lance == null ? NotFound() : Ok(lance);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] LanceCreateDto dto)
    {
        var (success, message) = await _service.CreateAsync(dto);
        if (!success)
            return BadRequest(new { message });

        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult Put()
        => StatusCode(405, new { message = "Lances não podem ser atualizados." });

    [HttpPatch("{id}/vencedor")]
    public async Task<IActionResult> AtualizarStatusVencedor(int id)
    {
        var success = await _service.UpdateStatusAsync(id, true);
        return success ? NoContent() : NotFound();
    }


    [HttpDelete("{id}")]
    public IActionResult Delete()
        => StatusCode(405, new { message = "Lances não podem ser removidos." });
}