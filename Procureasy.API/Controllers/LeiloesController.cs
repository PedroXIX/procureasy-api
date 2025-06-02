using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Procureasy.API.Dtos;
using Procureasy.API.Dtos.Leilao;
using Procureasy.API.Services.Interfaces;

namespace Procureasy.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class LeiloesController : ControllerBase
{
    private readonly ILeilaoService _service;

    public LeiloesController(ILeilaoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LeilaoDto>>> Get()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<LeilaoDto>> Get(int id)
    {
        var leilao = await _service.GetByIdAsync(id);
        return leilao == null ? NotFound() : Ok(leilao);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(LeilaoDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] LeilaoCreateDto dto)
    {
        var (success, message, leilao) = await _service.CreateAsync(dto);
        if (!success)
            return BadRequest(new { message });

        return CreatedAtAction(
            nameof(Get),
            new { id = leilao.Id },
            leilao);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] LeilaoUpdateDto dto)
    {
        var (success, message) = await _service.UpdateAsync(id, dto);
        if (!success)
            return BadRequest(new { message });

        return NoContent();
    }

    [HttpPatch("{leilaoId}/fornecedores")]
    public async Task<IActionResult> AddFornecedores(int leilaoId, [FromBody] LeilaoFornecedorCreateDto dto)
    {
        var (success, message) = await _service.AddFornecedoresAsync(leilaoId, dto.UsuariosIds);
        if (!success)
            return BadRequest(new { message });

        return NoContent();
    }

    [HttpGet("fornecedor/{fornecedorId}")]
    public async Task<ActionResult<List<LeilaoDto>>> GetLeiloesPorFornecedor(int fornecedorId)
    {
        var leiloes = await _service.GetLeiloesPorFornecedorAsync(fornecedorId);

        if (leiloes == null || !leiloes.Any())
            return NotFound(new { message = "Nenhum leil�o encontrado para o fornecedor especificado." });

        return Ok(leiloes);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
        => await _service.DeleteAsync(id) ? NoContent() : NotFound();
}
