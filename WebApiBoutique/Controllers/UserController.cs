using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiBoutique.Models;
using WebApiBoutique.Services;
using WebApiBoutique.Data;

// API controller for basic user management operations
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    // Direct database context injection for user operations
    private readonly AppDbContext _context;

    // Constructor to initialize database context
    public UserController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/User - Retrieve all users from database
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        => await _context.Users.ToListAsync();  // Fetch all user records

    // GET: api/User/{id} - Get specific user by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        // Find user by primary key
        var user = await _context.Users.FindAsync(id);
        return user == null ? NotFound() : user;
    }

    // POST: api/User - Create a new user
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(User user)
    {
        // Add new user to database
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
    }

  
    // PUT: api/User/{id} - Update existing user
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, User user)
    {
        // Validate that URL ID matches user ID
        if (id != user.UserId)
            return BadRequest("User ID mismatch.");

        // Find existing user in database
        var existingUser = await _context.Users.FindAsync(id);
        if (existingUser == null)
            return NotFound($"User with ID {id} not found.");

        // Update user fields manually to avoid overwriting unchanged data
        existingUser.Role = user.Role;
        existingUser.Email = user.Email;
        existingUser.PhoneNumber = user.PhoneNumber;
        existingUser.Name = user.Name;
        existingUser.Active = user.Active;

        try
        {
            // Save changes to database
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            // Handle concurrent modification conflicts
            return StatusCode(500, "Concurrency error: record may have been modified or deleted.");
        }
    }


    // DELETE: api/User/{id} - Delete user from database
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        // Find user to delete
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();
        
        // Remove user from database (hard delete)
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
