# Complete Email Setup Guide for Forgot Password Functionality

## 🚀 Quick Setup

### 1. Gmail App Password Setup (Recommended)

1. **Enable 2-Factor Authentication** on your Gmail account
2. **Generate App Password**:
   - Go to Google Account settings
   - Security → 2-Step Verification → App passwords
   - Select "Mail" and generate password
   - Copy the 16-character password

3. **Update appsettings.json**:
```json
{
  "Email": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": "587",
    "SenderEmail": "your-email@gmail.com",
    "SenderPassword": "your-16-char-app-password",
    "SenderName": "WebAPI Boutique",
    "EnableSsl": true,
    "Timeout": 30000
  }
}
```

### 2. Alternative SMTP Providers

#### SendGrid (Production Recommended)
```json
{
  "Email": {
    "SmtpHost": "smtp.sendgrid.net",
    "SmtpPort": "587",
    "SenderEmail": "your-verified-email@yourdomain.com",
    "SenderPassword": "your-sendgrid-api-key",
    "SenderName": "WebAPI Boutique"
  }
}
```

#### Outlook/Hotmail
```json
{
  "Email": {
    "SmtpHost": "smtp-mail.outlook.com",
    "SmtpPort": "587",
    "SenderEmail": "your-email@outlook.com",
    "SenderPassword": "your-app-password",
    "SenderName": "WebAPI Boutique"
  }
}
```

## 🔧 Implementation Features

### Security Features Implemented:
- ✅ **Cryptographically secure tokens** (48-byte random)
- ✅ **Token expiration** (15 minutes)
- ✅ **Rate limiting** (5-minute cooldown between requests)
- ✅ **Email enumeration protection** (always returns success)
- ✅ **Timing attack prevention** (random delays)
- ✅ **Password strength validation**
- ✅ **Token invalidation after use**

### API Endpoints:

#### 1. Forgot Password
```http
POST /api/Auth/forgot-password
Content-Type: application/json

{
  "email": "user@example.com"
}
```

**Response:**
```json
{
  "message": "If the email is registered, you will receive a password reset link.",
  "success": true
}
```

#### 2. Reset Password (API)
```http
POST /api/Auth/reset-password
Content-Type: application/json

{
  "email": "user@example.com",
  "token": "secure-reset-token",
  "newPassword": "NewSecurePassword123!"
}
```

#### 3. Reset Password (Web Page)
```http
GET /api/Auth/reset-password?email=user@example.com&token=secure-reset-token
```
Returns HTML page for password reset.

## 📧 Email Templates

### Password Reset Email Features:
- 🎨 **Professional HTML design**
- 📱 **Mobile responsive**
- 🔒 **Security warnings and tips**
- ⏰ **Clear expiration notice**
- 🔗 **Secure reset link**

### Confirmation Email:
- ✅ **Reset confirmation**
- 🕐 **Timestamp of reset**
- ⚠️ **Security alert if unauthorized**

## 🧪 Testing

### Test the Email Configuration:
```http
POST /api/Auth/test-email
Content-Type: application/json

{
  "email": "your-test-email@example.com"
}
```

### Test Forgot Password Flow:
1. Send forgot password request
2. Check email for reset link
3. Click link or use API to reset
4. Verify new password works

## 🛡️ Security Best Practices

### Implemented Security Measures:
1. **No Information Disclosure**: Always returns success regardless of email existence
2. **Rate Limiting**: Prevents spam and brute force attacks
3. **Short Token Expiry**: 15-minute window reduces attack surface
4. **Secure Token Generation**: Cryptographically secure random tokens
5. **Password Validation**: Enforces strong password requirements
6. **Audit Logging**: All actions are logged for security monitoring

### Additional Recommendations:
- Use HTTPS in production
- Implement CAPTCHA for additional protection
- Monitor for suspicious patterns
- Set up email delivery monitoring
- Use environment variables for sensitive config

## 🚨 Troubleshooting

### Common Issues:

1. **Email not sending**:
   - Check SMTP credentials
   - Verify app password (not regular password)
   - Check firewall/network restrictions

2. **Token validation fails**:
   - Check token expiration (15 minutes)
   - Verify email and token match exactly
   - Ensure token hasn't been used already

3. **Gmail authentication fails**:
   - Enable 2FA first
   - Generate new app password
   - Use app password, not account password

### Debug Endpoints:
```http
GET /api/Auth/email-config-test
```
Returns current email configuration (without password).

## 📝 Environment Variables (Production)

For production, use environment variables:

```bash
Email__SmtpHost=smtp.gmail.com
Email__SmtpPort=587
Email__SenderEmail=your-email@gmail.com
Email__SenderPassword=your-app-password
Email__SenderName=WebAPI Boutique
```

## 🔄 Migration Notes

If upgrading from existing implementation:
1. Update controller endpoints (`/forgot` → `/forgot-password`)
2. Update frontend calls to new endpoints
3. Test email delivery thoroughly
4. Monitor logs for any issues

## 📊 Monitoring

Key metrics to monitor:
- Password reset request frequency
- Email delivery success rate
- Token usage patterns
- Failed reset attempts
- Response times

The implementation is now production-ready with enterprise-level security features!