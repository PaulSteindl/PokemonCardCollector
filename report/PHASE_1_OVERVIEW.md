# Phase 1: Project Setup & Infrastructure - Completion Overview

**Date Completed**: November 29, 2025  
**Duration**: Project Foundation  
**Status**: ✅ Complete

---

## Executive Summary

Phase 1 established the foundational infrastructure for the Pokémon Card Collector application. This phase focused on database design, Entity Framework Core configuration, and dependency injection setup—critical components that all subsequent phases depend on.

### Key Accomplishments

- ✅ Designed and implemented domain models using inheritance patterns
- ✅ Configured Entity Framework Core with SQL Server provider
- ✅ Set up database context with Table-Per-Hierarchy (TPH) inheritance
- ✅ Configured dependency injection and connection strings
- ✅ Established database naming conventions and constraints

---

## Architectural Decisions & Rationale

### 1. **Table-Per-Hierarchy (TPH) Inheritance Pattern**

**Decision**: Use TPH inheritance with a discriminator column instead of Table-Per-Type (TPT) or Table-Per-Concrete-Type (TPC).

**Rationale**:
- **Single Table Efficiency**: All card types (PokemonCard, TrainerCard, EnergyCard) share common properties and are stored in one table
- **Query Performance**: Simpler queries without multiple joins; reduces database round-trips
- **Easier Polymorphic Queries**: Can easily query all cards or filter by card type using the discriminator column
- **Future Extensibility**: New card types can be added without schema migrations for the base table
- **Current Needs**: Since all card types share 80%+ of the same properties, TPH reduces redundancy

**Trade-off Accepted**: Different card types may have unused nullable columns, but this trade-off is acceptable given the performance benefits and query simplicity.

### 2. **Card Base Class Design**

**Decision**: Create an abstract `Card` base class with discriminator-driven polymorphism.

**Properties Included**:
- **Identification**: `Id` (database PK), `ApiId` (external API reference), `LocalId` (card number within set)
- **Display**: `Name`, `ImageUrl`, `Illustrator`
- **Classification**: `SetId`, `SetName`, `Rarity`, card type variants (Normal, Reverse, Holo, FirstEdition)
- **Pricing**: `TcgPlayerPrice` (USD), `CardmarketPrice` (EUR), `EstimatedValue` (calculated)
- **Metadata**: `Updated` (API sync timestamp), `DateAdded` (collection timestamp), `Condition`, `UserNotes`

**Why This Structure**:
- Eliminates data duplication across card types
- Supports future pricing integrations and market analysis
- Tracks when API data was last refreshed (important for price accuracy)
- Enables user annotations and condition tracking for collection management

### 3. **Connection String & Configuration**

**Setup**:
```
appsettings.json: Production/default connection to (localdb)\mssqllocaldb
appsettings.Development.json: Inherits production settings, overridable for local development
```

**Database**: `PokemonCardCollectorDb`  
**Provider**: SQL Server (localdb for development, scalable to Azure SQL in production)  
**Authentication**: Windows Trusted Connection (development); credentials in secrets for production

**Why SQL Server**:
- Entity Framework Core has excellent SQL Server support
- Native support for TPH inheritance with discriminator
- Easy migration path: localdb → Azure SQL for cloud deployment
- Strong query optimization for large card collections

### 4. **Property Constraints & Data Validation**

All string properties include `HasMaxLength()` constraints in `OnModelCreating`:
- `ApiId`: 50 chars (e.g., "swsh3-136")
- `LocalId`: 20 chars (e.g., "136")
- `Name`: 500 chars (allows long Pokémon names)
- `Rarity`: 100 chars (accommodates various rarity levels)
- `UserNotes`: 2000 chars (detailed user comments)

**Currency Precision**: `HasPrecision(10, 2)` for price fields (supports up to $9,999,999.99)

**Why This Matters**:
- Database-level validation prevents corrupt data at the source
- Consistent with best practices for defensive programming
- Enables future API validation rules that match database constraints
- Reduces risk of data truncation or type mismatches

