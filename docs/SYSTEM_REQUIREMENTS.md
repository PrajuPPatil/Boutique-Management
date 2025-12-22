# System Requirements Check Script

## Windows PowerShell Script
```powershell
# Save as: check-requirements.ps1
# Run: powershell -ExecutionPolicy Bypass -File check-requirements.ps1

Write-Host "=== Boutique Management System Requirements Check ===" -ForegroundColor Green

# Check .NET 8.0
try {
    $dotnetVersion = dotnet --version
    if ($dotnetVersion -like "8.*") {
        Write-Host "✅ .NET 8.0 SDK: $dotnetVersion" -ForegroundColor Green
    } else {
        Write-Host "❌ .NET 8.0 SDK not found. Current: $dotnetVersion" -ForegroundColor Red
        Write-Host "   Download from: https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor Yellow
    }
} catch {
    Write-Host "❌ .NET SDK not installed" -ForegroundColor Red
    Write-Host "   Download from: https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor Yellow
}

# Check SQL Server LocalDB
try {
    $localdb = sqllocaldb info
    Write-Host "✅ SQL Server LocalDB available" -ForegroundColor Green
} catch {
    Write-Host "❌ SQL Server LocalDB not found" -ForegroundColor Red
    Write-Host "   Install SQL Server Express or LocalDB" -ForegroundColor Yellow
}

# Check Git
try {
    $gitVersion = git --version
    Write-Host "✅ Git: $gitVersion" -ForegroundColor Green
} catch {
    Write-Host "❌ Git not installed" -ForegroundColor Red
    Write-Host "   Download from: https://git-scm.com/download/win" -ForegroundColor Yellow
}

# Check Entity Framework Tools
try {
    $efVersion = dotnet ef --version
    Write-Host "✅ Entity Framework Tools: $efVersion" -ForegroundColor Green
} catch {
    Write-Host "❌ EF Tools not installed" -ForegroundColor Red
    Write-Host "   Run: dotnet tool install --global dotnet-ef" -ForegroundColor Yellow
}

Write-Host "\n=== System Info ===" -ForegroundColor Cyan
Write-Host "OS: $([System.Environment]::OSVersion.VersionString)"
Write-Host "RAM: $([math]::Round((Get-CimInstance Win32_PhysicalMemory | Measure-Object -Property capacity -Sum).sum / 1gb, 2)) GB"
Write-Host "Free Space: $([math]::Round((Get-CimInstance Win32_LogicalDisk -Filter "DriveType=3" | Where-Object {$_.DeviceID -eq 'C:'} | Select-Object -ExpandProperty FreeSpace) / 1gb, 2)) GB"

Write-Host "\n=== Next Steps ===" -ForegroundColor Cyan
Write-Host "1. Install missing requirements above"
Write-Host "2. Clone repository: git clone https://github.com/PrajuPPatil/Boutique-Management.git"
Write-Host "3. Follow SETUP_GUIDE.md instructions"
```

## Manual Verification Commands

### Check Installations
```bash
# .NET Version
dotnet --version

# Entity Framework Tools
dotnet ef --version

# Git Version
git --version

# SQL Server LocalDB (Windows)
sqllocaldb info
```

### Test Database Connection
```bash
# Test LocalDB connection (Windows)
sqlcmd -S "(LocalDB)\MSSQLLocalDB" -Q "SELECT @@VERSION"
```

### Verify Project Build
```bash
# After cloning repository
cd Boutique-Management/WebApiBoutique
dotnet build --verbosity normal

cd ../Boutique.Client
dotnet build --verbosity normal
```