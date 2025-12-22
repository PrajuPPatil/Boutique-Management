# 🔧 Troubleshooting Guide

## Common Errors and Solutions

### 1. Registration/Customer Creation Errors

#### Error: "Database connection failed"
**Solution:**
```bash
# Install SQL Server LocalDB
# Or update connection string in appsettings.json
```

#### Error: "Email service failed"
**Solution:**
- Email is optional for basic functionality
- Update email settings or disable email features temporarily

#### Error: "JWT token invalid"
**Solution:**
- Ensure JWT Key is at least 32 characters
- Check if backend and frontend are using same JWT settings

### 2. Database Issues

#### Error: "Cannot create database"
**Solutions:**
1. Install SQL Server LocalDB
2. Or change to SQL Server Express
3. Update connection string accordingly

#### Error: "Migration failed"
**Solution:**
```bash
cd WebApiBoutique
dotnet ef database drop
dotnet ef database update
```

### 3. API Connection Issues

#### Error: "API not reachable"
**Solutions:**
1. Ensure backend is running on port 5100
2. Check firewall settings
3. Verify CORS configuration

#### Error: "CORS policy error"
**Solution:**
- Backend allows all origins in development
- Check if both frontend and backend are running

### 4. Authentication Issues

#### Error: "Login failed"
**Solutions:**
1. Register a new account first
2. Check if database has user data
3. Verify JWT configuration

#### Error: "Token expired"
**Solution:**
- Tokens expire after 60 minutes
- Login again to get new token

### 5. Port Conflicts

#### Error: "Port already in use"
**Solution:**
Update ports in `launchSettings.json`:
```json
"applicationUrl": "http://localhost:5101"
```

## Quick Fixes

### Reset Everything
```bash
# Delete database
cd WebApiBoutique
dotnet ef database drop

# Recreate database
dotnet ef database update

# Clear browser storage
# F12 -> Application -> Clear Storage
```

### Minimal Working Setup
1. Don't configure email (optional)
2. Use default JWT key for testing
3. Ensure LocalDB is installed
4. Run database migrations

## Getting Help
- Check console logs in browser (F12)
- Check backend logs in terminal
- Verify all prerequisites are installed