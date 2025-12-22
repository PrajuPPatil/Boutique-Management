using Microsoft.AspNetCore.Mvc;
using WebApiBoutique.Services;
using WebApiBoutique.Models.DTOs;
using WebApiBoutique.Data;

namespace WebApiBoutique.Controllers
{
    // API controller for managing user password history (security feature)
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordHistoryController : ControllerBase
    {
        // User service for password history operations
        private readonly IUserService _service;

        // Constructor with dependency injection for user service
        public PasswordHistoryController(IUserService service)
        {
            _service = service;
        }

        // Get password history for user (alternative route for compatibility)
        [HttpGet("api/password-history/{userId}")]
        // [HttpGet("{userId}")] // Commented alternative route
        public async Task<IActionResult> GetPasswordHistory(int userId)
        {
            // Retrieve password history for security auditing
            var result = await _service.GetPasswordHistoryByUserIdAsync(userId);
            return result.Any() ? Ok(result) : NotFound();
        }

        // Get password history for user (primary route)
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetHistory(int userId)
        {
            // Get complete password change history for user
            var history = await _service.GetPasswordHistoryByUserIdAsync(userId);
            return Ok(history);
        }
        
    }
}
