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
    public class PaymentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PaymentController(AppDbContext context) => _context = context;

        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] Payment payment)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            payment.Status = "Completed"; // Simplified for demo
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPaymentById), new { id = payment.Id }, payment);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return NotFound();
            return Ok(payment);
        }

        [HttpPost("refund/{paymentId}")]
        public async Task<IActionResult> RefundPayment(int paymentId, [FromBody] RefundRequest request)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null) return NotFound();
            payment.Status = "Refunded";
            await _context.SaveChangesAsync();
            return Ok(payment);
        }
    }

    public class RefundRequest
    {
        public decimal Amount { get; set; }
    }
}