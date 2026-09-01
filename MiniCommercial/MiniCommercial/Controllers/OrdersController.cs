using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCommercial.Models.DTOs;
using MiniCommercial.Models.Entities;
using MiniCommercial.Services;

namespace MiniCommercial.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;
        public OrdersController(IOrderService service) => _service = service;

        [HttpGet] public async Task<IActionResult> Get() => Ok(await _service.GetAllOrdersAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var res = await _service.GetOrderByIdAsync(id);
            return res == null ? NotFound() : Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> Post(OrderCreateDto dto)
        {
            try { return Ok(await _service.CreateOrderAsync(dto)); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost("{id}/validate")]
        public async Task<IActionResult> Validate(int id)
        {
            try { await _service.ValidateOrderAsync(id); return Ok(new { message = "Validée" }); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) =>
            await _service.DeleteOrderAsync(id) ? NoContent() : NotFound();

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderCreateDto dto)
        {
            // On ne vérifie pas id == dto.Id car le DTO de création n'a souvent pas d'ID
            try
            {
                var success = await _service.UpdateOrderAsync(id, dto);
                if (!success) return NotFound(new { message = "Commande introuvable" });

                return NoContent(); // 204 Success
            }
            catch (Exception ex)
            {
                // Renvoie l'erreur (ex: "Commande déjà validée")
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
