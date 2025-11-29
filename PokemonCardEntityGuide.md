# PokemonCard Entity Structure - JSON Guide

Based on the TCGdex API documentation (https://tcgdex.dev/reference/card), this guide provides comprehensive information about the PokemonCard entity structure and the different card types supported by the API.

## Overview

The TCGdex API supports three main card categories:
1. **Pokémon Cards** - Battle creatures with HP, attacks, and abilities
2. **Trainer Cards** - Support cards with various effects
3. **Energy Cards** - Resource cards needed to power attacks

All cards share common properties, with additional fields specific to their category.

---

## Common Properties (All Cards)

These properties are present in every card response:

```json
{
  "id": "swsh3-136",
  "localId": "136",
  "name": "Furret",
  "category": "Pokemon|Trainer|Energy",
  "image": "https://assets.tcgdex.net/en/swsh/swsh3/136",
  "illustrator": "tetsuya koizumi",
  "rarity": "Common|Uncommon|Rare|RareHolo|RareHoloEX|RareHoloLv.X|etc",
  "set": {
    "id": "swsh3",
    "name": "Darkness Ablaze",
    "logo": "https://assets.tcgdex.net/en/swsh/swsh3/logo",
    "symbol": "https://assets.tcgdex.net/univ/swsh/swsh3/symbol",
    "cardCount": {
      "official": 189,
      "total": 201
    }
  },
  "variants": {
    "normal": true,
    "reverse": true,
    "holo": false,
    "firstEdition": false,
    "wPromo": false
  },
  "boosters": [
    {
      "id": "swsh3.5-blister",
      "name": "Blister Booster Pack",
      "logo": null,
      "artwork_front": "https://assets.tcgdex.net/...",
      "artwork_back": "https://assets.tcgdex.net/..."
    }
  ],
  "updated": "2024-02-04T22:55:32+02:00",
  "pricing": {
    "tcgplayer": {
      "updated": "2025-08-05T20:07:54.000Z",
      "unit": "USD",
      "normal": {
        "lowPrice": 0.02,
        "midPrice": 0.17,
        "highPrice": 25.09,
        "marketPrice": 0.09,
        "directLowPrice": 0.04
      },
      "holofoil": {
        "lowPrice": 0.05,
        "midPrice": 0.25,
        "highPrice": 10.00,
        "marketPrice": 0.20,
        "directLowPrice": 0.15
      },
      "reverse-holofoil": {
        "lowPrice": 0.09,
        "midPrice": 0.26,
        "highPrice": 5.17,
        "marketPrice": 0.23,
        "directLowPrice": 0.23
      },
      "1st-edition": {
        "lowPrice": 0.50,
        "midPrice": 1.00,
        "highPrice": 100.00,
        "marketPrice": 0.75,
        "directLowPrice": 0.60
      },
      "1st-edition-holofoil": {
        "lowPrice": 2.00,
        "midPrice": 5.00,
        "highPrice": 500.00,
        "marketPrice": 3.50,
        "directLowPrice": 2.50
      },
      "unlimited": {
        "lowPrice": 0.01,
        "midPrice": 0.10,
        "highPrice": 50.00,
        "marketPrice": 0.05,
        "directLowPrice": 0.02
      },
      "unlimited-holofoil": {
        "lowPrice": 0.05,
        "midPrice": 0.15,
        "highPrice": 25.00,
        "marketPrice": 0.12,
        "directLowPrice": 0.08
      }
    },
    "cardmarket": {
      "updated": "2025-08-05T00:42:15.000Z",
      "unit": "EUR",
      "avg": 0.08,
      "low": 0.02,
      "trend": 0.08,
      "avg1": 0.03,
      "avg7": 0.08,
      "avg30": 0.08,
      "avg-holo": 0.27,
      "low-holo": 0.03,
      "trend-holo": 0.21,
      "avg1-holo": 0.19,
      "avg7-holo": 0.19,
      "avg30-holo": 0.26
    }
  }
}
```

---

## Pokémon Card Properties

Pokémon cards include all common properties PLUS these specific fields:

```json
{
  "category": "Pokemon",
  "dexId": [162],
  "hp": 110,
  "types": ["Colorless"],
  "evolveFrom": "Sentret",
  "description": "It makes a nest to suit its long and skinny body. The nest is impossible for other Pokémon to enter.",
  "level": null,
  "stage": "Stage1",
  "suffix": null,
  "item": {
    "name": "Poké Ball",
    "effect": "Attach a basic Pokémon from your deck to your active Pokémon as many times as you want"
  },
  "attacks": [
    {
      "name": "Feelin' Fine",
      "cost": ["Colorless"],
      "effect": "Draw 3 cards.",
      "damage": null
    },
    {
      "name": "Tail Smash",
      "cost": ["Colorless"],
      "effect": "Flip a coin. If tails, this attack does nothing.",
      "damage": 90
    }
  ],
  "abilities": [
    {
      "name": "Ability Name",
      "type": "Ability|Pokémon Power|Poké-Power|Poké-Body",
      "effect": "Ability effect text goes here"
    }
  ],
  "weaknesses": [
    {
      "type": "Fighting",
      "value": "×2"
    }
  ],
  "resistances": [
    {
      "type": "Psychic",
      "value": "-20"
    }
  ],
  "retreat": 1,
  "regulationMark": "D",
  "legal": {
    "standard": false,
    "expanded": true
  }
}
```

### Pokémon Types
Valid values for the `types` array:
- Colorless
- Darkness
- Dragon
- Fairy
- Fighting
- Fire
- Grass
- Lightning
- Metal
- Psychic
- Water

### Evolution Stages
Valid values for the `stage` field:
- Basic
- Stage1
- Stage2
- LevelUp
- VMAX
- VSTAR

### Attack Cost Format
Attack costs are represented as an array of energy types:
```json
"cost": ["Fire", "Fire", "Colorless", "Colorless"]
```

### Weaknesses and Resistances Format
```json
"weaknesses": [
  {
    "type": "Water",
    "value": "×2"
  }
],
"resistances": [
  {
    "type": "Fire",
    "value": "-20"
  }
]
```

---

## Trainer Card Properties

Trainer cards include all common properties PLUS these specific fields:

```json
{
  "category": "Trainer",
  "effect": "Search your deck for a Supporter card and put it into your hand. Then, shuffle your deck.",
  "trainerType": "Supporter|Item|Stadium|Pokémon Tool|Pokémon Tool F|Ace Spec|Pendant|Ancient Rotor|etc"
}
```

### Trainer Card Types
Valid values for the `trainerType` field:
- **Supporter** - Powerful cards that can only be played once per turn
- **Item** - Single-use cards with immediate effects
- **Stadium** - Continuous effect cards that affect both players
- **Pokémon Tool** - Cards attached to Pokémon for effects
- **Pokémon Tool F** - Restricted tool cards
- **Ace Spec** - High-power cards with the Ace Spec rule
- **Ancient Rotor** - Ancient type tools
- **Pendant** - Pendant type tools
- **Technical Machines** - Temporary effects for Pokémon
- **Scoop Up** - Utility cards

---

## Energy Card Properties

Energy cards include all common properties PLUS these specific fields:

```json
{
  "category": "Energy",
  "effect": "This card provides 1 [C] energy. While this card is attached to a Pokémon, it provides 2 [C] instead.",
  "energyType": "Special|Basic"
}
```

### Energy Types
Valid values for the `energyType` field:
- **Basic** - Standard single-type energy cards (Fire, Water, Grass, Lightning, Psychic, Fighting, Darkness, Metal, Dragon, Fairy, Colorless)
- **Special** - Multi-type or special effect energy cards

### Basic Energy Subtypes
For basic energy cards, the `types` array will contain:
```json
"types": ["Fire"]  // Fire, Water, Grass, Lightning, Psychic, Fighting, Darkness, Metal, Dragon, Fairy, Colorless
```

---

## Variants Details

Card variants represent different physical editions/finishes available:

```json
"variants": {
  "normal": true,           // Standard non-foil version
  "reverse": true,          // Reverse holofoil (foil background)
  "holo": false,            // Regular holofoil
  "firstEdition": false,    // First edition printing
  "wPromo": false           // Special promotional variant
}
```

---

## Pricing Information Structure

### TCGPlayer Pricing (USD)
Market data for the North American market with different variants:

```json
"tcgplayer": {
  "updated": "2025-08-05T20:07:54.000Z",
  "unit": "USD",
  "normal": {                      // Standard non-foil
    "lowPrice": 0.02,
    "midPrice": 0.17,
    "highPrice": 25.09,
    "marketPrice": 0.09,
    "directLowPrice": 0.04
  },
  "holofoil": {},                  // Regular holofoil
  "reverse-holofoil": {},          // Reverse holofoil
  "1st-edition": {},               // First edition
  "1st-edition-holofoil": {},      // First edition holofoil
  "unlimited": {},                 // Unlimited edition
  "unlimited-holofoil": {}         // Unlimited holofoil
}
```

**Fields meaning:**
- `lowPrice` - Lowest available price for this variant
- `midPrice` - Median market price
- `highPrice` - Highest available price
- `marketPrice` - Current market price
- `directLowPrice` - Lowest direct seller price

### Cardmarket Pricing (EUR)
European market data with foil and non-foil variants:

```json
"cardmarket": {
  "updated": "2025-08-05T00:42:15.000Z",
  "unit": "EUR",
  // Non-foil pricing
  "avg": 0.08,                     // Average selling price
  "low": 0.02,                     // Lowest market price
  "trend": 0.08,                   // Trend price from charts
  "avg1": 0.03,                    // Average (last 24 hours)
  "avg7": 0.08,                    // Average (last 7 days)
  "avg30": 0.08,                   // Average (last 30 days)
  // Foil pricing
  "avg-holo": 0.27,                // Average foil price
  "low-holo": 0.03,                // Lowest foil price
  "trend-holo": 0.21,              // Trend foil price
  "avg1-holo": 0.19,               // Average foil (last 24 hours)
  "avg7-holo": 0.19,               // Average foil (last 7 days)
  "avg30-holo": 0.26               // Average foil (last 30 days)
}
```

---

## Complete Examples by Card Type

### Example 1: Pokémon Card (Full Structure)

```json
{
  "id": "swsh3-136",
  "localId": "136",
  "name": "Furret",
  "category": "Pokemon",
  "image": "https://assets.tcgdex.net/en/swsh/swsh3/136",
  "illustrator": "tetsuya koizumi",
  "rarity": "Uncommon",
  "set": {
    "id": "swsh3",
    "name": "Darkness Ablaze",
    "logo": "https://assets.tcgdex.net/en/swsh/swsh3/logo",
    "symbol": "https://assets.tcgdex.net/univ/swsh/swsh3/symbol",
    "cardCount": {
      "official": 189,
      "total": 201
    }
  },
  "variants": {
    "normal": true,
    "reverse": true,
    "holo": false,
    "firstEdition": false,
    "wPromo": false
  },
  "dexId": [162],
  "hp": 110,
  "types": ["Colorless"],
  "evolveFrom": "Sentret",
  "description": "It makes a nest to suit its long and skinny body. The nest is impossible for other Pokémon to enter.",
  "level": null,
  "stage": "Stage1",
  "suffix": null,
  "item": null,
  "attacks": [
    {
      "name": "Feelin' Fine",
      "cost": ["Colorless"],
      "effect": "Draw 3 cards.",
      "damage": null
    },
    {
      "name": "Tail Smash",
      "cost": ["Colorless"],
      "effect": "Flip a coin. If tails, this attack does nothing.",
      "damage": 90
    }
  ],
  "abilities": [],
  "weaknesses": [
    {
      "type": "Fighting",
      "value": "×2"
    }
  ],
  "resistances": [],
  "retreat": 1,
  "regulationMark": "D",
  "legal": {
    "standard": false,
    "expanded": true
  },
  "updated": "2024-02-04T22:55:32+02:00",
  "pricing": {
    "tcgplayer": {
      "updated": "2025-08-05T20:07:54.000Z",
      "unit": "USD",
      "normal": {
        "lowPrice": 0.02,
        "midPrice": 0.17,
        "highPrice": 25.09,
        "marketPrice": 0.09,
        "directLowPrice": 0.04
      }
    },
    "cardmarket": {
      "updated": "2025-08-05T00:42:15.000Z",
      "unit": "EUR",
      "avg": 0.08,
      "low": 0.02,
      "trend": 0.08,
      "avg1": 0.03,
      "avg7": 0.08,
      "avg30": 0.08
    }
  }
}
```

### Example 2: Trainer Card

```json
{
  "id": "bw1-101",
  "localId": "101",
  "name": "Pokémon Breeder",
  "category": "Trainer",
  "image": "https://assets.tcgdex.net/en/bw/bw1/101",
  "illustrator": "Ken Sugimori",
  "rarity": "Uncommon",
  "set": {
    "id": "bw1",
    "name": "Black & White",
    "logo": "https://assets.tcgdex.net/en/bw/bw1/logo",
    "symbol": "https://assets.tcgdex.net/univ/bw/bw1/symbol",
    "cardCount": {
      "official": 114,
      "total": 114
    }
  },
  "variants": {
    "normal": true,
    "reverse": false,
    "holo": false,
    "firstEdition": false,
    "wPromo": false
  },
  "effect": "Search your deck for a card that evolves from 1 of your Pokémon and put it onto that Pokémon. (This counts as evolving that Pokémon.) Shuffle your deck.",
  "trainerType": "Supporter",
  "updated": "2024-02-04T22:55:32+02:00",
  "pricing": {
    "tcgplayer": {
      "updated": "2025-08-05T20:07:54.000Z",
      "unit": "USD",
      "normal": {
        "lowPrice": 0.50,
        "midPrice": 2.00,
        "highPrice": 50.00,
        "marketPrice": 1.50,
        "directLowPrice": 0.75
      }
    },
    "cardmarket": {
      "updated": "2025-08-05T00:42:15.000Z",
      "unit": "EUR",
      "avg": 1.50,
      "low": 0.50,
      "trend": 1.25,
      "avg1": 1.40,
      "avg7": 1.45,
      "avg30": 1.48
    }
  }
}
```

### Example 3: Energy Card

```json
{
  "id": "swsh1-230",
  "localId": "230",
  "name": "Fire Energy",
  "category": "Energy",
  "image": "https://assets.tcgdex.net/en/swsh/swsh1/230",
  "illustrator": null,
  "rarity": "Common",
  "set": {
    "id": "swsh1",
    "name": "Sword & Shield",
    "logo": "https://assets.tcgdex.net/en/swsh/swsh1/logo",
    "symbol": "https://assets.tcgdex.net/univ/swsh/swsh1/symbol",
    "cardCount": {
      "official": 202,
      "total": 202
    }
  },
  "variants": {
    "normal": true,
    "reverse": false,
    "holo": false,
    "firstEdition": false,
    "wPromo": false
  },
  "effect": null,
  "energyType": "Basic",
  "types": ["Fire"],
  "updated": "2024-02-04T22:55:32+02:00",
  "pricing": {
    "tcgplayer": {
      "updated": "2025-08-05T20:07:54.000Z",
      "unit": "USD",
      "normal": {
        "lowPrice": 0.01,
        "midPrice": 0.05,
        "highPrice": 1.00,
        "marketPrice": 0.03,
        "directLowPrice": 0.01
      }
    },
    "cardmarket": {
      "updated": "2025-08-05T00:42:15.000Z",
      "unit": "EUR",
      "avg": 0.02,
      "low": 0.01,
      "trend": 0.02,
      "avg1": 0.02,
      "avg7": 0.02,
      "avg30": 0.02
    }
  }
}
```

### Example 4: Special Energy Card

```json
{
  "id": "sv1-209",
  "localId": "209",
  "name": "Capture Energy",
  "category": "Energy",
  "image": "https://assets.tcgdex.net/en/sv/sv1/209",
  "illustrator": "5ban Graphics",
  "rarity": "Common",
  "set": {
    "id": "sv1",
    "name": "Scarlet & Violet",
    "logo": "https://assets.tcgdex.net/en/sv/sv1/logo",
    "symbol": "https://assets.tcgdex.net/univ/sv/sv1/symbol",
    "cardCount": {
      "official": 198,
      "total": 198
    }
  },
  "variants": {
    "normal": true,
    "reverse": true,
    "holo": false,
    "firstEdition": false,
    "wPromo": false
  },
  "effect": "This card provides 1 Colorless energy. While this card is attached to a Pokémon, it provides 1 Colorless energy. If the Pokémon this card is attached to is Knocked Out, put this card into your hand.",
  "energyType": "Special",
  "types": ["Colorless"],
  "updated": "2024-02-04T22:55:32+02:00",
  "pricing": {
    "tcgplayer": {
      "updated": "2025-08-05T20:07:54.000Z",
      "unit": "USD",
      "normal": {
        "lowPrice": 0.05,
        "midPrice": 0.25,
        "highPrice": 5.00,
        "marketPrice": 0.20,
        "directLowPrice": 0.10
      }
    },
    "cardmarket": {
      "updated": "2025-08-05T00:42:15.000Z",
      "unit": "EUR",
      "avg": 0.15,
      "low": 0.05,
      "trend": 0.12,
      "avg1": 0.13,
      "avg7": 0.14,
      "avg30": 0.15
    }
  }
}
```

---

## C# Entity Model (Recommended Structure)

Based on this JSON guide, here's the recommended C# entity structure:

```csharp
public class PokemonCard
{
    // Common Properties
    public int Id { get; set; }
    public string ApiId { get; set; } // "swsh3-136"
    public string LocalId { get; set; } // "136"
    public string Name { get; set; }
    public string Category { get; set; } // "Pokemon", "Trainer", "Energy"
    public string ImageUrl { get; set; }
    public string Illustrator { get; set; }
    public string Rarity { get; set; }
    public DateTime Updated { get; set; }
    
    // Set Information
    public string SetId { get; set; }
    public string SetName { get; set; }
    
    // Variants
    public bool VariantNormal { get; set; }
    public bool VariantReverse { get; set; }
    public bool VariantHolo { get; set; }
    public bool VariantFirstEdition { get; set; }
    
    // Pricing
    public decimal TcgPlayerPrice { get; set; }
    public decimal CardmarketPrice { get; set; }
    
    // Pokemon Properties (nullable for non-Pokemon cards)
    public string DexId { get; set; } // JSON array stored as string
    public int? Hp { get; set; }
    public string Types { get; set; } // JSON array
    public string EvolveFrom { get; set; }
    public string Description { get; set; }
    public string Stage { get; set; } // "Basic", "Stage1", "Stage2", "VMAX", "VSTAR"
    public string Attacks { get; set; } // JSON array
    public string Abilities { get; set; } // JSON array
    public string Weaknesses { get; set; } // JSON array
    public string Resistances { get; set; } // JSON array
    public int? RetreatCost { get; set; }
    public string RegulationMark { get; set; }
    public bool LegalStandard { get; set; }
    public bool LegalExpanded { get; set; }
    
    // Trainer Properties (nullable for non-Trainer cards)
    public string Effect { get; set; }
    public string TrainerType { get; set; } // "Supporter", "Item", "Stadium", etc.
    
    // Energy Properties (nullable for non-Energy cards)
    public string EnergyType { get; set; } // "Basic", "Special"
    
    // Metadata
    public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    public string UserNotes { get; set; }
    public string Condition { get; set; } // "Mint", "NearMint", "LightlyPlayed", "Played", "PoorCondition"
}
```

---

## API Response Fields Requirement Key

- ✅ **Field is always present** in the response
- ❌ **Field is omitted** when there is no value in the response

### Field Presence by Card Type

| Field | Common | Pokemon | Trainer | Energy |
|-------|--------|---------|---------|--------|
| id | ✅ | ✅ | ✅ | ✅ |
| name | ✅ | ✅ | ✅ | ✅ |
| category | ✅ | ✅ | ✅ | ✅ |
| set | ✅ | ✅ | ✅ | ✅ |
| variants | ✅ | ✅ | ✅ | ✅ |
| dexId | ❌ | ✅ | ❌ | ❌ |
| hp | ❌ | ✅ | ❌ | ❌ |
| types | ❌ | ✅ | ❌ | ❌ |
| attacks | ❌ | ✅ | ❌ | ❌ |
| effect | ❌ | ❌ | ✅ | ✅ |
| trainerType | ❌ | ❌ | ✅ | ❌ |
| energyType | ❌ | ❌ | ❌ | ✅ |

---

## Key Integration Points for Your Blazor App

1. **API Client Integration**: Use TCGdex API to search cards by name and number
2. **Database Mapping**: Store all relevant fields in your PokemonCard entity
3. **Search Features**: Leverage the API fields for advanced filtering
4. **Pricing Display**: Show both TCGPlayer (USD) and Cardmarket (EUR) prices
5. **Card Details**: Display complete card information based on category
6. **Collection Management**: Filter by type, rarity, condition, legality status

---

## Additional API Resources

- **REST API Documentation**: https://tcgdex.dev/rest
- **Search Cards Endpoint**: https://tcgdex.dev/rest/cards
- **Get Single Card**: https://tcgdex.dev/rest/card
- **Filtering & Pagination**: https://tcgdex.dev/rest/filtering-sorting-pagination
- **Card Brief Object**: https://tcgdex.dev/reference/card-brief
- **Available SDKs**: Java, JavaScript, Kotlin, PHP, Python, TypeScript
