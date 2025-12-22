using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using WebApiBoutique.Models;
using WebApiBoutique.Models.DTOs;
using WebApiBoutique.Repository.Interface;
using WebApiBoutique.Services.Interface;

namespace WebApiBoutique.Services
{
    // Service class for handling authentication operations
    public class AuthService : IAuthService
    {
        // Dependency injection for required services
        private readonly IUserRepository _userRepository;  // User data operations
        private readonly IJwtTokenService _jwtTokenService;  // JWT token generation
        private readonly IEmailService _emailService;  // Email sending functionality
        private readonly ILogger<AuthService> _logger;  // Logging service

        // Constructor to initialize all dependencies
        public AuthService(IUserRepository userRepository, IJwtTokenService jwtTokenService, IEmailService emailService, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            _emailService = emailService;
            _logger = logger;
        }

        // Register a new user with email verification
        public async Task<string> RegisterAsync(RegisterDTO dto, string scheme, string host)
        {
            try
            {
                _logger.LogInformation("Starting user registration for email: {Email}", dto?.Email);
                
                // Validate input data
                if (dto == null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                {
                    _logger.LogWarning("Invalid registration data provided");
                    return "Invalid registration data";
                }

                // Validate email format using regex
                if (!IsValidEmail(dto.Email))
                {
                    _logger.LogWarning("Invalid email format: {Email}", dto.Email);
                    return "Invalid email format";
                }

                // Check password strength requirements
                if (!IsStrongPassword(dto.Password))
                {
                    _logger.LogWarning("Weak password provided for email: {Email}", dto.Email);
                    return "Password does not meet security requirements";
                }

                // Check if email already exists in database
                if (await _userRepository.EmailExistsAsync(dto.Email))
                {
                    _logger.LogWarning("Registration attempt with existing email: {Email}", dto.Email);
                    return "User with this email already exists";
                }

                // Determine verification method (OTP or email link)
                var verificationMethod = dto.VerificationMethod?.ToLower()?.Trim() ?? "otp";
                if (verificationMethod != "otp" && verificationMethod != "link")
                {
                    _logger.LogWarning("Invalid verification method: {Method}", dto.VerificationMethod);
                    verificationMethod = "otp"; // Default to OTP for security
                }

                // Create new user object with unique business
                var businessId = Math.Abs(Guid.NewGuid().GetHashCode());
                var businessName = (dto.Username?.Trim() ?? "User") + "'s Boutique";
                
                var user = new ApplicationUser
                {
                    UserName = dto.Username?.Trim() ?? string.Empty,
                    Email = dto.Email.Trim(),
                    Role = "User",  // Default role for new users
                    IsEmailConfirmed = false,  // Requires email verification
                    CreatedAt = DateTime.UtcNow,
                    BusinessId = businessId,
                    BusinessName = businessName
                };

                _logger.LogInformation("Creating user in database for email: {Email}", dto.Email);
                var createdUser = await _userRepository.CreateAsync(user, dto.Password);
                _logger.LogInformation("User created successfully with ID: {UserId}", createdUser.Id);

                if (verificationMethod == "otp")
                {
                    // OTP Verification Method - Generate 6-digit code
                    var otp = GenerateSecureOtp();
                    user.OtpCode = otp;
                    user.OtpGeneratedAt = DateTime.UtcNow;  // Track when OTP was generated
                    await _userRepository.UpdateAsync(user);
                    
                    _logger.LogInformation("Generated OTP for email: {Email}. OTP: {OTP}", dto.Email, otp);
                    Console.WriteLine($"DEBUG - OTP for {dto.Email}: {otp}"); // For testing purposes only

                    try
                    {
                        _logger.LogInformation("Attempting to send OTP email to: {Email}", dto.Email);
                        await _emailService.SendEmailAsync(
                            user.Email,
                            "Your Verification Code - WebAPI Boutique",
                            CreateOtpEmail(user.UserName ?? string.Empty, otp)
                        );
                        _logger.LogInformation("OTP email sent successfully to: {Email}", dto.Email);
                        return "Registration successful! Please check your email for the 6-digit verification code and enter it on our website.";
                    }
                    catch (Exception emailEx)
                    {
                        _logger.LogError(emailEx, "Failed to send OTP email to: {Email}. Error: {Error}. StackTrace: {StackTrace}", 
                            dto.Email, emailEx.Message, emailEx.StackTrace);
                        
                        // Still return success but with different message
                        return $"Registration completed but failed to send OTP email: {emailEx.Message}. Your OTP is: {otp} (Use this for testing)";
                    }
                }
                else
                {
                    // Email Link Verification Method
                    var verificationToken = GenerateSecureToken();
                    user.ConfirmationToken = verificationToken;
                    await _userRepository.UpdateAsync(user);
                    
                    _logger.LogDebug("Generated verification token for: {Email}", dto.Email);

                    string verificationLink = $"{scheme}://{host}/api/Auth/verify-email?token={Uri.EscapeDataString(verificationToken)}";

                    try
                    {
                        await _emailService.SendEmailAsync(
                            user.Email,
                            "Verify Your Email Address - WebAPI Boutique",
                            CreateProfessionalVerificationEmail(user.UserName ?? string.Empty, verificationLink)
                        );
                        
                        _logger.LogInformation("Verification email sent successfully to: {Email}", dto.Email);
                        return "Registration successful! Please check your email and click the verification link to activate your account.";
                    }
                    catch (Exception emailEx)
                    {
                        _logger.LogError(emailEx, "Failed to send verification email to: {Email}. Error: {Error}", dto.Email, emailEx.Message);
                        return "Registration completed but failed to send verification email. Please contact support.";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during registration for email: {Email}. Error: {Error}", dto?.Email, ex.Message);
                return $"An error occurred during registration: {ex.Message}";
            }
        }



        public async Task<string?> VerifyEmailAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return "Invalid verification token";

            var user = await _userRepository.GetByConfirmationTokenAsync(token);
            if (user == null) 
                return "Invalid or expired verification link";

            if (user.IsEmailConfirmed)
                return "Email already verified";

            user.IsEmailConfirmed = true;
            user.ConfirmationToken = null;
            await _userRepository.UpdateAsync(user);
            
            _logger.LogInformation("Email verified successfully for user: {Email}", user.Email);
            return null;
        }

        public async Task<string?> ConfirmEmailAsync(string email, string token)
        {
            if (!IsValidEmail(email) || string.IsNullOrWhiteSpace(token))
                return "Invalid confirmation data";

            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return "User not found";

            if (user.ConfirmationToken != token)
                return "Invalid confirmation token";

            user.IsEmailConfirmed = true;
            user.ConfirmationToken = null;
            await _userRepository.UpdateAsync(user);
            return null;
        }



        public async Task<string?> ResendConfirmationAsync(string email, string scheme, string host)
        {
            if (!IsValidEmail(email)) return "Invalid email format";

            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return "User not found";
            if (user.IsEmailConfirmed) return "Email already confirmed";

            user.ConfirmationToken = Guid.NewGuid().ToString("N");
            await _userRepository.UpdateAsync(user);

            string confirmationLink = $"{scheme}://{host}/api/Auth/confirm?email={Uri.EscapeDataString(email)}&token={user.ConfirmationToken}";

            await _emailService.SendEmailAsync(
                email,
                "Resend Confirmation - WebApiBoutique",
                $"<p>Click <a href='{confirmationLink}'>here</a> to confirm your email.</p>"
            );

            return null;
        }



        // Authenticate user and return JWT token
        public async Task<(string? error, object? result)> LoginAsync(LoginDTO dto)
        {
            try
            {
                // Validate login input data
                if (dto == null)
                    return ("Invalid login request", null);
                
                if (string.IsNullOrWhiteSpace(dto.Email))
                    return ("Email address is required", null);
                
                if (string.IsNullOrWhiteSpace(dto.Password))
                    return ("Password is required", null);
                
                if (!IsValidEmail(dto.Email))
                    return ("Please enter a valid email address", null);

                _logger.LogInformation("Login attempt for email: {Email}", dto.Email);

                // Find user by email
                var user = await _userRepository.GetByEmailAsync(dto.Email);
                if (user == null)
                {
                    _logger.LogWarning("Login failed - user not found: {Email}", dto.Email);
                    return ("No account found with this email address. Please check your email or register for a new account.", null);
                }

                // Verify password
                if (!await _userRepository.VerifyPasswordAsync(user, dto.Password))
                {
                    _logger.LogWarning("Login failed - incorrect password for: {Email}", dto.Email);
                    return ("Incorrect password. Please check your password and try again.", null);
                }

                // Check if email is verified before allowing login
                if (!user.IsEmailConfirmed)
                {
                    _logger.LogWarning("Login failed - email not confirmed for: {Email}", dto.Email);
                    return ("Please verify your email address before logging in. Check your inbox for the verification email.", null);
                }

                // Generate JWT token for authenticated user
                var token = _jwtTokenService.GenerateToken(user);
                
                _logger.LogInformation("Login successful for: {Email}", dto.Email);
                
                return (null, new { 
                    token,
                    user = new {
                        id = user.Id,
                        username = user.UserName,
                        email = user.Email,
                        role = user.Role,
                        businessId = user.BusinessId,
                        businessName = user.BusinessName
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error for email: {Email}", dto?.Email);
                return ("Login failed due to a server error. Please try again later.", null);
            }
        }

        // Send password reset email (secure against enumeration attacks)
        public async Task ForgotPasswordAsync(string email, string scheme, string host)
        {
            try
            {
                _logger.LogInformation("Processing forgot password request for email: {Email}", email);

                // Always process the request to prevent email enumeration attacks
                if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
                {
                    _logger.LogWarning("Invalid email format in forgot password request: {Email}", email);
                    // Add delay to prevent timing attacks that could reveal valid emails
                    await Task.Delay(Random.Shared.Next(100, 500));
                    return;
                }

                var user = await _userRepository.GetByEmailAsync(email);
                if (user == null)
                {
                    _logger.LogWarning("Forgot password request for non-existent email: {Email}", email);
                    // Still delay to prevent timing attacks
                    await Task.Delay(Random.Shared.Next(100, 500));
                    return;
                }

                // Rate limiting: prevent spam password reset requests
                if (user.PasswordResetTokenExpiry.HasValue && 
                    user.PasswordResetTokenExpiry.Value > DateTime.UtcNow.AddMinutes(-5))
                {
                    _logger.LogWarning("Rate limit exceeded for password reset: {Email}", email);
                    return; // Don't send another email if one was sent in the last 5 minutes
                }

                // Generate secure password reset token using Identity
                var token = await _userRepository.GeneratePasswordResetTokenAsync(user);
                
                // Store token and expiry time in database
                user.PasswordResetToken = token;
                user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(24); // Token valid for 24 hours
                await _userRepository.UpdateAsync(user);

                var resetLink = $"{scheme}://{host}/api/Auth/reset-password?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}";

                try
                {
                    await _emailService.SendEmailAsync(
                        email,
                        "Reset Your Password - WebAPI Boutique",
                        CreatePasswordResetEmail(user.UserName ?? string.Empty, resetLink)
                    );
                    
                    _logger.LogInformation("Password reset email sent successfully to: {Email}", email);
                }
                catch (Exception emailEx)
                {
                    _logger.LogError(emailEx, "Failed to send password reset email to: {Email}", email);
                    // Don't throw - we don't want to reveal if the email exists
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing forgot password request for: {Email}", email);
                // Don't throw - we don't want to reveal if the email exists
            }
        }

        public async Task<string?> ResetPasswordAsync(ResetPasswordDTO dto)
        {
            try
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.Email) || 
                    string.IsNullOrWhiteSpace(dto.Token) || string.IsNullOrWhiteSpace(dto.NewPassword))
                {
                    return "Invalid reset data";
                }

                if (!IsValidEmail(dto.Email))
                    return "Invalid email format";

                if (!IsStrongPassword(dto.NewPassword))
                    return "Password must be at least 8 characters long and contain uppercase, lowercase, number, and special character";

                var user = await _userRepository.GetByEmailAsync(dto.Email);
                if (user == null)
                    return "Invalid reset token";

                // Validate reset token
                if (string.IsNullOrEmpty(user.PasswordResetToken) || 
                    user.PasswordResetToken != dto.Token)
                {
                    _logger.LogWarning("Invalid reset token used for email: {Email}", dto.Email);
                    return "Invalid reset token";
                }

                // Check if token has expired
                if (!user.PasswordResetTokenExpiry.HasValue || 
                    user.PasswordResetTokenExpiry.Value < DateTime.UtcNow)
                {
                    _logger.LogWarning("Expired reset token used for email: {Email}", dto.Email);
                    return "Reset token has expired";
                }

                // Reset password using Identity
                var resetResult = await _userRepository.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
                if (!resetResult)
                {
                    _logger.LogWarning("Password reset failed using UserManager for email: {Email}", dto.Email);
                    return "Failed to reset password. Token may be invalid or password doesn't meet requirements.";
                }

                // Clear reset token after successful reset
                user.PasswordResetToken = null;
                user.PasswordResetTokenExpiry = null;
                user.UpdatedAt = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user);
                
                _logger.LogInformation("Password reset successful for email: {Email}", dto.Email);
                
                // Send confirmation email
                try
                {
                    await _emailService.SendEmailAsync(
                        user.Email,
                        "Password Reset Confirmation - WebAPI Boutique",
                        CreatePasswordResetConfirmationEmail(user.UserName ?? string.Empty)
                    );
                }
                catch (Exception emailEx)
                {
                    _logger.LogError(emailEx, "Failed to send password reset confirmation email to: {Email}", dto.Email);
                    // Don't fail the reset if confirmation email fails
                }
                
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password for email: {Email}", dto?.Email);
                return "An error occurred while resetting password";
            }
        }

        public async Task<bool> ValidateResetTokenAsync(string email, string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
                    return false;

                var user = await _userRepository.GetByEmailAsync(email);
                if (user == null)
                    return false;

                if (string.IsNullOrEmpty(user.PasswordResetToken) || 
                    user.PasswordResetToken != token)
                    return false;

                if (!user.PasswordResetTokenExpiry.HasValue || 
                    user.PasswordResetTokenExpiry.Value < DateTime.UtcNow)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating reset token for email: {Email}", email);
                return false;
            }
        }

        // Generate cryptographically secure token for email verification
        private static string GenerateSecureToken()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[32];  // 256-bit token
            rng.GetBytes(bytes);
            // URL-safe base64 encoding
            return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }

