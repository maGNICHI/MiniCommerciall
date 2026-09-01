using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCommercial.Models.Entities;
using MiniCommercial.Services;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;
    public ProductsController(IProductService service) => _service = service;

    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var p = await _service.GetByIdAsync(id);
        return p == null ? NotFound() : Ok(p);
    }

    [HttpPost] public async Task<IActionResult> Create(Product p) => Ok(await _service.CreateAsync(p));
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) =>
        await _service.DeleteAsync(id) ? NoContent() : NotFound();

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Product product)
    {
        // 1. Vérification de sécurité pour éviter l'erreur 400
        if (id != product.Id)
        {
            return BadRequest("L'ID dans l'URL ne correspond pas à l'ID du produit.");
        }

        // 2. Appel au service pour la mise à jour
        var success = await _service.UpdateAsync(id, product);

        if (!success)
        {
            return NotFound("Produit introuvable.");
        }

        return NoContent(); // 204 Success
    }
}