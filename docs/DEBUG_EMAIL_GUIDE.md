# Email OTP Debugging Guide

## Issues Fixed

### 1. **CORS Configuration**
- Fixed CORS middleware order (now before Authentication)
- Proper CORS policy configuration

### 2. **Email Service Improvements**
- Added comprehensive logging
- Better error handling with try-catch blocks
- Improved SMTP configuration
- Added timeout settings
- Better email formatting

### 3. **Package Version Conflicts**
- Downgraded EF Core from 9.0.9 to 8.0.8 (compatible with .NET 8.0)
- Updated HotChocolate packages to compatible versions
- Removed conflicting packages

### 4. **Nullable Reference Warnings**
- Fixed all nullable reference warnings in models
- Added proper null handling

### 5. **Configuration Issues**
- Added proper JWT key validation
- Better configuration validation
- Enhanced logging configuration

## Debugging Steps for OTP Email Issue

### Step 1: Test Email Configuration
Use the new test endpoint:
```
POST /api/Auth/test-email
{
  "email": "your-test-email@gmail.com"
}
```

### Step 2: Check Logs
The application now has detailed logging. Check the console output for:
- Email configuration validation
- SMTP connection attempts
- Email sending success/failure

### Step 3: Verify Gmail Settings
Ensure your Gmail account has:
1. **2-Factor Authentication enabled**
2. **App Password generated** (not your regular password)
3. **Less secure app access** (if using regular password)

### Step 4: Check Registration Flow
1. Register with `ConfirmationMethod: "otp"`
2. Check logs for OTP generation
3. Verify email sending attempt
4. Check for any SMTP errors

### Step 5: Common Issues & Solutions

#### Issue: "SMTP Authentication Failed"
**Solution:** 
- Use App Password instead of regular Gmail password
- Enable 2FA on Gmail account
- Generate new App Password

#### Issue: "SMTP Connection Timeout"
**Solution:**
- Check firewall settings
- Verify port 587 is not blocked
- Try port 465 with SSL

#### Issue: "OTP Generated but Email Not Sent"
**Solution:**
- Check the logs for specific SMTP errors
- Verify email configuration in appsettings.Development.json

## Updated Email Configuration

### appsettings.Development.json
```json
{
  "Email": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": "587",
    "SenderEmail": "prajupp2001@gmail.com",
    "SenderPassword": "your-app-password-here"
  }
}
```

## Testing Commands

### Build and Run
```bash
dotnet build
dotnet run
```

### Test Registration with OTP
```bash
curl -X POST "https://localhost:7000/api/Auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "email": "test@example.com",
    "password": "Test123!@#",
    "confirmationMethod": "otp"
  }'
```

### Test Email Directly
```bash
curl -X POST "https://localhost:7000/api/Auth/test-email" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com"
  }'
```

## Log Levels
The application now logs at DEBUG level for email services. You'll see:
- Configuration validation
- OTP generation
- Email sending attempts
- SMTP connection details
- Success/failure messages

## Next Steps
1. Run the application
2. Try the test-email endpoint first
3. If test email works, try registration with OTP
4. Check logs for any issues
5. Verify OTP is received in email

## Gmail App Password Setup
1. Go to Google Account settings
2. Security → 2-Step Verification
3. App passwords → Generate new password
4. Use this password in appsettings.Development.json