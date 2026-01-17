# Airline Service API

A .NET 6.0 Web API for managing airline flight statuses. Built with Clean Architecture, CQRS pattern, and modern best practices.

## Features

- **Clean Architecture** - Domain, Application, Infrastructure, WebApi layers
- **CQRS** - Command Query Responsibility Segregation with MediatR
- **JWT Authentication** - Secure token-based authentication with BCrypt password hashing
- **Role-based Authorization** - Policy-based access control (Moderator)
- **EF Core** - Code-first approach with MSSQL
- **Caching** - In-memory caching with automatic invalidation
- **Validation** - FluentValidation with automatic pipeline validation
- **Logging** - Serilog with daily rolling file logs and audit trail
- **API Documentation** - Swagger/OpenAPI (Development only)
- **Unit Tests** - xUnit with Moq

## Prerequisites

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/sql-server) (Express or higher)

## Getting Started

### 1. Clone the repository

```bash
git clone <repository-url>
cd AirlineServiceApi
```

### 2. Update connection string

Edit `AirlineService.WebApi/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=AirlineServiceDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### 3. Run the application

```bash
cd AirlineServiceApi
dotnet run --project AirlineService.WebApi
```

The API will start at:
- HTTPS: https://localhost:7001
- HTTP: http://localhost:5001

### 4. Access Swagger UI (Development only)

Open https://localhost:7001/swagger in your browser.

## Project Structure

```
AirlineServiceApi/
├── AirlineService.Domain/          # Entities, Enums (no dependencies)
│   ├── Entities/
│   │   ├── Flight.cs
│   │   ├── User.cs
│   │   └── Role.cs
│   └── Enums/
│       └── FlightStatus.cs
│
├── AirlineService.Application/     # Business logic, CQRS, Interfaces
│   ├── Common/
│   │   ├── Behaviors/              # MediatR pipeline behaviors
│   │   ├── Interfaces/             # Abstractions (IApplicationDbContext, etc.)
│   │   └── Models/                 # Shared models (PaginatedList)
│   ├── Auth/
│   │   ├── Commands/               # LoginCommand
│   │   └── DTOs/                   # LoginRequest, TokenResponse
│   └── Flights/
│       ├── Commands/               # CreateFlight, UpdateFlightStatus
│       ├── Queries/                # GetFlights
│       └── DTOs/                   # FlightDto, CreateFlightRequest
│
├── AirlineService.Infrastructure/  # External concerns (EF Core, Services)
│   ├── Data/
│   │   ├── ApplicationDbContext.cs
│   │   ├── ApplicationDbContextInitializer.cs
│   │   └── Configurations/         # EF Core entity configurations
│   └── Services/
│       ├── AuthService.cs
│       ├── PasswordHasher.cs
│       └── MemoryCacheService.cs
│
├── AirlineService.WebApi/          # Presentation layer
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   └── FlightsController.cs
│   ├── Filters/                    # Exception filters
│   ├── Middleware/                 # Exception handling middleware
│   └── Services/                   # CurrentUserService
│
└── AirlineService.Tests/           # Unit tests
    ├── Auth/
    ├── Flights/
    └── Helpers/
```

## API Endpoints

### Authentication

#### POST /api/auth/token
Get JWT token for authentication.

**Request:**
```json
{
  "username": "moderator",
  "password": "moderator123"
}
```

**Response (200):**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "tokenType": "Bearer",
  "username": "moderator",
  "role": "Moderator"
}
```

### Flights

All flight endpoints require authentication. Include the JWT token in the Authorization header:
```
Authorization: Bearer <your-token>
```

#### GET /api/flights
Get paginated list of flights. Available to all authenticated users.

**Query Parameters:**
| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| origin | string | - | Filter by origin (optional) |
| destination | string | - | Filter by destination (optional) |
| pageNumber | int | 1 | Page number |
| pageSize | int | 10 | Items per page |

**Response (200):**
```json
{
  "items": [
    {
      "id": 1,
      "origin": "Almaty",
      "destination": "Astana",
      "departure": "2026-01-20T08:00:00+06:00",
      "arrival": "2026-01-20T09:30:00+06:00",
      "status": 0,
      "statusName": "InTime"
    }
  ],
  "pageNumber": 1,
  "totalPages": 1,
  "totalCount": 5,
  "pageSize": 10,
  "hasPreviousPage": false,
  "hasNextPage": false
}
```

#### POST /api/flights
Create a new flight. **Requires Moderator role.**

**Request:**
```json
{
  "origin": "Almaty",
  "destination": "Shymkent",
  "departure": "2026-01-25T08:00:00+06:00",
  "arrival": "2026-01-25T09:30:00+06:00",
  "status": 0
}
```

**Response (201):**
```json
{
  "id": 6,
  "origin": "Almaty",
  "destination": "Shymkent",
  "departure": "2026-01-25T08:00:00+06:00",
  "arrival": "2026-01-25T09:30:00+06:00",
  "status": 0,
  "statusName": "InTime"
}
```

#### PUT /api/flights/{id}/status
Update flight status. **Requires Moderator role.**

**Request:**
```json
{
  "status": 1
}
```

**Response:** 204 No Content

### Flight Status Values
| Value | Name |
|-------|------|
| 0 | InTime |
| 1 | Delayed |
| 2 | Cancelled |

## Test Users

| Username | Password | Role |
|----------|----------|------|
| moderator | moderator123 | Moderator |

## Running Tests

```bash
dotnet test
```

## Logs

Logs are written to the `logs/` folder with daily rolling files:
- Pattern: `airline-service-YYYY-MM-DD.log`
- Location: `AirlineService.WebApi/logs/`

### Log Format
```
{Timestamp} [{Level}] {Message}
```

### Audit Events Logged
- Login success/failure
- Flight creation
- Flight status updates
- Unauthorized/Forbidden access attempts
- Validation errors
- Internal server errors

## Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=AirlineServiceDb;..."
  },
  "JwtSettings": {
    "Secret": "YourSuperSecretKeyForJwtTokenGeneration...",
    "Issuer": "AirlineServiceApi",
    "Audience": "AirlineServiceApi",
    "ExpirationHours": 1
  }
}
```

## Local Application run
<img width="600" height="400" alt="image" src="https://github.com/user-attachments/assets/90404709-db69-4072-b40f-8e840986468b" />

<img width="650" height="400" alt="image" src="https://github.com/user-attachments/assets/2e97016d-8091-4265-9df2-1c73bd2979e5" />

<img width="580" height="400" alt="image" src="https://github.com/user-attachments/assets/9ae02a90-6c95-45d1-9f35-a644f481bc98" />


<img width="580" height="400" alt="image" src="https://github.com/user-attachments/assets/3c8d5e89-7093-4ec4-b499-14bb4ecf58f4" />





