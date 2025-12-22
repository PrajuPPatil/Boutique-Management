using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebApiBoutique.Services.Interface;
 
namespace WebApiBoutique.Services
{
    // Service class for sending emails via SMTP with comprehensive error handling
    public class EmailService : IEmailService
    {
        // Configuration service for email settings
        private readonly IConfiguration _config;
        // Logger for tracking email operations and debugging
        private readonly ILogger<EmailService> _logger;
 
        // Constructor to initialize configuration and logging services
        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }
 
        // Send HTML email with comprehensive validation and error handling
        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            try
            {
                _logger.LogInformation("Starting email send process to: {Email}", toEmail);
                
                // Load and validate email configuration from appsettings
                var smtpHost = _config["Email:SmtpHost"];           // SMTP server address
                var smtpPortStr = _config["Email:SmtpPort"];        // SMTP port (usually 587 for TLS)
                var senderEmail = _config["Email:SenderEmail"];     // Sender's email address
                var senderPassword = _config["Email:SenderPassword"]; // App password for authentication
                var senderName = _config["Email:SenderName"] ?? "WebAPI Boutique"; // Display name
                var enableSsl = _config.GetValue<bool>("Email:EnableSsl", true);     // SSL/TLS encryption
                var timeout = _config.GetValue<int>("Email:Timeout", 30000);        // Connection timeout
                
                _logger.LogDebug("Email Configuration - Host: {Host}, Port: {Port}, Sender: {Sender}", 
                    smtpHost, smtpPortStr, senderEmail);
                
                // Validate required SMTP host configuration
                if (string.IsNullOrWhiteSpace(smtpHost))
                {
                    _logger.LogError("SMTP host configuration is missing");
                    throw new InvalidOperationException("SMTP host configuration is missing.");
                }
                
                // Validate SMTP port configuration
                if (string.IsNullOrWhiteSpace(smtpPortStr))
                {
                    _logger.LogError("SMTP port configuration is missing");
                    throw new InvalidOperationException("SMTP port configuration is missing.");
                }
 
                // Parse and validate SMTP port number
                if (!int.TryParse(smtpPortStr, out int smtpPort))
                {
                    _logger.LogError("SMTP port configuration is invalid: {Port}", smtpPortStr);
                    throw new InvalidOperationException("SMTP port configuration is invalid.");
                }
 
                // Validate sender credentials
                if (string.IsNullOrWhiteSpace(senderEmail) || string.IsNullOrWhiteSpace(senderPassword))
                {
                    _logger.LogError("Sender email or password configuration is missing");
                    throw new InvalidOperationException("Sender email or password configuration is missing.");
                }
 
                // Create and configure SMTP client with security settings
                using var smtpClient = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(senderEmail, senderPassword), // Authentication
                    EnableSsl = enableSsl,                    // Enable SSL/TLS encryption
                    DeliveryMethod = SmtpDeliveryMethod.Network, // Send via network
                    UseDefaultCredentials = false,           // Use custom credentials
                    Timeout = timeout                        // Connection timeout in milliseconds
                };
 
                // Create email message with HTML content
                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, senderName), // Sender info with display name
                    Subject = subject,                               // Email subject line
                    Body = htmlMessage,                             // HTML email content
                    IsBodyHtml = true,                              // Enable HTML formatting
                    Priority = MailPriority.Normal                  // Standard priority
                };
                
                // Add recipient email address
                mailMessage.To.Add(new MailAddress(toEmail));
                
                _logger.LogInformation("Attempting to send email via SMTP: {Host}:{Port}", smtpHost, smtpPort);
                
                // Send email asynchronously
                await smtpClient.SendMailAsync(mailMessage);
                
                _logger.LogInformation("Email sent successfully to: {Email}", toEmail);
            }
            catch (SmtpException smtpEx)
            {
                // Handle SMTP-specific errors (authentication, server issues, etc.)
                _logger.LogError(smtpEx, "SMTP error occurred while sending email to {Email}. StatusCode: {StatusCode}", 
                    toEmail, smtpEx.StatusCode);
                throw new InvalidOperationException($"Failed to send email via SMTP: {smtpEx.Message}", smtpEx);
            }
            catch (Exception ex)
            {
                // Handle any other unexpected errors
                _logger.LogError(ex, "Unexpected error occurred while sending email to {Email}", toEmail);
                throw new InvalidOperationException($"Failed to send email: {ex.Message}", ex);
            }
        }
    }
}