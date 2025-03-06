using Brewery_DB_Service.Data;
using Brewery_DB_Service.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Brewery_DB_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContentController(AppDbContext context) => _context = context;

        [HttpPost]
        public async Task<IActionResult> CreateContent([FromBody] Content content)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _context.Content.Add(content);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetContentById), new { id = content.Id }, content);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContentById(int id)
        {
            var content = await _context.Content.FindAsync(id);
            if (content == null) return NotFound();
            return Ok(content);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContent()
        {
            return Ok(await _context.Content.ToListAsync());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContent(int id, [FromBody] Content content)
        {
            if (id != content.Id) return BadRequest();
            var existing = await _context.Content.FindAsync(id);
            if (existing == null) return NotFound();
            existing.Title = content.Title;
            existing.Type = content.Type;
            existing.Body = content.Body;
            existing.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContent(int id)
        {
            var content = await _context.Content.FindAsync(id);
            if (content == null) return NotFound();
            _context.Content.Remove(content);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetContentByType(string type)
        {
            var content = await _context.Content.Where(c => c.Type == type).ToListAsync();
            return Ok(content);
        }
    }
}