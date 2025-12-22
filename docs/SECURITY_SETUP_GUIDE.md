# Security Setup Guide

## CRITICAL: Secure Your Sensitive Data

### 1. Email Configuration (User Secrets)

**Remove from appsettings.json and use User Secrets instead:**

```bash
# Navigate to WebApiBoutique folder
cd WebApiBoutique

# Initialize user secrets
dotnet user-secrets init

# Set email configuration
dotnet user-secrets set "Email:SmtpHost" "smtp.gmail.com"
dotnet user-secrets set "Email:SmtpPort" "587"
dotnet user-secrets set "Email:SenderEmail" "your-email@gmail.com"
dotnet user-secrets set "Email:SenderPassword" "your-app-password"
dotnet user-secrets set "Email:SenderName" "WebAPI Boutique"
dotnet user-secrets set "Email:EnableSsl" "true"
dotnet user-secrets set "Email:Timeout" "30000"

# Set JWT key
dotnet user-secrets set "Jwt:Key" "MyVeryLongSecretKeyForJWTTokenGeneration2024!@#$%^&*()_+1234567890"
```

### 2. Production Environment Variables

**For production deployment, use environment variables:**

```bash
# Linux/Mac
export Email__SmtpHost="smtp.gmail.com"
export Email__SenderPassword="your-password"
export Jwt__Key="your-secret-key"

# Windows
set Email__SmtpHost=smtp.gmail.com
set Email__SenderPassword=your-password
set Jwt__Key=your-secret-key
```

### 3. Update appsettings.json

**Remove sensitive data from appsettings.json:**

```json
{
  "Email": {
    "SmtpHost": "",
    "SmtpPort": "587",
    "SenderEmail": "",
    "SenderPassword": "",
    "SenderName": "WebAPI Boutique",
    "EnableSsl": true,
    "Timeout": 30000
  },
  "Jwt": {
    "Issuer": "WebApiBoutique",
    "Audience": "WebApiBoutiqueClient",
    "Key": "",
    "ExpiryMinutes": 60
  }
}
```

### 4. CORS Configuration

**Update Program.cs to restrict CORS in production:**

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
        else
        {
            policy.WithOrigins("https://your-production-domain.com")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
    });
});
```

### 5. Gmail App Password Setup

1. Go to Google Account Settings
2. Enable 2-Factor Authentication
3. Go to Security > App Passwords
4. Generate new app password for "Mail"
5. Use this password in user secrets (NOT your Gmail password)

## Security Checklist

- [ ] Moved email credentials to user secrets
- [ ] Moved JWT key to user secrets
- [ ] Updated appsettings.json to remove sensitive data
- [ ] Configured CORS for production
- [ ] Generated Gmail app password
- [ ] Added .gitignore for secrets.json
- [ ] Tested application with user secrets

## Important Notes

- **NEVER** commit appsettings.json with real credentials
- **ALWAYS** use user secrets for development
- **ALWAYS** use environment variables for production
- User secrets are stored at: `%APPDATA%\Microsoft\UserSecrets\<user_secrets_id>\secrets.json`
