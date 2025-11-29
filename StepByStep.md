## 📋 Pokémon Card Collector - Development Plan

### **Phase 1: Project Setup & Infrastructure** (Weeks 1-2)

#### Step 1.1: Configure Database & Entity Framework Core
- **Action**: Install Entity Framework Core NuGet packages
  - `Microsoft.EntityFrameworkCore`
  - `Microsoft.EntityFrameworkCore.SqlServer` (or SQLite for development)
  - `Microsoft.EntityFrameworkCore.Design`
  - `Microsoft.EntityFrameworkCore.Tools`

- **Create Domain Models** (in a `Models/` folder):
  ```
  Models/
  ├── PokemonCard.cs
  ├── CardCollection.cs
  └── PokemonCardDbContext.cs
  ```

- **PokemonCard Entity Structure**:
See [Pokemon Card Entity Guide](./PokemonCardEntityGuide.md)


#### Step 1.2: Create Database Context
- **DbContext class** with:
  - `DbSet<PokemonCard>` for cards
  - Fluent API configuration for relationships
  - Migration support

#### Step 1.3: Configure Dependency Injection
- Register `PokemonCardDbContext` in Program.cs with appropriate lifetime (Scoped)
- Configure connection string in appsettings.json and appsettings.Development.json

---

### **Phase 2: Data Access Layer** (Weeks 2-3)

#### Step 2.1: Implement Repository Pattern
Create `Repositories/` folder:
```
Repositories/
├── ICardRepository.cs (interface)
└── CardRepository.cs (implementation)
```

**ICardRepository Interface Methods**:
- `Task<PokemonCard> GetCardByIdAsync(int id)`
- `Task<IEnumerable<PokemonCard>> GetAllCardsAsync()`
- `Task<IEnumerable<PokemonCard>> SearchByNameAsync(string name)`
- `Task<IEnumerable<PokemonCard>> SearchByCardNumberAsync(string number)`
- `Task<IEnumerable<PokemonCard>> GetCardsByTypeAsync(string type)`
- `Task<bool> CardExistsAsync(string apiId)`
- `Task<PokemonCard> AddCardAsync(PokemonCard card)`
- `Task<PokemonCard> UpdateCardAsync(PokemonCard card)`
- `Task<bool> DeleteCardAsync(int id)`

#### Step 2.2: Implement Async Data Access
- All methods should be `async` with `Task` return types
- Proper error handling with try-catch blocks
- Logging for debugging and monitoring

#### Step 2.3: Create Database Migrations
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

### **Phase 3: External API Integration** (Weeks 3-4)

#### Step 3.1: Create API Client Service
Create `Services/` folder:
```
Services/
├── IpokemonCardApiService.cs
└── PokemonCardApiService.cs
```

**Service Responsibilities**:
- Call external Pokémon card API
- Search cards by name
- Search cards by card number
- Parse and map API responses to domain models
- Handle API errors gracefully

#### Step 3.2: HttpClient Configuration
- Register `HttpClient` in DI container
- Configure base address, timeout, default headers
- Implement retry policies (using Polly if needed)

#### Step 3.3: Data Mapping
- Create mapping logic from API response → `PokemonCard` entity
- Handle null values and missing data gracefully

---

### **Phase 4: Application Services Layer** (Week 4)

#### Step 4.1: Create Business Logic Services
```
Services/
├── ICardCollectionService.cs
└── CardCollectionService.cs
```

**Service Methods**:
- `Task<PokemonCard> AddCardFromApiAsync(string name, string cardNumber)` - Fetch from API and store
- `Task<IEnumerable<PokemonCard>> SearchLocalCardsAsync(string name)` - Query local database
- `Task<IEnumerable<PokemonCard>> GetUserCollectionAsync()` - Get all cards
- `Task<bool> RemoveCardAsync(int cardId)`
- `Task<PokemonCard> UpdateCardConditionAsync(int cardId, string condition)`
- `Task<CardStatistics> GetCollectionStatisticsAsync()` - Count, value, etc.

#### Step 4.2: Error Handling & Validation
- Custom exceptions for business logic failures
- Input validation for all service methods
- Proper logging of operations

---

### **Phase 5: Blazor Components & UI** (Weeks 5-6)

#### Step 5.1: Create Component Structure
```
Components/
├── Pages/
│   ├── CardSearch.razor (Search API for new cards)
│   ├── MyCollection.razor (View stored cards)
│   ├── CardDetail.razor (View single card details)
│   └── AddCard.razor (Add card to collection)
├── Shared/
│   ├── CardCard.razor (Card display component)
│   └── SearchBar.razor (Reusable search component)
└── Forms/
    └── CardForm.razor (Add/edit card form)
```

#### Step 5.2: Implement Key Pages

**CardSearch.razor** - Search external API
- Input fields for: Name, Card Number
- Display API search results
- "Add to Collection" button for each result
- Loading state & error handling

**MyCollection.razor** - Display stored cards
- Table/Grid view of collected cards
- Filters: Type, Rarity, Condition
- Search box for filtering
- Edit/Delete buttons

**CardDetail.razor** - Individual card view
- Full card information
- Image display
- Condition selector
- Remove from collection button

#### Step 5.3: Component State Management
- Use component parameters for data passing
- Implement event callbacks for parent-child communication
- Use cascading parameters where appropriate

