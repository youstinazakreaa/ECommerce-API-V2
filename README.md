# E-Commerce API V2

A RESTful E-Commerce Web API built with **ASP.NET Core 8**, following **Onion Architecture** principles to provide a maintainable, scalable, and well-structured backend application.

The project demonstrates backend development concepts including authentication, product management, shopping baskets, order processing, payment integration, database management, and unit testing.

## Tech Stack

- **Framework:** ASP.NET Core 8 Web API
- **Language:** C#
- **Database:** SQL Server
- **ORM:** Entity Framework Core
- **Architecture:** Onion Architecture
- **Authentication:** ASP.NET Core Identity and JWT
- **API Documentation:** Swagger / OpenAPI
- **Caching / Basket Storage:** Redis
- **Testing:** xUnit
- **Version Control:** Git and GitHub

## Key Features

### Authentication and Authorization

- User registration and login
- JWT-based authentication
- ASP.NET Core Identity integration
- Role-based authorization
- User address management

### Product Management

- Retrieve all products and individual product details
- Filter products by brand and type
- Search products by name
- Sort products
- Pagination
- Product stock quantity management
- Product images

### Shopping Basket

- Customer basket management
- Redis-based basket storage
- Add, update, and remove basket items

### Order Management

- Create orders from customer baskets
- Manage order items and shipping addresses
- Delivery method selection
- Delivery cost calculation
- Order total calculation
- Order processing and order history

### Payment Integration

- Payment service implementation
- Payment configuration management

### Email Service

- Email service implementation
- Configurable email settings

### Error Handling

- Centralized exception-handling middleware
- Consistent API error responses
- Model validation error handling

### Unit Testing

- Separate xUnit test project
- Order service unit tests
- Tests for selected order-processing scenarios

## Project Architecture

The solution follows Onion Architecture to separate business logic, data access, infrastructure, and API responsibilities.

```text
ECommerce-API-V2/
│
├── onine.core/
│   ├── Entities
│   ├── DTOs
│   ├── Enums
│   └── Repository Interfaces
│
├── onine.Repository/
│   ├── DbContexts
│   ├── Entity Configurations
│   ├── Migrations
│   ├── Repositories
│   └── Identity
│
├── onine.services/
│   ├── Order Services
│   ├── Payment Service
│   ├── Email Service
│   └── Service Interfaces
│
├── onineproject/
│   ├── Controllers
│   ├── Middleware
│   ├── Extensions
│   └── API Configuration
│
└── onineproject.Tests/
    └── Order Service Tests
```

## Design Patterns and Concepts

- Onion Architecture
- Repository Pattern
- Generic Repository
- Specification Pattern
- Dependency Injection
- Separation of Concerns
- DTOs and AutoMapper
- Asynchronous Programming
- Entity Framework Core Migrations
- RESTful API Design

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server
- Redis
- Visual Studio 2022 or another compatible IDE

### Installation

**1. Clone the repository**

```bash
git clone https://github.com/youstinazakreaa/ECommerce-API-V2.git
```

**2. Navigate to the project**

```bash
cd ECommerce-API-V2
```

**3. Configure the application**

Set up the required database connection strings, JWT settings, and any applicable email or payment credentials using .NET User Secrets or environment variables.

Do not commit sensitive credentials to GitHub.

**4. Apply database migrations**

Configure the appropriate startup and database projects, then apply the EF Core migrations for the application and Identity databases.

**5. Run the API**

```bash
dotnet run --project onineproject
```

**6. Explore the endpoints**

Open the Swagger UI at the HTTPS or HTTP address displayed by the running application, followed by `/swagger`.

## Running Unit Tests

Run the test project using:

```bash
dotnet test onineproject.Tests/onineproject.Tests.csproj
```

## Future Improvements

- Expand automated unit and integration testing
- Add CI/CD pipelines using GitHub Actions
- Add Docker support
- Improve API monitoring and logging
- Expand payment and order-processing scenarios

## Author

**Youstina Zakrea**

.NET Backend Developer

- [LinkedIn](https://www.linkedin.com/in/youstina-zakrea-2244672b9)
- [GitHub](https://github.com/youstinazakreaa)

## Repository

[View E-Commerce API V2 on GitHub](https://github.com/youstinazakreaa/ECommerce-API-V2)
