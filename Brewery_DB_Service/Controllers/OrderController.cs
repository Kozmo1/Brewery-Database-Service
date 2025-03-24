using Brewery_DB_Service.Data;
using Brewery_DB_Service.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Brewery_DB_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context) => _context = context;

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest request)
        {
            Console.WriteLine("Received order request: " + System.Text.Json.JsonSerializer.Serialize(request));
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState errors: " + string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }

            var order = new Order
            {
                UserId = request.UserId,
                TotalPrice = request.Items.Sum(i => i.PriceAtOrder * i.Quantity),
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
                Items = new List<OrderItem>()
            };

            _context.Orders.Add(order);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("SaveChanges error (order): " + ex.ToString());
                throw;
            }

            order.Items = request.Items.Select(i => new OrderItem
            {
                OrderId = order.Id,
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                ProductName = i.ProductName,
                PriceAtOrder = i.PriceAtOrder
            }).ToList();

            _context.OrderItems.AddRange(order.Items);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("SaveChanges error (order items): " + ex.ToString());
                throw;
            }

            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();

            var response = new OrderResponse
            {
                Id = order.Id,
                UserId = order.UserId,
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                Items = order.Items.Select(i => new OrderItemResponse
                {
                    Id = i.Id,
                    OrderId = i.OrderId,
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    PriceAtOrder = i.PriceAtOrder
                }).ToList()
            };

            return Ok(response);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUser(int userId)
        {
            var orders = await _context.Orders.Include(o => o.Items).Where(o => o.UserId == userId).ToListAsync();
            var response = orders.Select(order => new OrderResponse
            {
                Id = order.Id,
                UserId = order.UserId,
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                Items = order.Items.Select(i => new OrderItemResponse
                {
                    Id = i.Id,
                    OrderId = i.OrderId,
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    PriceAtOrder = i.PriceAtOrder
                }).ToList()
            }).ToList();

            return Ok(response);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatusUpdate update)
        {
            Console.WriteLine($"Received PUT /api/order/{id}/status with body: {System.Text.Json.JsonSerializer.Serialize(update)}");
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                Console.WriteLine($"Order with ID {id} not found");
                return NotFound();
            }
            order.Status = update.Status;
            await _context.SaveChangesAsync();
            Console.WriteLine($"Updated order with ID {id} to status: {order.Status}");
            return Ok(order);
        }

    }

    public class OrderStatusUpdate
    {
        public string Status { get; set; }
    }


}