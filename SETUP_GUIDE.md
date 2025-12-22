# 🚀 Complete System Setup Guide

## System Requirements

### Operating System
- Windows 10/11 (64-bit)
- macOS 10.15+ 
- Linux (Ubuntu 18.04+, CentOS 7+)

### Hardware Requirements
- RAM: 8GB minimum, 16GB recommended
- Storage: 10GB free space
- CPU: Dual-core processor minimum

---

## Step 1: Install Prerequisites

### 1.1 Install .NET 8.0 SDK

**Windows:**
1. Download from: https://dotnet.microsoft.com/download/dotnet/8.0
2. Run installer and follow prompts
3. Verify installation:
```cmd
dotnet --version
```

**macOS:**
```bash
brew install dotnet
# OR download from Microsoft website
```

**Linux (Ubuntu):**
```bash
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update
sudo apt-get install -y dotnet-sdk-8.0
```

### 1.2 Install SQL Server

**Windows:**
1. Download SQL Server 2019+ or SQL Server Express (free)
2. Install with default settings
3. Enable SQL Server Authentication

**Alternative - SQL Server LocalDB (Recommended for Development):**
```cmd
# Usually comes with Visual Studio or can be downloaded separately
sqllocaldb info
```

**macOS/Linux:**
```bash
# Use Docker for SQL Server
docker pull mcr.microsoft.com/mssql/server:2019-latest
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourPassword123!" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2019-latest
```

### 1.3 Install Git

**Windows:**
- Download from: https://git-scm.com/download/win

**macOS:**
```bash
brew install git
```

**Linux:**
```bash
sudo apt-get install git
```

### 1.4 Install Code Editor (Choose One)

**Visual Studio 2022 (Windows - Recommended):**
- Download Community Edition (free)
- Include ASP.NET and web development workload
- Include .NET desktop development workload

**Visual Studio Code (Cross-platform):**
- Download from: https://code.visualstudio.com/
- Install C# extension
- Install C# Dev Kit extension

---

## Step 2: Clone and Setup Project

### 2.1 Clone Repository
```bash
git clone https://github.com/PrajuPPatil/Boutique-Management.git
cd Boutique-Management
```

### 2.2 Restore NuGet Packages
```bash
# Backend
cd WebApiBoutique
dotnet restore

# Frontend
cd ../Boutique.Client
dotnet restore
```

---

## Step 3: Database Setup

### 3.1 Update Connection String
Edit `WebApiBoutique/appsettings.json`:

**For LocalDB (Windows):**
```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Boutique_Management;Integrated Security=True;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

**For SQL Server:**
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=Boutique_Management;User Id=sa;Password=YourPassword123!;TrustServerCertificate=True;"
}
```

**For Docker SQL Server:**
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=Boutique_Management;User Id=sa;Password=YourPassword123!;TrustServerCertificate=True;"
}
```

### 3.2 Run Database Migrations
```bash
cd WebApiBoutique
dotnet ef database update
```

**If Entity Framework tools not installed:**
```bash
dotnet tool install --global dotnet-ef
```

---

## Step 4: Configure Application Settings

### 4.1 Update JWT Secret
Edit `WebApiBoutique/appsettings.json`:
```json
"Jwt": {
  "Key": "YourSecretKeyAtLeast32CharactersLong123456789",
  "Issuer": "WebApiBoutique",
  "Audience": "WebApiBoutiqueClient",
  "ExpiryMinutes": 60
}
```

### 4.2 Configure Email (Optional)
**For Gmail:**
1. Enable 2-factor authentication
2. Generate App Password
3. Update `appsettings.json`:
```json
"Email": {
  "SmtpHost": "smtp.gmail.com",
  "SmtpPort": "587",
  "SenderEmail": "your-email@gmail.com",
  "SenderPassword": "your-16-digit-app-password",
  "SenderName": "Boutique Management System",
  "EnableSsl": true
}
```

---

## Step 5: Build and Run

### 5.1 Build Projects
```bash
# Backend
cd WebApiBoutique
dotnet build

# Frontend
cd ../Boutique.Client
dotnet build
```

### 5.2 Run Backend API
```bash
cd WebApiBoutique
dotnet run
```
**Backend will run on:** http://localhost:5100

### 5.3 Run Frontend (New Terminal)
```bash
cd Boutique.Client
dotnet run
```
**Frontend will run on:** http://localhost:5000

---

## Step 6: Verify Installation

### 6.1 Check API
- Open: http://localhost:5100/swagger
- Should see Swagger API documentation

### 6.2 Check Frontend
- Open: http://localhost:5000
- Should see login page

### 6.3 Test Registration
1. Click "Create Account"
2. Fill registration form
3. Should receive success message

---

## Troubleshooting

### Database Issues
```bash
# Reset database
dotnet ef database drop
dotnet ef database update
```

### Port Conflicts
Update `Properties/launchSettings.json` in both projects:
```json
"applicationUrl": "http://localhost:5101"
```

### Package Issues
```bash
# Clear NuGet cache
dotnet nuget locals all --clear
dotnet restore
```

### SSL Certificate Issues
```bash
# Trust development certificates
dotnet dev-certs https --trust
```

---

## Production Deployment

### Environment Variables
Set these for production:
```bash
ASPNETCORE_ENVIRONMENT=Production
CONNECTION_STRING=your-production-db-string
JWT_SECRET=your-production-jwt-secret
EMAIL_PASSWORD=your-production-email-password
```

### Publish Applications
```bash
# Backend
cd WebApiBoutique
dotnet publish -c Release -o ./publish

# Frontend
cd ../Boutique.Client
dotnet publish -c Release -o ./publish
```

---

## Quick Start Checklist

- [ ] Install .NET 8.0 SDK
- [ ] Install SQL Server/LocalDB
- [ ] Install Git
- [ ] Install Visual Studio/VS Code
- [ ] Clone repository
- [ ] Restore packages
- [ ] Update connection string
- [ ] Run database migrations
- [ ] Update JWT secret
- [ ] Configure email (optional)
- [ ] Build projects
- [ ] Run backend (port 5100)
- [ ] Run frontend (port 5000)
- [ ] Test application

**Total Setup Time: 30-60 minutes**