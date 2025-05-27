using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Procureasy.API.Dtos.Lance;
using Procureasy.API.Services.Interfaces;

namespace Procureasy.API.Controllers;


[Route("api/[controller]")]
[ApiController]
public class LanceController : ControllerBase
{
    private readonly ILanceService _service;

    public LanceController(ILanceService service)
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

    [HttpDelete("{id}")]
    public IActionResult Delete()
        => StatusCode(405, new { message = "Lances não podem ser removidos." });
}