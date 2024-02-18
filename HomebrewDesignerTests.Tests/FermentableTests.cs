using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;
using HomebrewDesigner.Core.Enums;
using HomebrewDesigner.Core.ServiceContracts;
using HomebrewDesigner.Core.Services;
using HomebrewDesignerTests.FakeRepos;
using Xunit;
using Xunit.Abstractions;

namespace HomebrewDesignerTests.Tests;

public class FermentableTests
{
    private readonly IService<FermentableAddRequest, FermentableUpdateRequest, FermentableResponse> _fermentableService;

    
    public FermentableTests()
    {
        var fakeFermentableRepo = new FakeFermentableRepo();
        _fermentableService = new FermentableService(fakeFermentableRepo);
    }
    
    
    #region AddFermentable
    
    [Fact]
    public async Task AddGrain_NullGrain()
    {
        //arrange
        FermentableAddRequest request = null;

        //Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _fermentableService.AddAsync(request));
    }

    [Fact]
    public async Task AddGrain_InvalidName()
    {
        FermentableAddRequest fermentableAddRequest = new()
        {
            Id = 1,
            Name = "",
            Color = 3.5,
            PotentialGravity = 1.025,
            MaxInBatch = 25,
            Origin = Enum.Parse<FermentableOriginEnum>("Germany"),
            Type = Enum.Parse<FermentableTypeEnum>("Grain")
        };

        await Assert.ThrowsAsync<ArgumentException>(() => _fermentableService.AddAsync(fermentableAddRequest));
    }

    [Fact]
    public async Task AddGrain_GrainExists()
    {
        FermentableAddRequest fermentableAddRequest = new()
        {
            Id = 1,
            Name = "2-row",
            Color = 3.5,
            PotentialGravity = 1.025,
            MaxInBatch = 25,
            Origin = Enum.Parse<FermentableOriginEnum>("Germany"),
            Type = Enum.Parse<FermentableTypeEnum>("Grain")
        };

        await _fermentableService.AddAsync(fermentableAddRequest);

        await Assert.ThrowsAsync<ArgumentException>(() => _fermentableService.AddAsync(fermentableAddRequest));

    }
    
    [Fact]
    public async Task AddGrain_InvalidColor()
    {
        FermentableAddRequest fermentableAddRequest = new()
        {
            Id = 1,
            Name = "Pilsner",
            Color = 0,
            PotentialGravity = 1.025
        };

        await Assert.ThrowsAsync<ArgumentException>(() => _fermentableService.AddAsync(fermentableAddRequest));
    }
    
    [Fact]
    public async Task AddGrain_InvalidPotentialGravity()
    {
        FermentableAddRequest fermentableAddRequest = new()
        {
            Id = 1,
            Name = "Pilsner",
            Color = 3.5,
            PotentialGravity = .2
        };

        await Assert.ThrowsAsync<ArgumentException>(() => _fermentableService.AddAsync(fermentableAddRequest));
    }

    [Fact]
    public async Task AddGrain_ValidGrain()
    {
        FermentableAddRequest fermentableAddRequest = new()
        {
            Id = 1,
            Name = "2-row",
            Color = 3.5,
            PotentialGravity = 1.025,
            MaxInBatch = 25,
            Origin = Enum.Parse<FermentableOriginEnum>("Germany"),
            Type = Enum.Parse<FermentableTypeEnum>("Grain")
        }; 
        
        FermentableAddRequest fermentableAddRequest2 = new()
        {
            Id = 2,
            Name = "6-row",
            Color = 3.5,
            PotentialGravity = 1.025,
            MaxInBatch = 25,
            Origin = Enum.Parse<FermentableOriginEnum>("Germany"),
            Type = Enum.Parse<FermentableTypeEnum>("Grain")
        };

        await _fermentableService.AddAsync(fermentableAddRequest);
       await _fermentableService.AddAsync(fermentableAddRequest2);
        
        FermentableResponse fermentableResponse = await _fermentableService.GetByIdAsync(1);
        FermentableResponse fermentableResponse2 = await _fermentableService.GetByIdAsync(2);
        
        Assert.Equal(fermentableResponse.Id, fermentableAddRequest.Id);
        Assert.Equal(fermentableResponse2.Id, fermentableAddRequest2.Id);
        List<FermentableResponse> fermentables = await _fermentableService.GetAllAsync();
        Assert.Equal(2, fermentables.Count);
    }
    
