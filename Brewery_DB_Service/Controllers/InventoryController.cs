using Brewery_DB_Service.Data;
using Brewery_DB_Service.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Brewery_DB_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InventoryController(AppDbContext context) => _context = context;

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Inventory product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _context.Inventory.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _context.Inventory.FindAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Inventory product)
        {
            if (id != product.Id) return BadRequest();
            var existing = await _context.Inventory.FindAsync(id);
            if (existing == null) return NotFound();
            _context.Entry(existing).CurrentValues.SetValues(product);
            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Inventory.FindAsync(id);
            if (product == null) return NotFound();
            _context.Inventory.Remove(product);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await _context.Inventory.ToListAsync());
        }

        [HttpPut("{id}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] StockUpdate update)
        {
            var product = await _context.Inventory.FindAsync(id);
            if (product == null) return NotFound();
            product.StockQuantity += update.Quantity;
            if (product.StockQuantity < 0) return BadRequest("Stock cannot be negative");
            await _context.SaveChangesAsync();
            return Ok(product);
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStock()
        {
            var lowStock = await _context.Inventory.Where(i => i.StockQuantity <= i.ReorderPoint).ToListAsync();
            return Ok(lowStock);
        }
    }

    public class StockUpdate
    {
        public int Quantity { get; set; }
    }
}