### 5. **DateTime Handling**

**Policy**: All `DateTime` fields default to `DateTime.UtcNow`

**Why UTC**:
- Card prices fluctuate; UTC timestamps enable accurate market analysis
- Global collection means timezone-agnostic timestamps
- Simplifies future multi-user features (prevents timezone conflicts)
- Industry standard for financial/trading applications

---

## What Was Implemented

### Domain Models (`Models/` folder)

#### Card (Abstract Base Class)
- Represents the polymorphic root for all TCG cards
- Includes 20+ properties covering identification, classification, pricing, and metadata
- Fully XML-documented for IDE intellisense

#### PokemonCard (Derived Class)
- Specializes `Card` for Pokémon-type creatures
- Inherits all base properties; discriminator value: `"PokemonCard"`
- Future: May add Pokémon-specific properties (HP, attacks, weakness, retreat cost) in Phase 3

#### TrainerCard & EnergyCard (Derived Classes)
- Specialized card types for future use
- Follow same inheritance pattern for consistency
- Structure ready for Trainer/Energy-specific properties

### Database Context (`PokemonCardDbContext`)

**Features**:
- Inherits from `DbContext` with primary constructor pattern (C# 12 feature)
- Four `DbSet` properties: `Cards`, `PokemonCards`, `TrainerCards`, `EnergyCards`
- `OnModelCreating` method configures TPH inheritance with explicit discriminator mapping
- Fluent API configuration for all constraints, indexes, and relationships

**Key Configuration Details**:
- Discriminator column: `CardType` (string)
- Primary Key: `Id` (int, auto-increment)
- Indexes: ApiId (for fast duplicate checking), SetId (for set-based queries)
- Constraints: NOT NULL on ApiId, LocalId, Name, SetId, SetName

### Dependency Injection (`Program.cs`)

**Configuration**:
```csharp
builder.Services.AddDbContext<PokemonCardDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

**What This Enables**:
- Automatic DI container registration of `PokemonCardDbContext`
- Scoped lifetime (one DbContext per HTTP request; optimal for Blazor)
- Lazy initialization (DbContext created only when needed)
- Connection string from configuration (supports environment-specific values)

### Application Settings

**appsettings.json**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PokemonCardCollectorDb;Trusted_Connection=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

**Why This Configuration**:
- `(localdb)\mssqllocaldb`: Built-in SQL Server for development (no external DB install needed)
- `Trusted_Connection=true`: Uses Windows authentication (secure for development)
- Logging level "Information": Captures EF Core queries for debugging
- AllowedHosts: `"*"` (permissive for development; tighten in production)

---

## Database Schema Overview

### Cards Table (TPH Single Table)

| Column | Type | Constraints | Purpose |
|--------|------|-------------|---------|
| `Id` | INT | PK, AUTO INCREMENT | Unique database identifier |
| `CardType` | NVARCHAR(50) | NOT NULL | Discriminator: "PokemonCard", "TrainerCard", or "EnergyCard" |
| `ApiId` | NVARCHAR(50) | NOT NULL, UNIQUE INDEX | External API ID (e.g., "swsh3-136") |
| `LocalId` | NVARCHAR(20) | NOT NULL | Card number within set (e.g., "136") |
| `Name` | NVARCHAR(500) | NOT NULL | Official card name |
| `SetId` | NVARCHAR(50) | NOT NULL, INDEX | Set identifier (e.g., "swsh3") |
| `SetName` | NVARCHAR(200) | NOT NULL | Set display name (e.g., "Darkness Ablaze") |
| `ImageUrl` | NVARCHAR(500) | NULL | URL to card image |
| `Illustrator` | NVARCHAR(200) | NULL | Artist name |
| `Rarity` | NVARCHAR(100) | NULL | Card rarity level |
| `Condition` | NVARCHAR(50) | NULL | User's card condition assessment |
| `TcgPlayerPrice` | DECIMAL(10,2) | NULL | Market price in USD |
| `CardmarketPrice` | DECIMAL(10,2) | NULL | Market price in EUR |
| `EstimatedValue` | DECIMAL(10,2) | NULL | Calculated user collection value |
| `VariantNormal` | BIT | DEFAULT 0 | Normal (non-foil) available? |
| `VariantReverse` | BIT | DEFAULT 0 | Reverse holofoil available? |
| `VariantHolo` | BIT | DEFAULT 0 | Regular holofoil available? |
| `VariantFirstEdition` | BIT | DEFAULT 0 | First edition available? |
| `Updated` | DATETIME2 | DEFAULT GETUTCDATE() | Last API sync timestamp |
| `DateAdded` | DATETIME2 | DEFAULT GETUTCDATE() | Collection addition date |
| `UserNotes` | NVARCHAR(MAX) | NULL | User annotations |

**Index Strategy**:
- `ApiId`: Prevents duplicate API imports
- `SetId`: Enables fast filtering by card set
- `DateAdded`: Supports "Recently Added" queries
- Composite index planned for Phase 2: `(SetId, LocalId)` for fast card lookups

---

## Design Patterns & Best Practices Applied

### 1. **Entity Framework Core Fluent API**
- Explicit configuration of all constraints in `OnModelCreating`
- Data annotations avoided in favor of Fluent API (keeps models cleaner)
- Discriminator mapping explicit and easily understood

### 2. **Inheritance with Polymorphism**
- Abstract base class cannot be instantiated directly
- Derived types (`PokemonCard`, `TrainerCard`, `EnergyCard`) are concrete
- Enables future features like "Get all cards of type X"

### 3. **Scoped DbContext Lifetime**
- New context per HTTP request (Blazor server-side rendering)
- Prevents context reuse across requests (thread-safe)
- Automatic disposal after request completes

### 4. **Configuration Management**
- Environment-specific `appsettings.*.json` files
- Connection string in configuration (not hardcoded)
- Supports seamless transition: development → staging → production

### 5. **XML Documentation**
- All classes and properties have `/// <summary>` comments
- Enables IDE intellisense for developers
- Generates API documentation automatically
- Critical for team onboarding and long-term maintainability

---

## What Happens Next: Phase 2 Readiness

Phase 1 provides the **data access foundation** for Phase 2 (Data Access Layer). The completed infrastructure enables:

1. **Repository Pattern Implementation**: 
   - `ICardRepository` interface will abstract database queries
   - `CardRepository` will use injected `PokemonCardDbContext`

2. **CRUD Operations**:
   - Add, read, update, delete operations on `PokemonCard` entities
   - Polymorphic queries across all card types

3. **Database Migrations**:
   - EF Core Migrations will script schema changes
   - Command: `dotnet ef migrations add InitialCreate`
   - Enables versioning and rollback of schema changes

4. **Query Optimization**:
   - `.AsNoTracking()` for read-only queries
   - Pagination support for large result sets
   - Indexes support filtering by ApiId, SetId, DateAdded

---

## Key Files Created/Modified

| File | Purpose | Status |
|------|---------|--------|
| `Models/PokemonCard.cs` | Domain entity (PokemonCard type) | ✅ Complete |
| `Models/Card.cs` | Abstract base class (TPH inheritance) | ✅ Complete |
| `Models/TrainerCard.cs` | Domain entity (TrainerCard type) | ✅ Complete |
| `Models/EnergyCard.cs` | Domain entity (EnergyCard type) | ✅ Complete |
| `Models/PokemonCardDbContext.cs` | Entity Framework context | ✅ Complete |
| `Program.cs` | DI configuration for DbContext | ✅ Complete |
| `appsettings.json` | Production connection string | ✅ Complete |
| `appsettings.Development.json` | Development logging config | ✅ Complete |

---

## Technical Decisions Reference

### Why Not Entity Framework Core Code-First with Migrations Yet?

**Decision**: Database structure is configured but migrations not yet executed.

**Reasoning**:
- Phase 1 focuses on domain model design, not database scripts
- Phase 2 will execute: `dotnet ef migrations add InitialCreate`
- Keeps phases logically separated (design → access → implementation)
- Allows review of migration scripts before applying to database

### Why Not Include Repository Pattern in Phase 1?

**Decision**: Repository abstraction deferred to Phase 2.

**Reasoning**:
- Phase 1 = "Setup & Infrastructure" (tools and configuration)
- Phase 2 = "Data Access Layer" (patterns and interfaces)
- Keeps concerns separated and phases independently valuable
- DbContext is registered in DI; ready for repository injection

---

## Lessons Learned & Design Insights

### TPH vs. TPT Trade-offs

**Question**: Why not Table-Per-Type (TPT)?

**Answer**: 
- TPT would create separate tables: `Cards`, `PokemonCards`, `TrainerCards`
- Every query would require SQL JOINs across tables
- Our card types are 80%+ homogeneous (most properties are shared)
- TPH performance is superior for read-heavy workloads
- Acceptable trade-off: A few unused nullable columns in some rows

### Why Decimal for Prices, Not Double?

**Answer**:
- Currency demands precision; double has floating-point errors
- `decimal(10, 2)` guarantees exact $0.01 accuracy
- Prevents rounding errors that compound in financial systems

### Why DateTime.UtcNow, Not Local Time?

**Answer**:
- Card collectors worldwide; UTC prevents timezone confusion
- Price APIs (TCGPlayer, Cardmarket) use UTC
- Simplifies future multi-region features
- Standard practice for all trading/financial systems

---

## Success Criteria Met ✅

- ✅ Entity models created (`Card`, `PokemonCard`, `TrainerCard`, `EnergyCard`)
- ✅ Database context configured with TPH inheritance
- ✅ Dependency injection set up in `Program.cs`
- ✅ Connection strings in `appsettings.json` and `appsettings.Development.json`
- ✅ Constraints and indexes designed for data integrity and query performance
- ✅ Code fully documented with XML comments
- ✅ Foundation ready for Phase 2 (Repository Pattern)

---

## Conclusion

Phase 1 successfully established a **robust, extensible data foundation** using industry-standard patterns and best practices. The Table-Per-Hierarchy inheritance design balances performance with flexibility, while the Entity Framework configuration is production-ready and easy to understand.

The architecture is positioned for Phase 2's Repository Pattern implementation, which will abstract database access and enable clean separation of concerns throughout the application.

---

## Phase 2 Readiness Assessment ✅

### Infrastructure Checklist

| Item | Status | Notes |
|------|--------|-------|
| Entity Framework Core NuGet packages | ✅ Installed | All required packages present |
| Domain models (`Card`, `PokemonCard`, `TrainerCard`, `EnergyCard`) | ✅ Complete | Fully implemented with XML docs |
| Database context (`PokemonCardDbContext`) | ✅ Complete | TPH inheritance configured |
| DI configuration in Program.cs | ✅ Complete | DbContext registered as Scoped |
| Connection strings configured | ✅ Complete | appsettings.json and Development.json |
| Database constraints & indexes | ✅ Designed | Fluent API configuration complete |
| Migrations created | ⏳ Pending | Next step: `dotnet ef migrations add InitialCreate` |
| Database applied | ⏳ Pending | Next step: `dotnet ef database update` |

### What's Ready for Phase 2

✅ **Strong Foundation**:
- Domain models are well-designed with inheritance patterns
- Database context is properly configured with constraints
- DI is set up for easy repository injection
- All configuration is in place and production-ready

✅ **What Phase 2 Will Build On**:
1. **Repository Pattern**: Will wrap `PokemonCardDbContext` queries
2. **CRUD Operations**: All LINQ patterns support the planned repo methods
3. **Query Optimization**: Indexes and constraints are designed for Phase 2 queries
4. **Async/Await**: DbContext is ready for async repository methods

---

**Next Step**: Execute migrations, then proceed to Phase 2 - Data Access Layer (Repository Pattern Implementation)

---

<small>
Generated with GitHub Copilot as directed by PaulSteindl

*This document provides the strategic thinking and design rationale for Phase 1. Refer to code comments and the entity guide for implementation details.*
</small>
