using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HomebrewDesigner.Infrastructure.DbContext;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    //represents an entity set that can be queried from database
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Fermentables> Fermentables { get; set; }
    public DbSet<Hop> Hops { get; set; }
    public DbSet<Yeast> Yeasts { get; set; }


    //specifies how a model is mapped to the db
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Fermentables>()
            .HasKey(f => f.Id)
            .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

        modelBuilder.Entity<HopAddition>()
            .HasOne(h => h.Hop)
            .WithMany();

        modelBuilder.Entity<Recipe>()
            .HasOne(r => r.Yeast)
            .WithMany()
            .HasForeignKey(r => r.YeastId);

        modelBuilder.Entity<Hop>().HasData(
            new Hop
            {
                Id = 1,
                Name = "Citra",
                AlphaAcid = 12,
            },
            new Hop
            {
                Id = 2,
                Name = "Mosaic",
                AlphaAcid = 13,
            },
            new Hop
            {
                Id = 3,
                Name = "Motueka",
                AlphaAcid = 14,
            },
            new Hop
            {
                Id = 4,
                Name = "Simcoe",
                AlphaAcid = 12,
            },
            new Hop
            {
                Id = 5,
                Name = "Cascade",
                AlphaAcid = 7,
            },
            new Hop
            {
                Id = 6,
                Name = "Amarillo",
                AlphaAcid = 9,
            },
            new Hop
            {
                Id = 7,
                Name = "Saaz",
                AlphaAcid = 3,
            },
            new Hop
            {
                Id = 8,
                Name = "Styrian Goldings",
                AlphaAcid = 5,
            }
        );

        modelBuilder.Entity<Fermentables>().HasData(
            new Fermentables
            {
                Id = 1,
                Name = "2-row",
                Type = "Grain",
                Origin = "UnitedStates",
                Color = 2.2,
                PotentialGravity = 1.036,
                MaxInBatch = 100
            },
            new Fermentables
            {
                Id = 2,
                Name = "Maris Otter",
                Type = "Grain",
                Origin = "UnitedKingdom",
                Color = 3.5,
                PotentialGravity = 1.038,
                MaxInBatch = 80
            },
            new Fermentables
            {
                Id = 3,
                Name = "Caramel/Crystal Malt - 20L",
                Type = "Grain",
                Origin = "UnitedStates",
                Color = 20,
                PotentialGravity = 1.035,
                MaxInBatch = 15
            },
            new Fermentables
            {
                Id = 4,
                Name = "Pilsner Malt",
                Type = "Grain",
                Origin = "Belgium",
                Color = 2,
                PotentialGravity = 1.036,
                MaxInBatch = 80
            },
            new Fermentables
            {
                Id = 5,
                Name = "Munich Malt",
                Type = "Grain",
                Origin = "Germany",
                Color = 8,
                PotentialGravity = 1.037,
                MaxInBatch = 15
            }
        );

        modelBuilder.Entity<Yeast>().HasData(
            new Yeast
            {
                Id = 1,
                Name = "Cal Ale",
                Type = "Ale",
                Code = "US-05",
                Form = "Dry",
                Flocculation = "Medium",
                Lab = "Fermentis"
            },
            new Yeast
            {
                Id = 2,
                Name = "American Ale",
                Type = "Ale",
                Code = "1056",
                Form = "Liquid",
                Flocculation = "Medium",
                Lab = "Wyeast"
            },
            new Yeast
            {
                Id = 3,
                Name = "English Ale",
                Type = "Ale",
                Code = "S-04",
                Form = "Dry",
                Flocculation = "Low",
                Lab = "Fermentis"
            }
        );

        modelBuilder.Entity<HopAddition>().HasData(
            new HopAddition
            {
                Id = 1,
                RecipeId = 1, // Replace with the appropriate RecipeId
                HopId = 1, // Replace with the appropriate HopId
                Use = "Boil",
                BoilTime = 60,
                DryHopDays = 0,
                Amount = 25,
                Form = "Pellet"
            },
            new HopAddition
            {
                Id = 2,
                RecipeId = 1, // Replace with the appropriate RecipeId
                HopId = 2, // Replace with the appropriate HopId
                Use = "Boil",
                BoilTime = 60,
                DryHopDays = 0,
                Amount = 25,
                Form = "Pellet"
            },
            new HopAddition
            {
                Id = 3,
                RecipeId = 1, // Replace with the appropriate RecipeId
                HopId = 3, // Replace with the appropriate HopId
                Use = "Boil",
                BoilTime = 60,
                DryHopDays = 0,
                Amount = 25,
                Form = "Pellet"
            },
            new HopAddition
            {
                Id = 4,
                RecipeId = 2,
                HopId = 1,
                Use = "Boil",
                BoilTime = 60,
                DryHopDays = 5,
                Amount = 20,
                Form = "Pellet"
            },
            new HopAddition
            {
                Id = 5,
                RecipeId = 2,
                HopId = 2,
                Use = "Boil",
                BoilTime = 15,
                DryHopDays = 3,
                Amount = 15,
                Form = "Pellet"
            },
            new HopAddition
            {
                Id = 6,
                RecipeId = 2,
                HopId = 5,
                Use = "Dry Hop",
                BoilTime = 0,
                DryHopDays = 7,
                Amount = 30,
                Form = "Pellet"
            },
            new HopAddition
            {
                Id = 7,
                RecipeId = 3,
                HopId = 7,
                Use = "Boil",
                BoilTime = 60,
                DryHopDays = 0,
                Amount = 20,
                Form = "Pellet"
            },
            new HopAddition
            {
                Id = 8,
                RecipeId = 3,
                HopId = 8,
                Use = "Boil",
                BoilTime = 15,
                DryHopDays = 0,
                Amount = 15,
                Form = "Pellet"
            }
        );

        modelBuilder.Entity<Recipe.FermentablePair>().HasData(
            new Recipe.FermentablePair
            {
                Id = 1,
                RecipeId = 1,
                FermentableId = 1,
                Weight = 10
            },
            new Recipe.FermentablePair
            {
                Id = 2,
                RecipeId = 2,
                FermentableId = 1,
                Weight = 8
            },
            new Recipe.FermentablePair
            {
                Id = 3,
                RecipeId = 2,
                FermentableId = 2,
                Weight = 4
            },
            new Recipe.FermentablePair
            {
                Id = 4,
                RecipeId = 3,
                FermentableId = 4,
                Weight = 7
            },
            new Recipe.FermentablePair
            {
                Id = 5,
                RecipeId = 3,
                FermentableId = 5,
                Weight = 5
            }
        );

        modelBuilder.Entity<Recipe>().HasData(
            new Recipe
            {
                Id = 1,
                Name = "Classic IPA",
                Style = "AmericanIPA",
                OriginalGravity = 1.065,
                FinalGravity = 1.010,
                IBU = 60,
                ABV = 7.2,
                YeastId = 1,
                YeastAmount = 1.0,
                YeastViability = 95,
                MashTemp = 152,

                WaterRatio = 1.25,
                AmountOfWater = 6.0,
                Color = 12.0
            },
            new Recipe
            {
                Id = 2,
                Name = "Hoppy Pale Ale",
                Style = "AmericanPaleAle",
                OriginalGravity = 1.052,
                FinalGravity = 1.012,
                IBU = 35,
                ABV = 5.2,
                YeastId = 2,
                YeastAmount = 1.0,
                YeastViability = 95,
                MashTemp = 150,
                WaterRatio = 1.2,
                AmountOfWater = 5.5,
                Color = 8.0
            },
            new Recipe
            {
                Id = 3,
                Name = "Belgian Dubbel",
                Style = "BelgianDubbel",
                OriginalGravity = 1.072,
                FinalGravity = 1.018,
                IBU = 25,
                ABV = 7.5,
                YeastId = 3,
                YeastAmount = 1.0,
                YeastViability = 90,
                MashTemp = 154,
                WaterRatio = 1.3,
                AmountOfWater = 6.5,
                Color = 18.0
            }
        );
    }
}