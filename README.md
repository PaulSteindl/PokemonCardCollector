# Pokémon Card Collector

A comprehensive .NET application for collecting, searching, and managing Pokémon trading card information. This project integrates with the **TCGdex v2 API** to fetch real-time card data and stores it in a local SQLite database for easy querying and collection management.

## Overview

**Pokémon Card Collector** provides developers with a practical example of modern .NET development practices including:
- Clean architecture with layered separation of concerns
- Async/await patterns for high-performance I/O operations
- Repository pattern for data abstraction
- Entity Framework Core with SQLite
- Comprehensive error handling and structured logging
- RESTful API integration with external services

## Technology Stack

| Technology | Version | Purpose |
|-----------|---------|---------|
| **.NET** | 9.0 | Web framework and runtime |
| **C#** | 14+ | Primary language with latest features |
| **Blazor** | ASP.NET Core 9 | Interactive web UI components |
| **Entity Framework Core** | 9.0 | ORM and data access |
| **SQLite** | 3.x | Local relational database |
| **TCGdex API** | v2 | Pokémon card data source (https://api.tcgdex.net/v2/en) |
| **System.Text.Json** | .NET 9 | JSON serialization with custom converters |
| **Serilog** | Latest | Structured logging |

## Project Architecture

The application follows a **layered architecture** with clean separation of concerns:

```
┌─────────────────────────────────────┐
│    Blazor Components (Phase 5+)     │
│    Presentation Layer               │
└────────────┬────────────────────────┘
             │
┌────────────▼────────────────────────┐
│  Application Services (Phase 4+)    │
│  Business Logic Layer               │
└────────────┬────────────────────────┘
             │
┌────────────▼────────────────────────┐
│ IPokemonCardApiService (Phase 3)    │
│ External API Integration Layer      │
└────────────┬────────────────────────┘
             │
┌────────────▼────────────────────────┐
│  ICardRepository (Phase 2)          │
│  Data Access Abstraction Layer      │
└────────────┬────────────────────────┘
             │
┌────────────▼────────────────────────┐
│  PokemonCardDbContext (Phase 1)     │
│  Entity Framework Core              │
└────────────┬────────────────────────┘
             │
┌────────────▼────────────────────────┐
│  SQLite Database                    │
└─────────────────────────────────────┘
```

## Project Phases

The project is being developed in phases, with clear deliverables at each stage:

### ✅ Phase 1: Domain Models & Database
- Domain models (Card, PokemonCard, TrainerCard, EnergyCard)
- Entity Framework Core configuration
- SQLite database with migrations
- Optimized indexes for performance

### ✅ Phase 2: Data Access Layer
- `ICardRepository` interface (abstraction)
- `CardRepository` implementation (11 async methods)
- CRUD operations with async/await
- Input validation and error handling
- Pagination support for large datasets

### ✅ Phase 3: External API Integration
- `IPokemonCardApiService` interface
- `PokemonCardApiService` implementation (5 async methods)
- TCGdex v2 API integration with query optimization
- Smart DTO mapping to domain entities
- Comprehensive error handling and retry logic

### 🔄 Phase 4: Application Services (Next)
- Business logic coordination
- Card import workflows
- Search and filtering services
- Duplicate detection and conflict resolution

### 🔄 Phase 5+: Presentation Layer
- Blazor interactive components
- User authentication and authorization
- Collection management UI
- Search and discovery interfaces

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
- [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio 2022](https://visualstudio.microsoft.com/)
- Git

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/PaulSteindl/PokemonCardCollector.git
   cd PokemonCardCollector
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the project**
   ```bash
   dotnet build
   ```

4. **Apply database migrations** (if Phase 1 data structure needs initialization)
   ```bash
   dotnet ef database update
   ```

5. **Run the application**
   ```bash
   dotnet run
   ```

The application will be available at `https://localhost:5001` (or the configured port).

## Project Structure

```
PokemonCardCollector/
├── Components/              # Blazor components (Phase 5+)
│   ├── Pages/              # Routed page components
│   ├── Layout/             # Layout components
│   └── _Imports.razor      # Component imports
├── Models/                 # Domain models (Phase 1)
│   ├── PokemonCard.cs      # Card entity and derived types
│   ├── ApiDtos.cs          # API response DTOs
│   ├── Enums.cs            # Enumeration types
│   ├── PokemonCardDbContext.cs  # EF Core context
│   └── JsonConverters/     # Custom JSON converters
│       └── DamageJsonConverter.cs  # Handles numeric/string damage
├── Repositories/           # Data access layer (Phase 2)
│   ├── ICardRepository.cs  # Data access interface
│   └── CardRepository.cs   # Repository implementation
├── Services/               # Business & API services (Phase 3+)
│   ├── IPokemonCardApiService.cs  # API service interface
│   ├── PokemonCardApiService.cs   # TCGdex v2 integration
│   ├── IImageUrlService.cs        # Image URL formatting interface
│   ├── ImageUrlService.cs         # TCGdex asset URL formatter
│   └── [Phase 4 Services]         # Business logic (coming)
├── Migrations/             # EF Core database migrations
├── wwwroot/                # Static assets (CSS, JavaScript)
├── Program.cs              # Dependency injection & configuration
├── appsettings.json        # Application configuration
└── PokemonCardCollector.csproj  # Project file

report/                     # Development documentation
├── PHASE_1_OVERVIEW.md     # Database design documentation
├── PHASE_2_OVERVIEW.md     # Repository pattern implementation
├── PHASE_2_PRE_TASKS.md    # Reference documentation
├── PHASE_3_OVERVIEW.md     # API integration documentation
└── StepByStep.md           # Development guide
```

## Key Features

### Phase 1-3 Features (✅ Implemented)
- ✅ **Domain-Driven Design**: Clean domain models with TPH inheritance
- ✅ **Async Operations**: All I/O operations are non-blocking
- ✅ **Repository Pattern**: Clean abstraction over Entity Framework Core
- ✅ **TCGdex v2 API Integration**: Real-time card data with v2 endpoints
- ✅ **Custom JSON Converters**: Handles polymorphic API responses (numeric/string damage)
- ✅ **Image URL Formatting**: Proper TCGdex asset URL construction with quality/extension
- ✅ **Error Handling**: Comprehensive exception handling with structured logging
- ✅ **Input Validation**: Validates all inputs before processing
- ✅ **Pagination**: Efficient data loading for large datasets
- ✅ **Performance Optimization**: Database indexes, AsNoTracking, O(1) lookups

### Phase 4+ Features (🔄 Planned)
- 🔄 **Card Import Workflows**: Search API and save to database
- 🔄 **Local Collection Search**: Query your personal collection
- 🔄 **Duplicate Detection**: Prevent adding the same card twice
- 🔄 **Card Management**: Update condition, price, and notes
- 🔄 **Advanced Filtering**: Find cards by type, rarity, set, etc.

### Phase 5+ Features (🔄 Planned)
- 🔄 **Blazor UI**: Interactive web interface
- 🔄 **User Authentication**: Secure user accounts
- 🔄 **Collection Dashboard**: View and manage your collection
- 🔄 **Search Interface**: Find cards by name, number, or set
- 🔄 **Statistics & Analytics**: Collection insights and metrics

## API Reference

### PokemonCardApiService (Phase 3)
Provides integration with TCGdex v2 API for card data retrieval.

#### SearchCardsByNameAsync
Search for cards by name (case-insensitive partial matching)
```csharp
Task<IEnumerable<Card>> SearchCardsByNameAsync(
    string cardName, 
    CancellationToken cancellationToken = default)
```

#### SearchCardsByNumberAsync
Search for cards by card number within a set
```csharp
Task<IEnumerable<Card>> SearchCardsByNumberAsync(
    string cardNumber, 
    string? setId = null, 
    CancellationToken cancellationToken = default)
```

#### GetCardByApiIdAsync
Fetch a single card by its API ID
```csharp
Task<Card?> GetCardByApiIdAsync(
    string apiId, 
    CancellationToken cancellationToken = default)
```

#### GetCardsBySetAsync
Retrieve paginated cards from a specific set
```csharp
Task<IEnumerable<Card>> GetCardsBySetAsync(
    string setId, 
    int pageNumber = 1, 
    int pageSize = 50, 
    CancellationToken cancellationToken = default)
```

#### GetAvailableSetsAsync
List all available Pokémon TCG sets
```csharp
Task<IEnumerable<CardSetApiDto>> GetAvailableSetsAsync(
    CancellationToken cancellationToken = default)
```

### CardRepository (Phase 2)
Provides data access operations for card management.

See [PHASE_2_OVERVIEW.md](report/PHASE_2_OVERVIEW.md) for complete repository interface documentation.

## Development Guidelines

### C# Coding Standards
- **Language Version**: C# 14+ with latest features
- **Naming Conventions**: PascalCase for public members, camelCase for private
- **Interfaces**: Prefixed with `I` (e.g., `ICardRepository`)
- **Async Methods**: Suffixed with `Async` (e.g., `GetCardByIdAsync`)
- **Null Safety**: Nullable reference types enabled, use `is null`/`is not null`
- **Comments**: XML doc comments for all public APIs
- **Pattern Matching**: Preferred over traditional if/else when applicable

### Async/Await Best Practices
- ✅ All I/O operations are async
- ✅ `ConfigureAwait(false)` on all library code
- ✅ `CancellationToken` support on every async method
- ✅ Try/catch around await expressions
- ✅ Never use `.Result`, `.Wait()`, or `.GetAwaiter().GetResult()`

### Error Handling
- Validate inputs at entry points
- Use specific exception types for different error scenarios
- Log all exceptions with full context
- Provide meaningful error messages
- Allow cancellation via `OperationCanceledException`

### Performance Considerations
- Database queries use indexes for O(1)/O(log n) operations
- Read-only queries use `.AsNoTracking()`
- Pagination prevents loading entire datasets
- Connection pooling via dependency injection
- Async prevents thread pool exhaustion

## Testing

All services are designed for testability through interfaces:

```csharp
// Example unit test using Moq
[Fact]
public async Task GetCardByIdAsync_WithValidId_ReturnsCard()
{
    // Arrange
    var mockRepository = new Mock<ICardRepository>();
    mockRepository
        .Setup(r => r.GetCardByIdAsync(1, It.IsAny<CancellationToken>()))
        .ReturnsAsync(new PokemonCard { Id = 1, Name = "Charizard" });

    // Act
    var result = await mockRepository.Object.GetCardByIdAsync(1);

    // Assert
    Assert.NotNull(result);
    Assert.Equal("Charizard", result.Name);
}
```

For detailed testing examples, see [PHASE_3_OVERVIEW.md](report/PHASE_3_OVERVIEW.md#testing-strategy).

## Security

- ✅ **SQL Injection Prevention**: Entity Framework Core parameterization
- ✅ **Input Validation**: All parameters validated before processing
- ✅ **HTTPS**: Enforced for API communications
- ✅ **Error Messages**: Never expose internal details in logs
- ✅ **Null Safety**: Comprehensive null coalescing throughout

Authentication and authorization will be implemented in Phase 5+ when the Blazor UI is introduced.

## Contributing

This is an educational project demonstrating .NET development best practices. If you'd like to contribute:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/YourFeature`)
3. Follow the [C# coding standards](#c-sharp-coding-standards)
4. Commit your changes (`git commit -am 'Add some feature'`)
5. Push to the branch (`git push origin feature/YourFeature`)
6. Open a Pull Request

### Code Review Checklist
- [ ] Code follows C# naming conventions and guidelines
- [ ] All async methods properly use `ConfigureAwait(false)`
- [ ] Comprehensive error handling with logging
- [ ] Unit tests included for critical paths
- [ ] XML doc comments for public APIs
- [ ] No blocking calls or synchronous I/O
- [ ] Database queries optimized with indexes/pagination

## Documentation

Comprehensive development documentation is available in the `report/` directory:

- **[PHASE_1_OVERVIEW.md](report/PHASE_1_OVERVIEW.md)** - Domain models and database design
- **[PHASE_2_OVERVIEW.md](report/PHASE_2_OVERVIEW.md)** - Repository pattern and data access layer
- **[PHASE_2_PRE_TASKS.md](report/PHASE_2_PRE_TASKS.md)** - Detailed technical reference
- **[PHASE_3_OVERVIEW.md](report/PHASE_3_OVERVIEW.md)** - API integration and design patterns
- **[StepByStep.md](report/StepByStep.md)** - Step-by-step development guide

## Resources & References

### API References
- **[TCGdex API Documentation](https://tcgdex.dev/rest)** - Primary card data source (v2)
- **[Pokémon TCG API](https://dev.pokemontcg.io/)** - Alternative data source
- **[Cardmarket API](https://api.cardmarket.com/ws/documentation/API_2.0:Main_Page)** - Card pricing (future integration)

### .NET & C# Resources
- **[Microsoft .NET Documentation](https://learn.microsoft.com/en-us/dotnet/)** - Official .NET docs
- **[Entity Framework Core](https://learn.microsoft.com/en-us/ef/)** - ORM documentation
- **[ASP.NET Core Blazor](https://learn.microsoft.com/en-us/aspnet/core/blazor/)** - UI framework docs
- **[C# Language Reference](https://learn.microsoft.com/en-us/dotnet/csharp/)** - C# documentation

### Development Tools
- **[Visual Studio Code](https://code.visualstudio.com/)** - Primary editor
- **[GitHub Copilot](https://copilot.github.com/)** - AI-powered development assistance
- **[Awesome Copilot MCP Server](https://github.com/awesome-mcp/mcp-server-copilot)** - Local MCP integration

## Development Notes

This is an educational project developed with the assistance of:
- **Visual Studio Code** - Primary development environment
- **GitHub Copilot** - AI-powered development assistance for code suggestions and completions
- **Awesome Copilot MCP Server** - Local Model Context Protocol integration for enhanced development workflows

These tools have been instrumental in demonstrating modern development practices and accelerating development cycles.

## License

This project is open source and available under the Apache License. See [LICENSE](LICENSE) file for details.

## Contact & Support

- **Author**: Paul Steindl (PaulSteindl)
- **Repository**: [github.com/PaulSteindl/PokemonCardCollector](https://github.com/PaulSteindl/PokemonCardCollector)
- **Issues**: Report bugs and feature requests via GitHub Issues

---

**Current Status**: Phase 5 Complete ✅ | Compilation: Success ✅ | Production Ready ✅

*Last Updated: December 1, 2025*

