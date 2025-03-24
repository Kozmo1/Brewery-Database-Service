using Brewery_DB_Service.Data;
using Brewery_DB_Service.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Brewery_DB_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context) => _context = context;

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] CartRequest cartRequest)
        {
            Console.WriteLine($"Received cartRequest: UserId={cartRequest.UserId}, InventoryId={cartRequest.InventoryId}, Quantity={cartRequest.Quantity}");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var cartItem = new Cart
            {
                UserId = cartRequest.UserId,
                InventoryId = cartRequest.InventoryId,
                Quantity = cartRequest.Quantity
            };
            var existing = await _context.Cart
                .FirstOrDefaultAsync(c => c.UserId == cartItem.UserId && c.InventoryId == cartItem.InventoryId);
            if (existing != null)
            {
                existing.Quantity += cartItem.Quantity;
            }
            else
            {
                _context.Cart.Add(cartItem);
            }
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCart), new { userId = cartItem.UserId }, cartItem);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            var cart = await _context.Cart.Where(c => c.UserId == userId).ToListAsync();
            return Ok(cart);
        }

        [HttpGet("item/{id}")]
        public async Task<IActionResult> GetCartItem(int id)
        {
            var cartItem = await _context.Cart.FindAsync(id);
            if (cartItem == null) return NotFound();
            return Ok(cartItem);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCart(int id, [FromBody] CartUpdate update)
        {
            var cartItem = await _context.Cart.FindAsync(id);
            if (cartItem == null) return NotFound();
            cartItem.Quantity = update.Quantity;
            await _context.SaveChangesAsync();
            return Ok(cartItem);
        }

        [HttpDelete("remove/{id}")]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var cartItem = await _context.Cart.FindAsync(id);
            if (cartItem == null) return NotFound();
            _context.Cart.Remove(cartItem);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("clear/{userId}")]
        public async Task<IActionResult> ClearCart(int userId)
        {
            var cartItems = await _context.Cart.Where(c => c.UserId == userId).ToListAsync();
            _context.Cart.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }

    public class CartUpdate
    {
        public int Quantity { get; set; }
    }
}