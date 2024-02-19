using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;
using HomebrewDesigner.Core.Enums;
using HomebrewDesigner.Core.RepositoryContracts;
using HomebrewDesigner.Core.ServiceContracts;
using HomebrewDesigner.Core.Services;
using HomebrewDesignerTests.FakeRepos;
using Xunit;

namespace HomebrewDesignerTests.Tests;

public class RecipeTests
{
    private readonly IRecipeService _recipes;
    private readonly IRepository<Hop, HopUpdateRequest> _hopRepo;
    private readonly IRepository<Yeast, YeastUpdateRequest> _yeastRepo;
    private readonly IRepository<Fermentables, FermentableUpdateRequest> _fermentableRepo;

    public RecipeTests()
    {
        _hopRepo = new FakeHopRepo();
        _yeastRepo = new FakeYeastRepo();
        _fermentableRepo = new FakeFermentableRepo();
        var fakeRecipeRepo = new FakeRecipeRepo();
        _recipes = new RecipeService(fakeRecipeRepo, _hopRepo, _fermentableRepo, _yeastRepo);
        
    }

    #region SetupMethods

   
    public void AddHops()
    {
        _hopRepo.AddAsync(new Hop()
        {
            Id = 1,
            Name = "Hop1",
            AlphaAcid = 1.1,
        });
        _hopRepo.AddAsync(new Hop()
        {
            Id = 2,
            Name = "Hop2",
            AlphaAcid = 2.2,
        });
        _hopRepo.AddAsync(new Hop()
        {
            Id = 3,
            Name = "Hop3",
            AlphaAcid = 3.3,
        });
    }

    public void AddGrains()
    {
        _fermentableRepo.AddAsync(new Fermentables()
        {
            Id = 1,
            Name = "Grain1",
            Color = 1.1,
        });
        _fermentableRepo.AddAsync(new Fermentables()
        {
            Id = 2,
            Name = "Grain2",
            Color = 2.2,
        });
        _fermentableRepo.AddAsync(new Fermentables()
        {
            Id = 3,
            Name = "Grain3",
            Color = 3.3,
        });
    }

    public void AddYeast()
    {
        _yeastRepo.AddAsync(new Yeast()
        {
            Id = 1,
            Name = "Yeast1"
        });
        _yeastRepo.AddAsync(new Yeast()
        {
            Id = 2,
            Name = "Yeast2"
        });
        _yeastRepo.AddAsync(new Yeast()
        {
            Id = 3,
            Name = "Yeast3"
        });
    }

    public List<RecipeResponse.FermentablePair> CreateFermentablePair()
    {
        return new List<RecipeResponse.FermentablePair>()
        {
            new RecipeResponse.FermentablePair()
            {
                Fermentable = _fermentableRepo.GetByIdAsync(1).Result,
                Weight = 10
            }
        };
    }

    public List<RecipeAddRequest.FermentablePair> CreateAddFermentablePair()
    {
        return new List<RecipeAddRequest.FermentablePair>()
        {
            new RecipeAddRequest.FermentablePair()
            {
                Fermentable = _fermentableRepo.GetByIdAsync(1).Result,
                Weight = 10
            }
        };
    }

    public List<HopAddition> CreateHopAdditions()
    {
        AddHops();
        List<HopAddition> hopAdditions = new();

        hopAdditions.Add(new HopAddition()
        {
            Hop = _hopRepo.GetByIdAsync(1).Result,
            Use = AdditionEnum.Boil.ToString(),
            Form = HopFormEnum.Pellet.ToString(),
            Amount = 1.1,
            BoilTime = 90,
            DryHopDays = 0
        });

        hopAdditions.Add(new HopAddition()
        {
            Hop = _hopRepo.GetByIdAsync(2).Result,
            Use = AdditionEnum.Boil.ToString(),
            Form = HopFormEnum.Pellet.ToString(),
            Amount = 2.2,
            BoilTime = 60,
            DryHopDays = 0
        });

        hopAdditions.Add(new HopAddition()
        {
            Hop = _hopRepo.GetByIdAsync(3).Result,
            Use = AdditionEnum.Whirlpool.ToString(),
            Form = HopFormEnum.Pellet.ToString(),
            Amount = 3.3,
            BoilTime = 30,
            DryHopDays = 0
        });

        return hopAdditions;
    }


    public RecipeResponse CreateValidRecipe()
    {
        return new RecipeResponse()
        {
            Id = 1,
            Name = "Test Recipe",
            Style = "American IPA",
            OriginalGravity = 1.056,
            FinalGravity = 1.010,
            IBU = 30,
            ABV = 6.5,
            Hops = CreateHopAdditions(),
            Yeast = _yeastRepo.GetByIdAsync(1).Result,
            YeastAmount = 100,
            YeastViability = 95,
            MashTemp = 152,
            Maltbill = CreateFermentablePair(),
            WaterRatio = 1.25,
            AmountOfWater = 12,
            Color = 10,
            YeastId = 1
        };
    }

