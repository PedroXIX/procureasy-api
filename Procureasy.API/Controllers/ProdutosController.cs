using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Procureasy.API.Dtos.Produto;
using Procureasy.API.Services.Interfaces;

namespace Procureasy.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoService _service;

    public ProdutosController(IProdutoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProdutoDto>>> Get()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<ProdutoDto>> Get(int id)
    {
        var produto = await _service.GetByIdAsync(id);
        return produto == null ? NotFound() : Ok(produto);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ProdutoCreateDto dto)
    {
        var (success, message) = await _service.CreateAsync(dto);
        if (!success)
            return BadRequest(new { message });

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] ProdutoUpdateDto dto)
    {
        var (success, message) = await _service.UpdateAsync(id, dto);
        if (!success)
            return BadRequest(new { message });

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
        => await _service.DeleteAsync(id) ? NoContent() : NotFound();
}
