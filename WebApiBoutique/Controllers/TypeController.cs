using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiBoutique.Data;
using WebApiBoutique.Models;

namespace WebApiBoutique.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TypeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TypeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Type
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeModel>>> GetTypes()
        {
            return await _context.Types.ToListAsync();
        }

        // GET: api/Type/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TypeModel>> GetType(int id)
        {
            var type = await _context.Types.FindAsync(id);
            if (type == null)
                return NotFound();
            return type;
        }

        // POST: api/Type
        [HttpPost]
        public async Task<ActionResult<TypeModel>> CreateType(TypeModel type)
        {
            _context.Types.Add(type);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetType), new { id = type.TypeId }, type);
        }

        // PUT: api/Type/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateType(int id, TypeModel type)
        {
            if (id != type.TypeId)
                return BadRequest();

            _context.Entry(type).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Type/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteType(int id)
        {
            var type = await _context.Types.FindAsync(id);
            if (type == null)
                return NotFound();

            _context.Types.Remove(type);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}