        // Generate larger secure token for password reset (higher security)
        private static string GenerateSecureResetToken()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[48]; // 384-bit token for password reset
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }

        // Generate secure 6-digit OTP code
        private static string GenerateSecureOtp()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[4];
            rng.GetBytes(bytes);
            // Generate number between 100000-999999 (6 digits)
            int otp = BitConverter.ToInt32(bytes, 0) % 900000 + 100000;
            return Math.Abs(otp).ToString("D6");
        }

        public async Task<string?> VerifyOtpAsync(OtpVerifyDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.OtpCode))
                return "Invalid OTP verification data";

            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null) return "User not found";

            if (string.IsNullOrEmpty(user.OtpCode) || user.OtpGeneratedAt == null)
                return "No OTP found for this email";

            if ((DateTime.UtcNow - user.OtpGeneratedAt.Value).TotalMinutes > 5)
                return "OTP expired";

            if (user.OtpCode != dto.OtpCode)
                return "Invalid OTP";

            user.IsEmailConfirmed = true;
            user.OtpCode = null;
            user.OtpGeneratedAt = null;
            await _userRepository.UpdateAsync(user);

            return null;
        }

        private static string CreateOtpEmail(string username, string otp)
        {
            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Your Verification Code</title>
            </head>
            <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px;'>
                <div style='background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 30px; text-align: center; border-radius: 10px 10px 0 0;'>
                    <h1 style='color: white; margin: 0; font-size: 28px;'>WebAPI Boutique</h1>
                    <p style='color: #f0f0f0; margin: 10px 0 0 0;'>Email Verification</p>
                </div>
                
                <div style='background: #ffffff; padding: 40px; border-radius: 0 0 10px 10px; box-shadow: 0 4px 10px rgba(0,0,0,0.1);'>
                    <h2 style='color: #333; margin-bottom: 20px;'>Hi {username},</h2>
                    
                    <p style='margin-bottom: 30px; font-size: 16px;'>Thank you for registering with WebAPI Boutique! Please use the verification code below to complete your registration:</p>
                    
                    <div style='text-align: center; margin: 30px 0;'>
                        <div style='background: #f8f9fa; border: 2px dashed #667eea; padding: 20px; border-radius: 10px; display: inline-block;'>
                            <p style='margin: 0; font-size: 14px; color: #666; margin-bottom: 10px;'>Your Verification Code:</p>
                            <p style='margin: 0; font-size: 32px; font-weight: bold; color: #667eea; letter-spacing: 5px; font-family: monospace;'>{otp}</p>
                        </div>
                    </div>
                    
                    <p style='margin: 20px 0; font-size: 16px; text-align: center;'>Enter this code on our website to verify your email address.</p>
                    
                    <div style='margin-top: 30px; padding-top: 20px; border-top: 1px solid #eee;'>
                        <p style='margin: 0; font-size: 14px; color: #666;'>⏰ This code will expire in 5 minutes for security reasons.</p>
                        <p style='margin: 10px 0 0 0; font-size: 14px; color: #666;'>🔒 If you didn't create an account with us, please ignore this email.</p>
                    </div>
                </div>
                
                <div style='text-align: center; margin-top: 20px; color: #666; font-size: 12px;'>
                    <p>© 2024 WebAPI Boutique. All rights reserved.</p>
                </div>
            </body>
            </html>";
        }

        private static string CreateProfessionalVerificationEmail(string username, string verificationLink)
        {
            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Verify Your Email</title>
            </head>
            <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px;'>
                <div style='background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 30px; text-align: center; border-radius: 10px 10px 0 0;'>
                    <h1 style='color: white; margin: 0; font-size: 28px;'>WebAPI Boutique</h1>
                    <p style='color: #f0f0f0; margin: 10px 0 0 0;'>Welcome to our platform!</p>
                </div>
                
                <div style='background: #ffffff; padding: 40px; border-radius: 0 0 10px 10px; box-shadow: 0 4px 10px rgba(0,0,0,0.1);'>
                    <h2 style='color: #333; margin-bottom: 20px;'>Hi {username},</h2>
                    
                    <p style='margin-bottom: 20px; font-size: 16px;'>Thank you for registering with WebAPI Boutique! To complete your registration and secure your account, please verify your email address.</p>
                    
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{verificationLink}' style='background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 15px 30px; text-decoration: none; border-radius: 25px; font-weight: bold; font-size: 16px; display: inline-block; transition: transform 0.2s;'>Verify Email Address</a>
                    </div>
                    
                    <p style='margin: 20px 0; font-size: 14px; color: #666;'>If the button above doesn't work, copy and paste this link into your browser:</p>
                    <p style='background: #f8f9fa; padding: 15px; border-radius: 5px; word-break: break-all; font-size: 14px; border-left: 4px solid #667eea;'>{verificationLink}</p>
                    
                    <div style='margin-top: 30px; padding-top: 20px; border-top: 1px solid #eee;'>
                        <p style='margin: 0; font-size: 14px; color: #666;'>This verification link will expire in 24 hours for security reasons.</p>
                        <p style='margin: 10px 0 0 0; font-size: 14px; color: #666;'>If you didn't create an account with us, please ignore this email.</p>
                    </div>
                </div>
                
                <div style='text-align: center; margin-top: 20px; color: #666; font-size: 12px;'>
                    <p>© 2024 WebAPI Boutique. All rights reserved.</p>
                </div>
            </body>
            </html>";
        }



        // Validate email format using regex pattern
        private static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        }

        // Check if password meets security requirements
        private static bool IsStrongPassword(string password)
        {
            return password.Length >= 8 &&  // Minimum 8 characters
                   password.Any(char.IsUpper) &&  // At least one uppercase letter
                   password.Any(char.IsLower) &&  // At least one lowercase letter
                   password.Any(char.IsDigit) &&  // At least one digit
                   password.Any(ch => !char.IsLetterOrDigit(ch));  // At least one special character
        }

        public async Task<UserProfileDTO?> GetUserProfileAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return null;

            return new UserProfileDTO
            {
                Id = user.Id,
                Username = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Role = user.Role,
                IsEmailConfirmed = user.IsEmailConfirmed,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }

        private static string CreatePasswordResetEmail(string username, string resetLink)
        {
            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Reset Your Password</title>
            </head>
            <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px;'>
                <div style='background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 30px; text-align: center; border-radius: 10px 10px 0 0;'>
                    <h1 style='color: white; margin: 0; font-size: 28px;'>🔐 Password Reset</h1>
                    <p style='color: #f0f0f0; margin: 10px 0 0 0;'>WebAPI Boutique</p>
                </div>
                
                <div style='background: #ffffff; padding: 40px; border-radius: 0 0 10px 10px; box-shadow: 0 4px 10px rgba(0,0,0,0.1);'>
                    <h2 style='color: #333; margin-bottom: 20px;'>Hi {username},</h2>
                    
                    <p style='margin-bottom: 20px; font-size: 16px;'>We received a request to reset your password for your WebAPI Boutique account. If you didn't make this request, you can safely ignore this email.</p>
                    
                    <div style='background: #fff3cd; border: 1px solid #ffeaa7; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                        <p style='margin: 0; font-size: 14px; color: #856404;'>
                            <strong>⚠️ Security Notice:</strong> This reset link will expire in 15 minutes for your security.
                        </p>
                    </div>
                    
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{resetLink}' style='background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 15px 30px; text-decoration: none; border-radius: 25px; font-weight: bold; font-size: 16px; display: inline-block; transition: transform 0.2s;'>Reset My Password</a>
                    </div>
                    
                    <p style='margin: 20px 0; font-size: 14px; color: #666;'>If the button above doesn't work, copy and paste this link into your browser:</p>
                    <p style='background: #f8f9fa; padding: 15px; border-radius: 5px; word-break: break-all; font-size: 12px; border-left: 4px solid #667eea; font-family: monospace;'>{resetLink}</p>
                    
                    <div style='margin-top: 30px; padding-top: 20px; border-top: 1px solid #eee;'>
                        <p style='margin: 0; font-size: 14px; color: #666;'>🛡️ <strong>Security Tips:</strong></p>
                        <ul style='font-size: 14px; color: #666; margin: 10px 0;'>
                            <li>Never share your password with anyone</li>
                            <li>Use a strong, unique password</li>
                            <li>Enable two-factor authentication when available</li>
                        </ul>
                        <p style='margin: 15px 0 0 0; font-size: 14px; color: #666;'>If you didn't request this reset, please contact our support team immediately.</p>
                    </div>
                </div>
                
                <div style='text-align: center; margin-top: 20px; color: #666; font-size: 12px;'>
                    <p>© 2024 WebAPI Boutique. All rights reserved.</p>
                    <p>This is an automated message, please do not reply to this email.</p>
                </div>
            </body>
            </html>";
        }

        private static string CreatePasswordResetConfirmationEmail(string username)
        {
            var resetTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Password Reset Successful</title>
            </head>
            <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px;'>
                <div style='background: linear-gradient(135deg, #28a745 0%, #20c997 100%); padding: 30px; text-align: center; border-radius: 10px 10px 0 0;'>
                    <h1 style='color: white; margin: 0; font-size: 28px;'>✅ Password Reset Successful</h1>
                    <p style='color: #f0f0f0; margin: 10px 0 0 0;'>WebAPI Boutique</p>
                </div>
                
                <div style='background: #ffffff; padding: 40px; border-radius: 0 0 10px 10px; box-shadow: 0 4px 10px rgba(0,0,0,0.1);'>
                    <h2 style='color: #333; margin-bottom: 20px;'>Hi {username},</h2>
                    
                    <p style='margin-bottom: 20px; font-size: 16px;'>Your password has been successfully reset. You can now log in to your WebAPI Boutique account using your new password.</p>
                    
                    <div style='background: #d4edda; border: 1px solid #c3e6cb; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                        <p style='margin: 0; font-size: 14px; color: #155724;'>
                            <strong>🔒 Security Confirmation:</strong> Your account is now secured with your new password.
                        </p>
                    </div>
                    
                    <div style='margin-top: 30px; padding-top: 20px; border-top: 1px solid #eee;'>
                        <p style='margin: 0; font-size: 14px; color: #666;'>⚠️ <strong>Important:</strong> If you didn't reset your password, please contact our support team immediately as your account may be compromised.</p>
                        <p style='margin: 15px 0 0 0; font-size: 14px; color: #666;'>Reset Time: {resetTime} UTC</p>
                    </div>
                </div>
                
                <div style='text-align: center; margin-top: 20px; color: #666; font-size: 12px;'>
                    <p>© 2024 WebAPI Boutique. All rights reserved.</p>
                </div>
            </body>
            </html>";
        }
    }
}

