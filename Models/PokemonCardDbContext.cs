using Microsoft.EntityFrameworkCore;

namespace PokemonCardCollector.Models;

/// <summary>
/// Represents the database context for the Pokémon Card Collector application.
/// Manages entity sets for Card and its derived types (PokemonCard, TrainerCard, EnergyCard).
/// Uses Table-Per-Hierarchy (TPH) inheritance pattern with discriminator column.
/// </summary>
public class PokemonCardDbContext(DbContextOptions<PokemonCardDbContext> options) 
    : DbContext(options)
{
    /// <summary>
    /// Gets or sets the collection of all Card entities (polymorphic).
    /// </summary>
    public DbSet<Card> Cards { get; set; }

    /// <summary>
    /// Gets or sets the collection of Pokémon card entities.
    /// </summary>
    public DbSet<PokemonCard> PokemonCards { get; set; }

    /// <summary>
    /// Gets or sets the collection of Trainer card entities.
    /// </summary>
    public DbSet<TrainerCard> TrainerCards { get; set; }

    /// <summary>
    /// Gets or sets the collection of Energy card entities.
    /// </summary>
    public DbSet<EnergyCard> EnergyCards { get; set; }

    /// <summary>
    /// Configures the model and database relationships using Table-Per-Hierarchy inheritance.
    /// </summary>
    /// <param name="modelBuilder">The model builder for configuring entity mappings.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure base Card entity with Table-Per-Hierarchy (TPH) inheritance
        var cardEntity = modelBuilder.Entity<Card>();

        // Discriminator column for TPH inheritance
        cardEntity.HasDiscriminator<string>("CardType")
            .HasValue<PokemonCard>("PokemonCard")
            .HasValue<TrainerCard>("TrainerCard")
            .HasValue<EnergyCard>("EnergyCard");

        // Primary Key
        cardEntity.HasKey(c => c.Id);

        // Required Properties
        cardEntity.Property(c => c.ApiId)
            .IsRequired()
            .HasMaxLength(50);

        cardEntity.Property(c => c.LocalId)
            .IsRequired()
            .HasMaxLength(20);

        cardEntity.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(500);

        cardEntity.Property(c => c.SetId)
            .IsRequired()
            .HasMaxLength(50);

        cardEntity.Property(c => c.SetName)
            .IsRequired()
            .HasMaxLength(200);

        // Optional Properties with constraints
        cardEntity.Property(c => c.ImageUrl)
            .HasMaxLength(500);

        cardEntity.Property(c => c.Illustrator)
            .HasMaxLength(200);

        cardEntity.Property(c => c.Rarity)
            .HasMaxLength(100);

        cardEntity.Property(c => c.Condition)
            .HasMaxLength(50); // "Mint", "NearMint", "LightlyPlayed", "Played", "PoorCondition"

        cardEntity.Property(c => c.UserNotes)
            .HasMaxLength(2000);

        // Price properties with decimal precision (10,2) for currency
        cardEntity.Property(c => c.TcgPlayerPrice)
            .HasPrecision(10, 2);

        cardEntity.Property(c => c.CardmarketPrice)
            .HasPrecision(10, 2);

        cardEntity.Property(c => c.EstimatedValue)
            .HasPrecision(10, 2);

        // DateTime properties with UTC conversion
        cardEntity.Property(c => c.Updated)
            .HasConversion(v => v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        cardEntity.Property(c => c.DateAdded)
            .HasConversion(v => v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        // Indexes for performance on base entity
        cardEntity.HasIndex(c => c.ApiId)
            .IsUnique()
            .HasDatabaseName("IX_Card_ApiId");

        cardEntity.HasIndex(c => c.Name)
            .HasDatabaseName("IX_Card_Name");

        cardEntity.HasIndex(c => c.LocalId)
            .HasDatabaseName("IX_Card_LocalId");

        cardEntity.HasIndex(c => new { c.SetId, c.LocalId })
            .IsUnique()
            .HasDatabaseName("IX_Card_SetId_LocalId");

        cardEntity.HasIndex(c => c.Rarity)
            .HasDatabaseName("IX_Card_Rarity");

        cardEntity.HasIndex(c => c.DateAdded)
            .HasDatabaseName("IX_Card_DateAdded");

        cardEntity.HasIndex(c => c.Condition)
            .HasDatabaseName("IX_Card_Condition");

        // Composite indexes for common queries
        cardEntity.HasIndex(c => new { c.Rarity, c.Condition })
            .HasDatabaseName("IX_Card_Rarity_Condition");

        // Configure PokemonCard entity properties
        var pokemonCard = modelBuilder.Entity<PokemonCard>();

        pokemonCard.Property(c => c.DexId)
            .HasMaxLength(500); // JSON array

        pokemonCard.Property(c => c.Types)
            .HasMaxLength(500); // JSON array

        pokemonCard.Property(c => c.Attacks)
            .HasColumnType("TEXT"); // JSON array with large content

        pokemonCard.Property(c => c.Abilities)
            .HasColumnType("TEXT"); // JSON array with large content

        pokemonCard.Property(c => c.Weaknesses)
            .HasMaxLength(500); // JSON array

        pokemonCard.Property(c => c.Resistances)
            .HasMaxLength(500); // JSON array

        pokemonCard.Property(c => c.Stage)
            .HasMaxLength(20); // "Basic", "Stage1", "Stage2", "VMAX", "VSTAR", etc.

        pokemonCard.Property(c => c.EvolveFrom)
            .HasMaxLength(200);

        pokemonCard.Property(c => c.Description)
            .HasMaxLength(1000);

        pokemonCard.Property(c => c.RegulationMark)
            .HasMaxLength(10);

        // Configure TrainerCard entity properties
        var trainerCard = modelBuilder.Entity<TrainerCard>();

        trainerCard.Property(c => c.TrainerType)
            .IsRequired()
            .HasMaxLength(100); // "Supporter", "Item", "Stadium", etc.

        trainerCard.Property(c => c.Effect)
            .HasMaxLength(2000);

        // Configure EnergyCard entity properties
        var energyCard = modelBuilder.Entity<EnergyCard>();

        energyCard.Property(c => c.EnergyType)
            .IsRequired()
            .HasMaxLength(20); // "Basic", "Special"

        energyCard.Property(c => c.Effect)
            .HasMaxLength(2000);
    }
}