    public RecipeAddRequest CreateAddRequest()
    {
        return new RecipeAddRequest()
        {
            Id = 1,
            Name = "Test Recipe",
            Style = "AmericanIPA",
            OriginalGravity = 1.056,
            FinalGravity = 1.010,
            IBU = 30,
            ABV = 6.5,
            Hops = CreateHopAdditions(),
            Yeast = _yeastRepo.GetByIdAsync(1).Result,
            YeastAmount = 100,
            YeastViability = 95,
            MashTemp = 152,
            MaltBill = CreateAddFermentablePair(),
            WaterRatio = 1.25,
            AmountOfWater = 12,
            Color = 10,
            YeastId = 1
        };
    }

    #endregion

    #region AddRecipeTests

    [Fact]
    public async Task AddRecipe_ValidRecipe()
    {
        AddGrains();
        AddGrains();
        AddYeast();
        RecipeAddRequest recipe = CreateAddRequest();

        //Act
        RecipeResponse response = await _recipes.AddAsync(recipe);
        List<RecipeResponse> list = await _recipes.GetAllAsync();

        Assert.True(response.Id == recipe.Id);
        Assert.True(list[0].Id == recipe.Id);
        Assert.True(list.Count == 1);
    }


    [Fact]
    public async Task AddRecipe_RecipeAlreadyExists()
    {
        //Arrange
        AddGrains();
        AddGrains();
        AddYeast();
        RecipeAddRequest recipe = CreateAddRequest();
        RecipeAddRequest recipe2 = CreateAddRequest();

        //Act
        await _recipes.AddAsync(recipe);

        //Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _recipes.AddAsync(recipe2));
    }

    #endregion

    #region GetAllRecipesTests

    [Fact]
    public async Task GetAllRecipes_NoRecipes()
    {
        //Arrange

        //Act
        List<RecipeResponse> list = await _recipes.GetAllAsync();

        //Assert
        Assert.True(list.Count == 0);
    }

    [Fact]
    public async Task GetAllRecipes()
    {
        AddGrains();
        AddGrains();
        AddYeast();
        //Arrange
        RecipeAddRequest recipe = CreateAddRequest();
        await _recipes.AddAsync(recipe);
        RecipeAddRequest recipe2 = CreateAddRequest();
        recipe2.Name = "TestRecipe2";
        await _recipes.AddAsync(recipe2);
        RecipeAddRequest recipe3 = CreateAddRequest();
        recipe3.Name = "TestRecipe3";
        await _recipes.AddAsync(recipe3);

        //Act
        List<RecipeResponse> responses = await _recipes.GetAllAsync();

        Assert.True(responses.Count == 3);
    }

    #endregion

    #region GetByIdTests

    [Fact]
    public async Task GetRecipeById_ValidId()
    {
        //Arrange
        AddGrains();
        AddGrains();
        AddYeast();
        RecipeAddRequest recipe = CreateAddRequest();
        await _recipes.AddAsync(recipe);

        //Act
        RecipeResponse response = await _recipes.GetByIdAsync(1);

        //Assert
        Assert.True(response.Id == recipe.Id);
    }


    [Fact]
    public async Task GetRecipeById_IdNotFound()
    {
        //Arrange
        AddGrains();
        AddGrains();
        AddYeast();
        RecipeAddRequest recipe = CreateAddRequest();
        await _recipes.AddAsync(recipe);

        //Assert
        await Assert.ThrowsAnyAsync<ArgumentException>(() => _recipes.GetByIdAsync(2));
    }

    [Fact]
    public async Task GetRecipeById_IdLessThanZero()
    {
        //Arrange
        AddGrains();
        AddGrains();
        AddYeast();
        RecipeAddRequest recipe = CreateAddRequest();
        await _recipes.AddAsync(recipe);

        //Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _recipes.GetByIdAsync(-1));
    }

    #endregion

    #region GetLatestEntryTest
    
    [Fact]
    public async Task GetLastThreeEntries_NoRecipes()
    {
        //Arrange

        //Act
        IEnumerable<RecipeResponse> list = await _recipes.GetLastThreeEntriesAsync();

        //Assert
        Assert.True(!list.Any());
    }
    
    [Fact]
    public async Task GetLastThreeEntries()
    {
        AddGrains();
        AddHops();
        AddYeast();
        //Arrange
        RecipeAddRequest recipe = CreateAddRequest();
        await _recipes.AddAsync(recipe);
        RecipeAddRequest recipe2 = CreateAddRequest();
        recipe2.Name = "TestRecipe2";
        await _recipes.AddAsync(recipe2);
        RecipeAddRequest recipe3 = CreateAddRequest();
        recipe3.Name = "TestRecipe3";
        await _recipes.AddAsync(recipe3);

        //Act
        IEnumerable<RecipeResponse> responses = await _recipes.GetLastThreeEntriesAsync();

        Assert.True(responses.Count() == 3);
    }

    #endregion

    #region UpdateRecipeTests

    [Fact]
    public async Task UpdateRecipe_ValidRecipe()
    {
        AddGrains();
        AddGrains();
        AddYeast();

        RecipeAddRequest recipe = CreateAddRequest();
        RecipeResponse recipeToUpdate = await _recipes.AddAsync(recipe);


        RecipeResponse response = await _recipes.UpdateAsync(recipeToUpdate.ToRecipeUpdateRequest());

        Assert.Equal("Test Recipe", response.Name);
    }

    #endregion
}