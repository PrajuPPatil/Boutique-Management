# 🚨 FIX: OTP Emails Not Sending After Registration

## Problem
After successful registration, OTP emails are not being sent to users because Gmail App Password is not configured.

## ⚡ Quick Fix (5 minutes)

### Step 1: Enable 2-Factor Authentication on Gmail
1. Go to [Google Account Settings](https://myaccount.google.com/)
2. Click **Security** → **2-Step Verification**
3. Follow the setup process to enable 2FA

### Step 2: Generate Gmail App Password
1. Go to [Google Account Settings](https://myaccount.google.com/)
2. Click **Security** → **App passwords**
3. Select **Mail** and **Windows Computer** (or Other)
4. Click **Generate**
5. **Copy the 16-character password** (format: `abcd efgh ijkl mnop`)

### Step 3: Update Configuration
Replace `YOUR_GMAIL_APP_PASSWORD_HERE` in `appsettings.json` with your actual App Password:

```json
{
  "Email": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": "587",
    "SenderEmail": "prajaktap033@gmail.com",
    "SenderPassword": "abcd efgh ijkl mnop",  // ← Your actual App Password here
    "SenderName": "WebAPI Boutique",
    "EnableSsl": true,
    "Timeout": 30000
  }
}
```

### Step 4: Test the Fix
1. **Restart your API server**
2. **Test email sending**:
   ```http
   POST /api/Auth/test-email
   Content-Type: application/json
   
   {
     "email": "your-test-email@gmail.com"
   }
   ```
3. **Test registration**:
   ```http
   POST /api/Auth/register
   Content-Type: application/json
   
   {
     "username": "testuser",
     "email": "your-email@gmail.com",
     "password": "Test123!@#",
     "confirmPassword": "Test123!@#",
     "confirmationMethod": "otp"
   }
   ```

## ✅ Expected Results After Fix

1. **Registration Success**: User gets success message
2. **OTP Email Sent**: User receives email with 6-digit code
3. **Console Output**: You'll see OTP in server logs for testing
4. **Verification Works**: User can enter OTP to verify email

## 🔍 Troubleshooting

### If emails still don't send:
1. **Check App Password**: Make sure you copied it correctly (no spaces)
2. **Check 2FA**: Must be enabled before generating App Password
3. **Check Firewall**: Ensure port 587 is not blocked
4. **Check Logs**: Look for detailed error messages in server console

### Common Errors:
- `"Authentication failed"` → Wrong App Password
- `"Less secure apps"` → Need App Password, not regular password
- `"Connection timeout"` → Firewall/network issue

## 🚀 Alternative: Use Different Email Provider

If Gmail doesn't work, you can use other providers:

### Outlook/Hotmail:
```json
{
  "Email": {
    "SmtpHost": "smtp-mail.outlook.com",
    "SmtpPort": "587",
    "SenderEmail": "your-email@outlook.com",
    "SenderPassword": "your-outlook-app-password"
  }
}
```

### SendGrid (Production Recommended):
```json
{
  "Email": {
    "SmtpHost": "smtp.sendgrid.net",
    "SmtpPort": "587",
    "SenderEmail": "your-verified-email@yourdomain.com",
    "SenderPassword": "your-sendgrid-api-key"
  }
}
```

## 📝 Notes
- The registration code is working perfectly
- The OTP generation is working correctly
- Only the email sending is failing due to authentication
- Once fixed, users will receive OTP emails immediately after registration