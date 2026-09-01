using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCommercial.Models.Entities;
using MiniCommercial.Services;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly IClientService _service;
    public ClientsController(IClientService service) => _service = service;

    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var c = await _service.GetByIdAsync(id);
        return c == null ? NotFound() : Ok(c);
    }

    [HttpPost] public async Task<IActionResult> Create(Client c) => Ok(await _service.CreateAsync(c));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Client c) =>
        await _service.UpdateAsync(id, c) ? NoContent() : BadRequest();

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) =>
        await _service.DeleteAsync(id) ? NoContent() : NotFound();
}