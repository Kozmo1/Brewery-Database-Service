using Brewery_DB_Service.Data;
using Brewery_DB_Service.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Brewery_DB_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ShippingController(AppDbContext context) => _context = context;

        [HttpPost]
        public async Task<IActionResult> CreateShipment([FromBody] Shipping shipment)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _context.Shipping.Add(shipment);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetShipment), new { id = shipment.Id }, shipment);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShipment(int id)
        {
            var shipment = await _context.Shipping.FindAsync(id);
            if (shipment == null) return NotFound();
            return Ok(shipment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShipment(int id, [FromBody] Shipping shipment)
        {
            if (id != shipment.Id) return BadRequest();
            var existing = await _context.Shipping.FindAsync(id);
            if (existing == null) return NotFound();
            existing.Status = shipment.Status;
            existing.TrackingNumber = shipment.TrackingNumber;
            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetShipmentByOrder(int orderId)
        {
            var shipment = await _context.Shipping.FirstOrDefaultAsync(s => s.OrderId == orderId);
            if (shipment == null) return NotFound();
            return Ok(shipment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShipment(int id)
        {
            var shipment = await _context.Shipping.FindAsync(id);
            if (shipment == null) return NotFound();
            _context.Shipping.Remove(shipment);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}