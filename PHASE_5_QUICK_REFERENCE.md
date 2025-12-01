# Phase 5 Quick Reference Guide

**For**: AI Agents & Experienced Developers  
**Last Updated**: December 1, 2025  
**Status**: ✅ Production Ready

## Quick Jump

- **[Routes & Navigation](#routes--navigation)** - All page routes
- **[Component API Reference](#component-api-reference)** - Complete signatures
- **[Service Injection](#service-injection-patterns)** - DI patterns
- **[Common Patterns](#common-patterns)** - Code snippets
- **[CSS & Styling](#css--styling)** - Bootstrap classes
- **[Debugging & Help](#debugging--help)** - Troubleshooting

## Routes & Navigation

| Route | Component | Purpose |
|-------|-----------|---------|
| `/` | Home.razor | Landing page |
| `/card-search` | CardSearch.razor | Search API & add cards |
| `/my-collection` | MyCollection.razor | View & manage collection |
| `/card-detail/{id}` | CardDetail.razor | View card details |
| `/card-edit/{id}` | CardDetail.razor | Edit card info |
| `/collection-stats` | CollectionStats.razor | Analytics dashboard |

## Component API Reference

### PageHeader.razor

```csharp
// Parameters
[Parameter] public required string Title { get; set; }
[Parameter] public string? Description { get; set; }
[Parameter] public RenderFragment? ActionContent { get; set; }

// Usage
<PageHeader Title="Page Title" Description="Optional description" />
```

### SearchBar.razor

```csharp
// Parameters
[Parameter] public string Placeholder { get; set; } = "Search cards...";
[Parameter] public EventCallback<string> OnSearchRequested { get; set; }
[Parameter] public bool IsSearching { get; set; } = false;

// Public Methods
public void ClearSearch()

// Usage
<SearchBar 
    Placeholder="Search cards..."
    OnSearchRequested="HandleSearch"
    IsSearching="@isLoading" />
```

### CardCard.razor

```csharp
// Parameters
[Parameter] public required Card Card { get; set; }
[Parameter] public bool ShowViewDetailsButton { get; set; } = false;
[Parameter] public bool ShowEditButton { get; set; } = false;
[Parameter] public bool ShowRemoveButton { get; set; } = false;
[Parameter] public bool ShowAddButton { get; set; } = false;
[Parameter] public bool IsAdding { get; set; } = false;
[Parameter] public EventCallback<int> OnViewDetails { get; set; }
[Parameter] public EventCallback<int> OnEdit { get; set; }
[Parameter] public EventCallback<int> OnRemove { get; set; }
[Parameter] public EventCallback<string> OnAdd { get; set; }

// Usage
<CardCard 
    Card="@card"
    ShowViewDetailsButton="true"
    ShowEditButton="true"
    ShowRemoveButton="true"
    OnViewDetails="ViewCard"
    OnEdit="EditCard"
    OnRemove="RemoveCard" />
```

## Service Injection Patterns

All pages inject these services:

```csharp
@inject ICardCollectionService CardCollectionService
@inject NavigationManager NavigationManager
@inject ILogger<PageName> Logger
@inject IJSRuntime? JSRuntime  // For confirmations
```

**Service Lifetimes**:
- `ICardCollectionService`: Scoped (per HTTP request)
- `NavigationManager`: Scoped
- `ILogger<T>`: Singleton
- `IJSRuntime`: Singleton

## Common Patterns

### State Variables

```csharp
private List<Card> cards = new();          // Data list
private bool isLoading = true;              // Loading state
private string? errorMessage;               // Error display
private string? successMessage;             // Success feedback
private int currentPage = 1;                // Pagination
private int pageSize = 10;                  // Page size
```

### Error Handling
try
{
    isLoading = true;
    errorMessage = null;
    
    // Perform operation
    
    Logger.LogInformation("Operation completed");
}
catch (Exception ex)
{
    Logger.LogError(ex, "Operation failed");
    errorMessage = "Friendly error message";
}
finally
{
    isLoading = false;
}
```

## CSS Classes Used

**Spacing**:
try
{
    isLoading = true;
    errorMessage = null;
    
    // Perform operation
    
    Logger.LogInformation("Operation completed");
}
catch (InvalidOperationException ex)
{
    Logger.LogWarning(ex, "Business rule violated");
    errorMessage = ex.Message;
}
catch (Exception ex)
{
    Logger.LogError(ex, "Unexpected error");
    errorMessage = "An error occurred. Please try again.";
}
finally
{
    isLoading = false;
}
```

### Component Composition

```csharp
// Parent passing child data
<CardCard Card="@currentCard" ShowEditButton="true" OnEdit="HandleEdit" />

// Child invoking parent callback
private async Task HandleClick()
{
    await OnEdit.InvokeAsync(card.Id);
}
```

### Async/Pagination Pattern

```csharp
private async Task LoadPage(int pageNumber)
{
    try
    {
        isLoading = true;
        var (cards, total) = await CardCollectionService
            .GetUserCollectionAsync(pageNumber, pageSize, CancellationToken.None);
        this.cards = cards.ToList();
        totalCount = total;
    }
    catch (Exception ex)
    {
        Logger.LogError(ex, "Failed to load page {Page}", pageNumber);
        errorMessage = "Failed to load cards";
    }
    finally
    {
        isLoading = false;
    }
}
```

## CSS & Styling

### Layout Classes

- `container-fluid` - Full-width container
- `row`, `col-md-6`, `col-lg-4` - Bootstrap grid
- `d-flex`, `justify-content-between` - Flexbox
- `px-4`, `mb-3`, `g-3` - Spacing (padding, margin, gap)

### Component Classes

- `card`, `card-body`, `card-header` - Card containers
- `btn`, `btn-primary`, `btn-danger`, `btn-success` - Buttons
- `alert`, `alert-success`, `alert-danger`, `alert-warning` - Alerts
- `badge`, `badge-primary` - Badges
- `spinner-border`, `spinner-border-sm` - Loading spinners

### Text Classes

- `display-5` - Large titles
- `lead` - Subtitle text
- `text-muted` - Secondary text
- `text-success`, `text-danger` - Color text

### Custom Color/Style Quick Reference

```html
<!-- Buttons -->
<button class="btn btn-primary">Primary</button>
<button class="btn btn-success">Success</button>
<button class="btn btn-danger">Danger</button>
<button class="btn btn-warning">Warning</button>

<!-- Alerts -->
<div class="alert alert-success">Success message</div>
<div class="alert alert-danger">Error message</div>
<div class="alert alert-warning">Warning message</div>

<!-- Badges -->
<span class="badge bg-success">Mint</span>
<span class="badge bg-danger">Poor</span>

<!-- Grid -->
<div class="col-md-6">50% on medium+ screens</div>
<div class="col-lg-4">33% on large+ screens</div>
```

## Debugging & Help

### Development

```bash
# Run application
dotnet run
# Navigate to https://localhost:5001

# Build & check for errors
dotnet build

# Run tests (if configured)
dotnet test
```

### Browser DevTools (F12)

- **Console**: Check for JavaScript errors
- **Network**: Monitor API calls and response times
- **Application**: View localStorage, cookies
- **Elements**: Inspect DOM structure

### Application Logs

- Visual Studio Output window shows `ILogger` output
- Look for `LogError`, `LogWarning`, `LogInformation`
- Development environment shows detailed exception info

### Common Issues & Solutions

| Issue | Solution |
|-------|----------|
| Component not rendering | Check `@code` section for syntax errors |
| Service not injected | Verify `@inject` statement and DI registration |
| Data not displaying | Check if task is `await`ed and data is bound with `@` |
| Styling not applied | Check CSS class names and Bootstrap import |
| API call failing | Verify service is registered with correct lifetime |
| Async errors | Use `try-catch` and check `CancellationToken` |

## Service Methods Quick Reference

| Service | Method | Returns | Pages |
|---------|--------|---------|-------|
| ICardCollectionService | BrowseCardsByNameAsync(string) | List<Card> | CardSearch |
| ICardCollectionService | BrowseCardsByNumberAsync(string, string) | List<Card> | CardSearch |
| ICardCollectionService | AddCardFromApiAsync(string) | Card | CardSearch |
| ICardCollectionService | GetUserCollectionAsync(int, int) | (List<Card>, int) | MyCollection |
| ICardCollectionService | SearchLocalCardsAsync(string) | List<Card> | MyCollection |
| ICardCollectionService | GetCollectionStatisticsAsync() | CollectionStatistics | MyCollection, CollectionStats |
| ICardCollectionService | UpdateCardAsync(int, string, string) | Card | CardDetail |
| ICardCollectionService | RemoveCardAsync(int) | bool | MyCollection, CardDetail |

## Key Files Quick Reference

| File | Type | Lines | Purpose |
|------|------|-------|---------|
| `Components/Pages/CardSearch.razor` | Page | 253 | Search API & add cards |
| `Components/Pages/MyCollection.razor` | Page | 375 | Browse & manage collection |
| `Components/Pages/CardDetail.razor` | Page | 380 | View & edit card details |
| `Components/Pages/CollectionStats.razor` | Page | 310 | Analytics dashboard |
| `Components/Shared/SearchBar.razor` | Component | 40 | Reusable search input |
| `Components/Shared/CardCard.razor` | Component | 180 | Reusable card display |
| `Components/Shared/PageHeader.razor` | Component | 22 | Reusable page header |

## Performance Tips

1. **Collection Loading**
   - Use pagination (don't load all cards)
   - Apply filters to reduce data
   - Avoid `StateHasChanged()` in loops

2. **Search Optimization**
   - Debounce search input for frequent searches
   - Use indexed database fields
   - Cache popular searches

3. **Image Loading**
   - Use lazy loading attributes
   - Set max-height/max-width on images
   - Consider CDN for image delivery

4. **Component Updates**
   - Use `@key` for list rendering
   - Minimize parameter changes
   - Avoid unnecessary async calls

## Known Limitations & Future Work

- Statistics recalculate on every load (no caching)
- No virtual scrolling for large collections
- No bulk operations (add/remove multiple)
- Images not cached between sessions
- No search result pagination

## Getting Help

- **Component Details**: See `PHASE_5_OVERVIEW.md`
- **Service Documentation**: See `PHASE_4_OVERVIEW.md`
- **Code Comments**: Check inline XML documentation
- **Examples**: Review existing page implementations

---

**Phase 5 Status**: ✅ Complete and Production Ready
**Last Updated**: December 1, 2025