    #endregion
   
    #region GetById
    
    [Fact]
    public async Task GetGrainById_IdLessThanZero()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _fermentableService.GetByIdAsync(-1));
    }
    
    [Fact]
    public async Task GetGrainById_ValidId()
    {
        FermentableAddRequest fermentableAddRequest = new()
        {
            Id = 1,
            Name = "2-row",
            Color = 3.5,
            PotentialGravity = 1.025,
            MaxInBatch = 25,
            Origin = Enum.Parse<FermentableOriginEnum>("Germany"),
            Type = Enum.Parse<FermentableTypeEnum>("Grain")
        };
        
        await _fermentableService.AddAsync(fermentableAddRequest);
        
        FermentableResponse fermentableResponse = await _fermentableService.GetByIdAsync(1);
        
        Assert.True(fermentableResponse.Id == fermentableAddRequest.Id);
    }
    
    [Fact]
    public async Task GetGrainById_GrainDoesNotExist()
    {
        FermentableAddRequest fermentableAddRequest = new()
        {
            Id = 1,
            Name = "2-row",
            Color = 3.5,
            PotentialGravity = 1.025,
            MaxInBatch = 25,
            Origin = Enum.Parse<FermentableOriginEnum>("Germany"),
            Type = Enum.Parse<FermentableTypeEnum>("Grain")
        };
        
        await _fermentableService.AddAsync(fermentableAddRequest);
        
        await Assert.ThrowsAsync<ArgumentException>(() => _fermentableService.GetByIdAsync(2));
    }
    
    #endregion

    #region GetAllFermentables

    [Fact]
    public async Task GetAllFermentablesTest()
    {
        FermentableAddRequest fermentableAddRequest = new()
        {
            Id = 1,
            Name = "2-row",
            Color = 3.5,
            PotentialGravity = 1.025,
            MaxInBatch = 25,
            Origin = Enum.Parse<FermentableOriginEnum>("Germany"),
            Type = Enum.Parse<FermentableTypeEnum>("Grain")
        };
        
        FermentableAddRequest fermentableAddRequest2 = new()
        {
            Id = 2,
            Name = "2-row2",
            Color = 3.5,
            PotentialGravity = 1.025,
            MaxInBatch = 25,
            Origin = Enum.Parse<FermentableOriginEnum>("Germany"),
            Type = Enum.Parse<FermentableTypeEnum>("Grain")
        };
        
        await _fermentableService.AddAsync(fermentableAddRequest);
        await _fermentableService.AddAsync(fermentableAddRequest2);
        
        List<FermentableResponse> grainResponses = await _fermentableService.GetAllAsync();
        
        Assert.True(grainResponses.Count == 2);
    }
    

    #endregion
    
    #region UpdateFermentable
    
    [Fact]
    public async Task UpdateGrain_NullGrain()
    {
        FermentableUpdateRequest?  grainUpdateRequest = null;
        
        await Assert.ThrowsAsync<ArgumentException>(() => _fermentableService.UpdateAsync(grainUpdateRequest));
        
    }
    
    [Fact]
    public async Task UpdateGrain_ValidGrain()
    {
        FermentableAddRequest? grainAddRequest = new()
        {
            Id = 1,
            Name = "2-row",
            Color = 3.5,
            PotentialGravity = 1.025,
            MaxInBatch = 25,
            Origin = Enum.Parse<FermentableOriginEnum>("Germany"),
            Type = Enum.Parse<FermentableTypeEnum>("Grain")
        };
        
        await _fermentableService.AddAsync(grainAddRequest);
        
        FermentableUpdateRequest? grainUpdateRequest = new()
        {
            Id = 1,
            Name = "6-row",
            Color = 3.5,
            PotentialGravity = 1.025,
            MaxInBatch = 25,
            Origin = Enum.Parse<FermentableOriginEnum>("Germany"),
            Type = Enum.Parse<FermentableTypeEnum>("Grain")
        };
        
        FermentableResponse fermentableResponse = await _fermentableService.UpdateAsync(grainUpdateRequest);
        
        Assert.True(fermentableResponse.Name == grainUpdateRequest.Name);
    }
    
    [Fact]
    public async Task UpdateGrain_InvalidId()
    {
        FermentableAddRequest? grainAddRequest = new()
        {
            Id = 1,
            Name = "2-row",
            Color = 3.5,
            PotentialGravity = 1.025,
            MaxInBatch = 25,
            Origin = Enum.Parse<FermentableOriginEnum>("Germany"),
            Type = Enum.Parse<FermentableTypeEnum>("Grain")
        };
        
        _fermentableService.AddAsync(grainAddRequest);
        
        FermentableUpdateRequest? grainUpdateRequest = new()
        {
            Id = 2,
            Name = "2-row",
            Color = 3.5,
            PotentialGravity = 1.025,
            MaxInBatch = 25,
            Origin = Enum.Parse<FermentableOriginEnum>("Germany"),
            Type = Enum.Parse<FermentableTypeEnum>("Grain")
        };
        
        await Assert.ThrowsAsync<ArgumentException>(() => _fermentableService.UpdateAsync(grainUpdateRequest));
    }
    
    [Fact]
    public async Task UpdateGrain_InvalidName()
    {
        FermentableAddRequest? grainAddRequest = new()
        {
            Id = 1,
            Name = "2-row",
            Color = 3.5,
            PotentialGravity = 1.025,
            MaxInBatch = 25,
            Origin = Enum.Parse<FermentableOriginEnum>("Germany"),
            Type = Enum.Parse<FermentableTypeEnum>("Grain")
        };
        
        await _fermentableService.AddAsync(grainAddRequest);
        
        FermentableUpdateRequest? grainUpdateRequest = new()
        {
            Id = 1,
            Name = "",
            Color = 3.5,
            PotentialGravity = 1.025,
            MaxInBatch = 25,
            Origin = Enum.Parse<FermentableOriginEnum>("Germany"),
            Type = Enum.Parse<FermentableTypeEnum>("Grain")
        };
        
        await Assert.ThrowsAsync<ArgumentException>(() => _fermentableService.UpdateAsync(grainUpdateRequest));
    }
    
    [Fact]
    public async Task UpdateGrain_GrainDoesNotExist()
    {
        FermentableUpdateRequest? grainUpdateRequest = new()
        {
            Id = 1,
            Name = "Pilsner2",
            Color = 3.5,
            PotentialGravity = 1.025
        };
        
        await Assert.ThrowsAsync<ArgumentException>(() => _fermentableService.UpdateAsync(grainUpdateRequest));
    }
    
    
    
    [Fact]
    public async Task UpdateGrain_InvalidPotentialGravity()
    {
        FermentableAddRequest? grainAddRequest = new()
        {
            Id = 1,
            Name = "2-row",
            Color = 3.5,
            PotentialGravity = 1.025,
            MaxInBatch = 25,
            Origin = Enum.Parse<FermentableOriginEnum>("Germany"),
            Type = Enum.Parse<FermentableTypeEnum>("Grain")
        };
        
        await _fermentableService.AddAsync(grainAddRequest);
        
        FermentableUpdateRequest? grainUpdateRequest = new()
        {
            Id = 1,
            Name = "2-row",
            Color = 3.5,
            PotentialGravity = 15,
            MaxInBatch = 25,
            Origin = Enum.Parse<FermentableOriginEnum>("Germany"),
            Type = Enum.Parse<FermentableTypeEnum>("Grain")
        };
        
        await Assert.ThrowsAsync<ArgumentException>(() => _fermentableService.UpdateAsync(grainUpdateRequest));
    }
    
    #endregion

    #region GetFilteredFermentables

    [Fact]
    public async Task GetFilteredFermentables_InvalidSearchBy()
    {
        FermentableAddRequest fermentableAddRequest = new()
        {
            Id = 1,
            Name = "2-row",
            Color = 3.5,
            PotentialGravity = 1.025,
            MaxInBatch = 25,
            Origin = Enum.Parse<FermentableOriginEnum>("Germany"),
            Type = Enum.Parse<FermentableTypeEnum>("Grain")
        }; 
        
        FermentableAddRequest fermentableAddRequest2 = new()
        {
            Id = 2,
            Name = "6-row",
            Color = 3.5,
            PotentialGravity = 1.025,
            MaxInBatch = 25,
            Origin = Enum.Parse<FermentableOriginEnum>("Germany"),
            Type = Enum.Parse<FermentableTypeEnum>("Grain")
        };

        await _fermentableService.AddAsync(fermentableAddRequest);
       await  _fermentableService.AddAsync(fermentableAddRequest2);
        
        await Assert.ThrowsAsync<NullReferenceException>(() => _fermentableService.GetFilteredAsync("Invalid", "2-row"));
    }
    
    [Fact]
    public async Task GetFilteredFermentables_InvalidSearchTerm()
    {
        FermentableAddRequest fermentableAddRequest = new()
        {
            Id = 1,
            Name = "2-row",
            Color = 2.2,
            PotentialGravity = 1.036,
            MaxInBatch = 100,
            Origin = Enum.Parse<FermentableOriginEnum>("UnitedStates"),
            Type = Enum.Parse<FermentableTypeEnum>("Grain")
        }; 
        
        FermentableAddRequest fermentableAddRequest2 = new()
        {
            Id = 2,
            Name = "6-row",
            Color = 3.5,
            PotentialGravity = 1.025,
            MaxInBatch = 25,
            Origin = Enum.Parse<FermentableOriginEnum>("Germany"),
            Type = Enum.Parse<FermentableTypeEnum>("Grain")
        };

        await _fermentableService.AddAsync(fermentableAddRequest);
        await _fermentableService.AddAsync(fermentableAddRequest2);

        List<FermentableResponse> fermentables = await _fermentableService.GetAllAsync();
        Assert.Equal(2, fermentables.Count());
    }
    
    [Fact]
    public async Task GetFilteredFermentables_ValidSearchByAndSearchTerm()
    {
        FermentableAddRequest fermentableAddRequest = new()
        {
            Id = 1,
            Name = "2-row",
            Color = 2.2,
            PotentialGravity = 1.036,
            MaxInBatch = 100,
            Origin = Enum.Parse<FermentableOriginEnum>("UnitedStates"),
            Type = Enum.Parse<FermentableTypeEnum>("Grain")
        }; 
        
        FermentableAddRequest fermentableAddRequest2 = new()
        {
            Id = 2,
            Name = "6-row",
            Color = 3.5,
            PotentialGravity = 1.025,
            MaxInBatch = 25,
            Origin = Enum.Parse<FermentableOriginEnum>("Germany"),
            Type = Enum.Parse<FermentableTypeEnum>("Grain")
        };

        await _fermentableService.AddAsync(fermentableAddRequest);
        await _fermentableService.AddAsync(fermentableAddRequest2);
        
        List<FermentableResponse> fermentableResponses = await _fermentableService.GetFilteredAsync("name", "2-row");
        
        Assert.True(fermentableResponses.Count == 1);
    }

    #endregion
}