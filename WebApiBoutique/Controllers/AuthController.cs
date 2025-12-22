using WebApiBoutique.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using WebApiBoutique.Services.Interface;
using WebApiBoutique.Repository.Interface;
using WebApiBoutique.Data;

namespace WebApiBoutique.Controllers
{
    // API controller for handling authentication operations
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Dependency injection for authentication service
        private readonly IAuthService _authService;

        // Constructor to initialize authentication service
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/Auth/register - Register a new user
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            try
            {
                // Validate incoming registration data
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value.Errors.Count > 0)
                        .Select(x => new { Field = x.Key, Errors = x.Value.Errors.Select(e => e.ErrorMessage) })
                        .ToList();
                    
                    var errorMessage = string.Join("; ", errors.SelectMany(e => e.Errors));
                    return BadRequest(new { 
                        message = $"Validation failed: {errorMessage}", 
                        success = false,
                        errors = errors
                    });
                }

                // Additional validation
                if (string.IsNullOrWhiteSpace(dto.Username))
                    return BadRequest(new { message = "Username is required", success = false });
                
                if (string.IsNullOrWhiteSpace(dto.Email))
                    return BadRequest(new { message = "Email is required", success = false });
                
                if (!IsValidEmail(dto.Email))
                    return BadRequest(new { message = "Please enter a valid email address", success = false });
                
                if (string.IsNullOrWhiteSpace(dto.Password))
                    return BadRequest(new { message = "Password is required", success = false });
                
                if (dto.Password.Length < 8)
                    return BadRequest(new { message = "Password must be at least 8 characters long", success = false });
                
                if (dto.Password != dto.ConfirmPassword)
                    return BadRequest(new { message = "Passwords do not match", success = false });
                


                // Get logger instance for tracking registration attempts
                var logger = HttpContext.RequestServices.GetRequiredService<ILogger<AuthController>>();
                logger.LogInformation("Registration attempt for email: {Email}", dto.Email);

                // Call service to register user with email verification
                var result = await _authService.RegisterAsync(dto, Request.Scheme, Request.Host.Value ?? "localhost");
                
                logger.LogInformation("Registration result: {Result}", result);
                
                // Check for specific error conditions
                if (result.Contains("User with this email already exists"))
                    return BadRequest(new { message = "An account with this email already exists. Please use a different email or try logging in.", success = false });
                
                if (result.Contains("Username already exists"))
                    return BadRequest(new { message = "This username is already taken. Please choose a different username.", success = false });
                
                if (result.Contains("Email service unavailable"))
                    return BadRequest(new { message = "Unable to send verification email. Please try again later or contact support.", success = false });

                // Return success response
                return Ok(new { message = result, success = true });
            }
            catch (ArgumentException ex)
            {
                var logger = HttpContext.RequestServices.GetRequiredService<ILogger<AuthController>>();
                logger.LogWarning(ex, "Registration validation failed for email: {Email}", dto?.Email);
                return BadRequest(new { message = ex.Message, success = false });
            }
            catch (InvalidOperationException ex)
            {
                var logger = HttpContext.RequestServices.GetRequiredService<ILogger<AuthController>>();
                logger.LogWarning(ex, "Registration business logic failed for email: {Email}", dto?.Email);
                return BadRequest(new { message = ex.Message, success = false });
            }
            catch (Exception ex)
            {
                var logger = HttpContext.RequestServices.GetRequiredService<ILogger<AuthController>>();
                logger.LogError(ex, "Registration failed for email: {Email}", dto?.Email);
                
                // Don't expose internal errors to users
                return BadRequest(new { 
                    message = "Registration failed due to a server error. Please try again later.", 
                    success = false 
                });
            }
        }
        
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        
        private async Task<bool> IsEmailDomainValid(string email)
        {
            try
            {
                var domain = email.Split('@').LastOrDefault();
                if (string.IsNullOrEmpty(domain))
                    return false;
                
                // Check if domain has MX records (mail server)
                var hostEntry = await System.Net.Dns.GetHostEntryAsync(domain);
                return hostEntry != null;
            }
            catch
            {
                // If DNS lookup fails, assume email might be valid (don't block registration)
                return true;
            }
        }

        // POST: api/Auth/verify-otp - Verify user email using OTP code
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpVerifyDTO dto)
        {
            // Verify the OTP code provided by user
            var error = await _authService.VerifyOtpAsync(dto);
            if (error != null)
                return BadRequest(error);

            // Return success if OTP is valid
            return Ok("Email verified successfully via OTP.");
        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            var result = await _authService.VerifyEmailAsync(token);
            if (result != null)
            {
                return Content($@"
                <!DOCTYPE html>
                <html>
                <head>
                    <title>Email Verification Failed</title>
                    <style>
                        body {{ font-family: Arial, sans-serif; text-align: center; padding: 50px; background: #f5f5f5; }}
                        .container {{ background: white; padding: 40px; border-radius: 10px; box-shadow: 0 4px 10px rgba(0,0,0,0.1); max-width: 500px; margin: 0 auto; }}
                        .error {{ color: #dc3545; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h2 class='error'>Verification Failed</h2>
                        <p>{result}</p>
                        <p>Please contact support if you continue to experience issues.</p>
                    </div>
                </body>
                </html>", "text/html");
            }

            return Content($@"
            <!DOCTYPE html>
            <html>
            <head>
                <title>Email Verified Successfully</title>
                <style>
                    body {{ font-family: Arial, sans-serif; text-align: center; padding: 50px; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); }}
                    .container {{ background: white; padding: 40px; border-radius: 10px; box-shadow: 0 4px 10px rgba(0,0,0,0.1); max-width: 500px; margin: 0 auto; }}
                    .success {{ color: #28a745; }}
                    .btn {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 12px 24px; text-decoration: none; border-radius: 25px; display: inline-block; margin-top: 20px; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <h2 class='success'>✓ Email Verified Successfully!</h2>
                    <p>Your email has been verified and your account is now active.</p>
                    <p>You can now log in to your account and start using our services.</p>
                    <a href='#' class='btn'>Continue to Login</a>
                </div>
            </body>
            </html>", "text/html");
        }

        [HttpGet("confirm")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var result = await _authService.ConfirmEmailAsync(email, token);
            if (result != null)
                return BadRequest(result);

            return Ok("Email confirmed successfully.");
        }

        [HttpPost("resend-verification")]
        public async Task<IActionResult> ResendVerification([FromBody] ConfirmEmailDTO dto)
        {
            var error = await _authService.ResendConfirmationAsync(dto.Email, Request.Scheme, Request.Host.Value ?? "localhost");
            if (error != null)
                return BadRequest(error);

            return Ok("Verification email resent successfully.");
        }

        [HttpPost("resend-confirmation")]
        public async Task<IActionResult> ResendConfirmation([FromBody] ConfirmEmailDTO dto)
        {
            var error = await _authService.ResendConfirmationAsync(dto.Email, Request.Scheme, Request.Host.Value);
            if (error != null)
                return BadRequest(error);

            return Ok("Confirmation email resent successfully.");
        }

        // POST: api/Auth/login - Authenticate user and return JWT token
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            // Attempt to login user with credentials
            var (error, result) = await _authService.LoginAsync(dto);
            if (error != null)
                return Unauthorized(error);

            // Return JWT token and user info on success
            return Ok(result);
        }

        // POST: api/Auth/forgot-password - Send password reset email
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto)
        {
            try
            {
                // Validate email format
                if (!ModelState.IsValid)
                    return BadRequest("Invalid email format.");

                var logger = HttpContext.RequestServices.GetRequiredService<ILogger<AuthController>>();
                logger.LogInformation("Password reset requested for email: {Email}", dto.Email);

                // Send password reset email with token
                await _authService.ForgotPasswordAsync(dto.Email, Request.Scheme, Request.Host.Value ?? "localhost");
                
                return Ok(new { 
                    message = "If the email is registered, you will receive a password reset link.",
                    success = true 
                });
            }
            catch (Exception ex)
            {
                var logger = HttpContext.RequestServices.GetRequiredService<ILogger<AuthController>>();
                logger.LogError(ex, "Error processing forgot password request for email: {Email}", dto?.Email);
                
                return Ok(new { 
                    message = "If the email is registered, you will receive a password reset link.",
                    success = true 
                });
            }
        }

        // POST: api/Auth/reset-password - Reset user password with token
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            try
            {
                // Validate reset password data
                if (!ModelState.IsValid)
                    return BadRequest("Invalid reset password data.");

                var logger = HttpContext.RequestServices.GetRequiredService<ILogger<AuthController>>();
                logger.LogInformation("Password reset attempt for email: {Email}", dto.Email);

                // Reset password using token
                var error = await _authService.ResetPasswordAsync(dto);
                if (error != null)
                {
                    logger.LogWarning("Password reset failed for email: {Email}. Error: {Error}", dto.Email, error);
                    return BadRequest(new { message = error, success = false });
                }

                logger.LogInformation("Password reset successful for email: {Email}", dto.Email);
                return Ok(new { message = "Password reset successful.", success = true });
            }
            catch (Exception ex)
            {
                var logger = HttpContext.RequestServices.GetRequiredService<ILogger<AuthController>>();
                logger.LogError(ex, "Error processing password reset for email: {Email}", dto?.Email);
                return BadRequest(new { message = $"Password reset failed: {ex.Message}", success = false });
            }
        }

        [HttpPost("test-email")]
        public async Task<IActionResult> TestEmail([FromBody] TestEmailDTO dto)
        {
            try
            {
                var emailService = HttpContext.RequestServices.GetRequiredService<IEmailService>();
                var logger = HttpContext.RequestServices.GetRequiredService<ILogger<AuthController>>();
                
                logger.LogInformation("Testing email send to: {Email}", dto.Email);
                
                await emailService.SendEmailAsync(
                    dto.Email,
                    "Test Email - WebAPI Boutique",
                    "<h1>Test Email</h1><p>If you receive this email, your email configuration is working correctly!</p>"
                );
                
                return Ok(new { message = "Test email sent successfully!", success = true });
            }
            catch (Exception ex)
            {
                var logger = HttpContext.RequestServices.GetRequiredService<ILogger<AuthController>>();
                logger.LogError(ex, "Test email failed for: {Email}", dto?.Email);
                return BadRequest(new { message = $"Test email failed: {ex.Message}", success = false });
            }
        }

        [HttpPost("fix-business-ids")]
        public async Task<IActionResult> FixBusinessIds()
        {
            try
            {
                var userRepo = HttpContext.RequestServices.GetRequiredService<IUserRepository>();
                var users = await userRepo.GetAllUsersAsync();
                var duplicateUsers = users.Where(u => u.BusinessId == 1 || 
                    users.Count(x => x.BusinessId == u.BusinessId) > 1).ToList();
                
                foreach (var user in duplicateUsers)
                {
                    user.BusinessId = Math.Abs(Guid.NewGuid().GetHashCode());
                    user.BusinessName = (user.UserName ?? "User") + "'s Boutique";
                    user.UpdatedAt = DateTime.UtcNow;
                    await userRepo.UpdateAsync(user);
                }
                
                return Ok(new { message = $"Fixed {duplicateUsers.Count} users with duplicate BusinessIds" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Auth/profile - Get authenticated user profile
        [HttpGet("profile")]
        [Microsoft.AspNetCore.Authorization.Authorize] // Requires valid JWT token
        public async Task<IActionResult> GetProfile()
        {
            // Extract email from JWT token
            var email = User.Identity?.Name;
            if (string.IsNullOrEmpty(email))
                return Unauthorized("Invalid token");

            // Fetch user profile from database
            var profile = await _authService.GetUserProfileAsync(email);
            if (profile == null)
                return NotFound("User not found");

            // Return user profile data
            return Ok(profile);
        }

        [HttpGet("email-config-test")]
        public IActionResult TestEmailConfig()
        {
            var config = HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            return Ok(new {
                SmtpHost = config["Email:SmtpHost"],
                SmtpPort = config["Email:SmtpPort"],
                SenderEmail = config["Email:SenderEmail"],
                HasPassword = !string.IsNullOrEmpty(config["Email:SenderPassword"])
            });
        }

        [HttpPost("test-password-reset")]
        public async Task<IActionResult> TestPasswordReset([FromBody] TestPasswordResetDTO dto)
        {
            try
            {
                var userRepo = HttpContext.RequestServices.GetRequiredService<IUserRepository>();
                var user = await userRepo.GetByEmailAsync(dto.Email);
                
                if (user == null)
                {
                    return Ok(new { success = false, message = "User not found" });
                }
                
                var token = await userRepo.GeneratePasswordResetTokenAsync(user);
                var resetResult = await userRepo.ResetPasswordAsync(user, token, dto.NewPassword);
                
                if (resetResult)
                {
                    var loginResult = await userRepo.VerifyPasswordAsync(user, dto.NewPassword);
                    
                    return Ok(new { 
                        success = true, 
                        passwordReset = true,
                        loginWorks = loginResult,
                        message = "Password reset and login test completed"
                    });
                }
                
                return Ok(new { success = false, message = "Password reset failed" });
            }
            catch (Exception ex)
            {
                return BadRequest($"Test failed: {ex.Message}");
            }
        }

        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPasswordPage(string email, string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
                {
                    return Content("<h2>Invalid reset link</h2><p>Please request a new password reset.</p>", "text/html");
                }

                var isValid = await _authService.ValidateResetTokenAsync(email, token);
                if (!isValid)
                {
                    return Content("<h2>Reset link expired</h2><p>This reset link has expired.</p>", "text/html");
                }

                var html = CreateResetPasswordForm(email, token);
                return Content(html, "text/html");
            }
            catch (Exception ex)
            {
                var logger = HttpContext.RequestServices.GetRequiredService<ILogger<AuthController>>();
                logger.LogError(ex, "Error validating reset token for email: {Email}", email);
                return Content("<h2>Error</h2><p>An error occurred. Please try again later.</p>", "text/html");
            }
        }

        private string CreateResetPasswordForm(string email, string token)
        {
            return $@"<!DOCTYPE html><html><head><title>Reset Password</title><style>body{{font-family:Arial;max-width:400px;margin:50px auto;padding:20px}}input{{width:100%;padding:10px;margin:10px 0;box-sizing:border-box}}button{{background:#007bff;color:white;padding:10px 20px;border:none;cursor:pointer;width:100%}}.error{{color:red}}.success{{color:green}}</style></head><body><h2>Reset Your Password</h2><form id='resetForm'><input type='hidden' id='email' value='{email}'/><input type='hidden' id='token' value='{token}'/><label>New Password:</label><input type='password' id='newPassword' required minlength='8'/><label>Confirm Password:</label><input type='password' id='confirmPassword' required minlength='8'/><button type='submit'>Reset Password</button><div id='message'></div></form><script>document.getElementById('resetForm').addEventListener('submit',async function(e){{e.preventDefault();const messageDiv=document.getElementById('message');const newPassword=document.getElementById('newPassword').value;const confirmPassword=document.getElementById('confirmPassword').value;if(newPassword!==confirmPassword){{messageDiv.innerHTML='<div class=""error"">Passwords do not match</div>';return;}}try{{const response=await fetch('/api/Auth/reset-password',{{method:'POST',headers:{{'Content-Type':'application/json'}},body:JSON.stringify({{email:document.getElementById('email').value,token:document.getElementById('token').value,newPassword:newPassword}})}});const result=await response.json();if(response.ok){{messageDiv.innerHTML='<div class=""success"">Password reset successful!</div>';document.getElementById('resetForm').style.display='none';}}else{{messageDiv.innerHTML='<div class=""error"">'+(result.message||'Password reset failed')+'</div>';}}}}catch(error){{messageDiv.innerHTML='<div class=""error"">An error occurred. Please try again.</div>';}}}});</script></body></html>";
        }

        [HttpGet("debug-reset-token")]
        public async Task<IActionResult> DebugResetToken(string email, string token)
        {
            try
            {
                var userRepo = HttpContext.RequestServices.GetRequiredService<IUserRepository>();
                var user = await userRepo.GetByEmailAsync(email);
                if (user == null)
                {
                    return Ok(new { userExists = false });
                }
                
                var isValid = await _authService.ValidateResetTokenAsync(email, token);
                return Ok(new {
                    userExists = true,
                    tokenInDb = user.PasswordResetToken,
                    tokenProvided = token,
                    tokensMatch = user.PasswordResetToken == token,
                    expiryTime = user.PasswordResetTokenExpiry,
                    currentTime = DateTime.UtcNow,
                    isValid = isValid
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Debug failed: {ex.Message}");
            }
        }
    }

    public class TestEmailDTO
    {
        public string Email { get; set; } = string.Empty;
    }
    
    public class TestPasswordResetDTO
    {
        public string Email { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}