# 👔 Boutique Management System

A professional, full-stack boutique management system built with **ASP.NET Core Web API** and **Blazor WebAssembly**.

![Status](https://img.shields.io/badge/Status-Production%20Ready-success)
![.NET](https://img.shields.io/badge/.NET-8.0-blue)
![License](https://img.shields.io/badge/License-MIT-green)

---

## ✨ Features

### 🔐 Authentication & Authorization
- User registration with email verification
- JWT-based authentication
- Role-based access control (Admin/User)
- Password reset with OTP
- Secure password storage

### 👥 Customer Management
- Complete CRUD operations
- Customer profiles with contact details
- Gender-based categorization
- Search and filter capabilities
- Customer history tracking

### 📏 Measurement Management
- Gender-specific measurement forms
- Multiple garment types (Kurta, Shirt, Pant, Blazer, etc.)
- Real-time validation with realistic ranges
- Measurement history per customer
- Edit and update measurements
- Visual measurement cards

### 📦 Order Management
- Create orders with customer and measurement selection
- Priority levels (Regular, Urgent, Express)
- Order status tracking (Pending, InProgress, ReadyForDelivery, Delivered)
- Automatic delivery date calculation
- Order filtering and search
- Payment tracking per order

### 💰 Payment Management ✨NEW
- Complete payment processing system
- Multiple payment methods (Cash, Card, UPI, Bank Transfer, Cheque)
- Payment status tracking (Pending, Completed, Failed, Refunded)
- Payment history per customer
- Payment summary dashboard
- Transaction ID tracking
- Automatic order balance updates

### 📊 Dashboard & Analytics
- Real-time statistics
- Order overview
- Payment summaries
- Customer insights

---

## 🏗️ Architecture

### Technology Stack

**Frontend:**
- Blazor WebAssembly (.NET 8.0)
- Bootstrap 5 + Custom CSS
- Blazored.LocalStorage
- Custom JWT Authentication

**Backend:**
- ASP.NET Core Web API (.NET 8.0)
- Entity Framework Core 8.0
- SQL Server
- JWT Bearer Authentication
- Swagger/OpenAPI

**Database:**
- SQL Server 2019+
- Entity Framework Core Migrations
- Foreign Key Constraints
- Performance Indexes

---

## 🚀 Quick Start

### Prerequisites
- .NET 8.0 SDK
- SQL Server (LocalDB or full version)
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**
```bash
git clone <repository-url>
cd BoutiqueManagementProject
```

2. **Setup Database**
```bash
cd WebApiBoutique
dotnet ef database update
```

3. **Configure Secrets (Optional)**
```bash
dotnet user-secrets init
dotnet user-secrets set "Email:SenderPassword" "your-app-password"
```

4. **Run Backend**
```bash
cd WebApiBoutique
dotnet run
```

5. **Run Frontend** (New Terminal)
```bash
cd Boutique.Client
dotnet run
```

6. **Access Application**
- Frontend: http://localhost:5000
- Backend API: http://localhost:5100
- Swagger UI: http://localhost:5100/swagger

---

## 📊 API Endpoints

### Authentication
```
POST   /api/auth/register          - Register new user
POST   /api/auth/login             - Login user
POST   /api/auth/verify-otp        - Verify OTP
POST   /api/auth/forgot-password   - Request password reset
POST   /api/auth/reset-password    - Reset password
```

### Customers
```
GET    /api/customer               - Get all customers
GET    /api/customer/{id}          - Get customer by ID
POST   /api/customer               - Create customer
PUT    /api/customer/{id}          - Update customer
DELETE /api/customer/{id}          - Delete customer
```

### Orders
```
GET    /api/order                  - Get all orders
GET    /api/order/{id}             - Get order by ID
GET    /api/order/active           - Get active orders
POST   /api/order                  - Create order
PUT    /api/order/{id}/status      - Update order status
DELETE /api/order/{id}             - Delete order
```

### Payments ✨NEW
```
GET    /api/payment                      - Get all payments
GET    /api/payment/{id}                 - Get payment by ID
GET    /api/payment/customer/{id}        - Get customer payments
GET    /api/payment/customer/{id}/summary - Get payment summary
POST   /api/payment                      - Create payment
PUT    /api/payment/{id}                 - Update payment
DELETE /api/payment/{id}                 - Delete payment
```

### Measurements
```
GET    /api/measurement/customer/{id}  - Get customer measurements
GET    /api/measurement/statistics     - Get measurement statistics ✨NEW
GET    /api/measurement/recent         - Get recent measurements ✨NEW
POST   /api/measurement                - Create measurement
POST   /api/measurement/validate       - Validate measurement
PUT    /api/measurement/{id}           - Update measurement
DELETE /api/measurement/{id}           - Delete measurement
```

---

## 🔒 Security

### Development
- User secrets for sensitive data
- Local database with integrated security
- CORS allows localhost

### Production
- Environment variables for configuration
- Restricted CORS origins
- HTTPS enforcement
- Secure password storage
- JWT token expiration

---

## 🛠️ Development

### Project Structure
```
BoutiqueManagementProject/
├── WebApiBoutique/              # Backend API
│   ├── Controllers/             # API endpoints
│   ├── Services/                # Business logic
│   ├── Models/                  # Database entities
│   ├── DTOs/                    # Data transfer objects
│   ├── Data/                    # Database context
│   ├── Middleware/              # Custom middleware
│   └── Migrations/              # EF Core migrations
│
├── Boutique.Client/             # Frontend Blazor
│   ├── Pages/                   # Razor pages
│   ├── Components/              # Reusable components
│   ├── Services/                # API services
│   ├── Models/                  # Client models
│   └── wwwroot/                 # Static files
│
└── Documentation/               # Guides and docs
```

---

## 📄 License

This project is licensed under the MIT License.