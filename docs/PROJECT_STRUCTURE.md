# 🏗️ Project Structure

## Overview
Professional enterprise-level structure following Clean Architecture principles.

## Folder Structure

```
Boutique-Management/
├── src/                          # Source code
│   ├── Boutique.Client/          # Blazor WebAssembly Frontend
│   └── WebApiBoutique/           # ASP.NET Core Web API
├── tests/                        # Test projects
│   ├── WebApiBoutique.Tests/     # API unit tests
│   └── Boutique.Client.Tests/    # Client unit tests
├── docs/                         # Documentation
├── scripts/                      # Build/deployment scripts
├── tools/                        # Development tools
└── README.md                     # Project overview
```

## Backend Structure (WebApiBoutique)

```
WebApiBoutique/
├── Controllers/                  # API endpoints
├── Services/                     # Business logic
│   └── Interface/               # Service contracts
├── Repository/                   # Data access layer
│   └── Interface/               # Repository contracts
├── Models/                       # Domain models
│   └── DTOs/                    # Data transfer objects
│       ├── Request/             # Input DTOs
│       ├── Response/            # Output DTOs
│       └── Common/              # Shared DTOs
├── Core/                        # Business domain layer
│   ├── Entities/                # Domain entities
│   ├── Interfaces/              # Core contracts
│   └── Services/                # Domain services
├── Infrastructure/              # External dependencies
│   ├── Data/                    # Database context
│   ├── External/                # Third-party services
│   └── Persistence/             # Data persistence
├── Auth/                        # Authentication logic
├── Middleware/                  # Custom middleware
├── Attributes/                  # Custom attributes
└── Migrations/                  # Database migrations
```

## Frontend Structure (Boutique.Client)

```
Boutique.Client/
├── Pages/                       # Razor pages/components
├── Components/                  # Reusable UI components
├── Layout/                      # Layout components
├── Services/                    # API client services
├── Models/                      # Client-side models
│   └── DTOs/                   # Client DTOs
├── Shared/                      # Shared components
└── wwwroot/                     # Static assets
```

## Architecture Principles

- **Separation of Concerns**: Clear layer separation
- **Dependency Injection**: Loose coupling
- **Clean Architecture**: Domain-centric design
- **SOLID Principles**: Maintainable code
- **Repository Pattern**: Data access abstraction
- **Service Layer**: Business logic encapsulation