---

### **Phase 6: Data Validation & Error Handling** (Week 6)

#### Step 6.1: Input Validation
- Card name not empty/null
- Card number format validation
- Prevent duplicate cards in collection (check by ApiId)
- Condition/Type dropdown restrictions

#### Step 6.2: User Feedback
- Toast notifications (Blazor Toastr library)
- Loading spinners during async operations
- Error messages with actionable information
- Success messages for operations

---

### **Phase 7: Database Queries & Optimization** (Week 7)

#### Step 7.1: Optimize Query Performance
- Use `.AsNoTracking()` for read-only queries
- Implement pagination for large result sets
- Add database indexes on searchable columns (Name, CardNumber)
- Use `Select()` to project only needed fields

#### Step 7.2: Implement Search Features
- Full-text search on card names
- Partial card number matching
- Case-insensitive searches
- Combined filters (type + rarity + condition)

---

### **Phase 8: Testing** (Week 8)

#### Step 8.1: Unit Tests
Create `PokemonCardCollector.Tests` project:
```
Tests/
├── Services/
│   ├── CardCollectionServiceTests.cs
│   └── PokemonCardApiServiceTests.cs
├── Repositories/
│   └── CardRepositoryTests.cs
└── Components/
    └── CardSearchTests.cs
```

#### Step 8.2: Test Coverage
- Test repository CRUD operations
- Test API service with mock responses
- Test business logic validation
- Test component rendering and interactions

---

### **Phase 9: UI/UX Enhancements** (Week 9)

#### Step 9.1: Styling & Layout
- Bootstrap or Tailwind CSS integration
- Responsive design for mobile/tablet
- Card image gallery view
- Collection statistics dashboard

#### Step 9.2: User Experience
- Keyboard shortcuts
- Bulk operations (add multiple cards)
- Export collection (CSV/JSON)
- Sort options (by name, date added, value)

---

### **Phase 10: Deployment & Documentation** (Week 10)

#### Step 10.1: Production Setup
- Environment-specific configuration
- Secure API key management
- Database backup strategy
- Health checks implementation

#### Step 10.2: Documentation
- API documentation
- User guide
- Database schema documentation
- Deployment instructions

---

## 🏗️ Architecture Overview

```
Presentation Layer (Blazor Components)
           ↓
Application Layer (Services)
           ↓
Domain Layer (Models & Interfaces)
           ↓
Infrastructure Layer (Repository, DbContext, API Client)
           ↓
External API & Database
```

---

## 📦 Recommended NuGet Packages

- **Entity Framework Core**: `Microsoft.EntityFrameworkCore.*`
- **Logging**: `Microsoft.Extensions.Logging`
- **HTTP**: `HttpClientFactory` (built-in)
- **Validation**: `FluentValidation` (optional)
- **Testing**: `xUnit`, `Moq`, `xunit.runner.visualstudio`
- **UI Components**: `Blazorise` or `Syncfusion Blazor` (optional)

---

## 🔄 Workflow for Adding a Card

1. User searches for card by name/number in external API
2. API returns results with card details
3. User clicks "Add to Collection"
4. System checks if card already exists (by ApiId)
5. If new, card is saved to database
6. Card appears in "My Collection"
7. User can view, edit condition, or remove card

---

## 📊 Key Queries

```csharp
// Search by name (case-insensitive)
var cards = context.PokemonCards
    .Where(c => c.Name.Contains(searchTerm))
    .ToListAsync();

// Search by card number
var card = context.PokemonCards
    .FirstOrDefaultAsync(c => c.CardNumber == number);

// Get cards by type with conditions
var rareFireCards = context.PokemonCards
    .Where(c => c.Type == "Fire" && c.Rarity == "Rare")
    .OrderByDescending(c => c.DateAdded)
    .ToListAsync();
```

---

## ✅ Success Criteria

- ✅ Users can search external API for cards by name and number
- ✅ Cards can be stored in database
- ✅ Users can view all stored cards with filtering/search
- ✅ Users can manage card details (condition, notes)
- ✅ Graceful error handling and user feedback
- ✅ Responsive Blazor UI with good UX
- ✅ Proper async/await patterns throughout
- ✅ Comprehensive unit tests

Would you like me to start implementing any specific phase? I recommend starting with **Phase 1 & 2** (Database setup and repository pattern) as the foundation for everything else.

PaulSteindl: Create a mdfile for me and call it StepByStepPlan

GitHub Copilot: I'll create a comprehensive step-by-step plan markdown file for your Pokémon Card Collector application. I apologize, but it appears the file creation tool is currently disabled. However, I have the complete step-by-step plan ready for you. 

You can create this file yourself by:

1. **In VS Code**: Create a new file at the root of your project called `StepByStepPlan.md`
2. **Copy the content**: Paste the comprehensive plan I provided above
3. **Commit**: Add and commit the file to your repository

The plan includes:
- **10 Phases** with detailed implementation steps
- **Database design** for PokemonCard entity
- **Repository pattern** setup
- **API integration** with external service
- **Blazor components** architecture
- **Testing strategy** with xUnit
- **UI/UX enhancements**
- **Deployment checklist**
- **Key database queries**
- **Success criteria**
- **Tips for success**