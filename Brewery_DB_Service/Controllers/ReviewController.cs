using Brewery_DB_Service.Data;
using Brewery_DB_Service.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Brewery_DB_Service.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReviewsController(AppDbContext context) => _context = context;

        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] Review review)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetReviewById), new { id = review.Id }, review);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewById(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return NotFound();
            return Ok(review);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] Review review)
        {
            if (id != review.Id) return BadRequest();
            var existing = await _context.Reviews.FindAsync(id);
            if (existing == null) return NotFound();
            _context.Entry(existing).CurrentValues.SetValues(review);
            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return NotFound();
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            return Ok(await _context.Reviews.ToListAsync());
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetReviewsByProduct(int productId)
        {
            var reviews = await _context.Reviews.Where(r => r.ProductId == productId).ToListAsync();
            return Ok(reviews);
        }
    }
}