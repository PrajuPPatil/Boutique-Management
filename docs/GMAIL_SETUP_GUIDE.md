# Gmail App Password Setup Guide

## ⚠️ CRITICAL: You're using regular Gmail password which won't work!

Gmail requires **App Passwords** for third-party applications since 2022.

## 🔧 Step-by-Step Setup:

### 1. Enable 2-Factor Authentication
1. Go to [Google Account Settings](https://myaccount.google.com/)
2. Click **Security** → **2-Step Verification**
3. Follow the setup process

### 2. Generate App Password
1. Go to [Google Account Settings](https://myaccount.google.com/)
2. Click **Security** → **App passwords**
3. Select **Mail** and **Windows Computer**
4. Click **Generate**
5. Copy the 16-character password (like: `abcd efgh ijkl mnop`)

### 3. Update appsettings.json
```json
{
  "Email": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": "587",
    "SenderEmail": "prajaktap033@gmail.com",
    "SenderPassword": "your-16-char-app-password-here"
  }
}
```

## 🧪 Test Your Configuration

### 1. Test Email Config
```
GET /api/Auth/email-config-test
```

### 2. Test Email Sending
```
POST /api/Auth/test-email
{
  "email": "your-test-email@gmail.com"
}
```

### 3. Test Registration
```
POST /api/Auth/register
{
  "username": "testuser",
  "email": "your-email@gmail.com",
  "password": "Test123!@#"
}
```

## 🔍 Common Issues:

1. **"Authentication failed"** → Use App Password, not regular password
2. **"Less secure apps"** → Enable 2FA and use App Password
3. **"Connection timeout"** → Check firewall/antivirus blocking port 587

## ✅ Quick Fix:
Replace `"Praju@1234"` with your Gmail App Password in appsettings.json