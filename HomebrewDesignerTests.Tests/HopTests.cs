using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;
using HomebrewDesigner.Core.ServiceContracts;
using HomebrewDesigner.Core.Services;
using HomebrewDesignerTests.FakeRepos;
using Xunit;


namespace HomebrewDesignerTests.Tests;

public class HopTests
{
    private readonly IHopService _hopService;
    
    public HopTests()
    {
        var fakeHopRepo = new FakeHopRepo();
        _hopService = new HopService(fakeHopRepo);
    }
    
    #region Add

    [Fact]
    public async Task  AddHop_NullHop()
    {
        //Arrange

        HopAddRequest? hopAddRequest = null;

        //Act
        await Assert.ThrowsAsync<ArgumentNullException>(() => _hopService.AddHopAsync(hopAddRequest));
    }

    [Fact]
    public async Task AddHop_ValidHop()
    {
        //Arrange
        HopAddRequest hop = new()
        {
            Id = 1,
            Name = "TestHop",
            AlphaAcid = 1.1,
        };
    
        HopResponse hopResponse = await _hopService.AddHopAsync(hop);
    
        List<HopResponse> hops = await _hopService.GetAllHopsAsync();
    
        //Assert
        Assert.True(hopResponse.Id == hop.Id);
        Assert.True(hops[0].Id == 1);
    }


 
   [Fact]
    public async Task AddHop_HopExists()
    {
        //Arrange
        HopAddRequest hop = new()
        {
            Id = 1,
            Name = "TestHop",
            AlphaAcid = 1.1,
        };
    
        await _hopService.AddHopAsync(hop);
    
        //Act
        await Assert.ThrowsAsync<ArgumentException>(() => _hopService.AddHopAsync(hop));
    }
    
    [Fact]
    public async Task AddHop_InvalidName()
    {
        //Arrange
        HopAddRequest hop = new()
        {
            Id = 1,
            Name = "",
            AlphaAcid = 1.1,
        };
    
        //Act
        await Assert.ThrowsAsync<ArgumentException>(() => _hopService.AddHopAsync(hop));
    }

    #endregion
    #region Update

    [Fact]
    public async Task UpdateHop_NullHop()
    {
        //Arrange
        HopUpdateRequest? hopUpdateRequest = null;

        //Act
       await  Assert.ThrowsAsync<ArgumentNullException>(() => _hopService.UpdateHopAsync(hopUpdateRequest));
    }

    [Fact]
    public async Task UpdateHop_InvalidHopId()
    {
        //Arrange
        HopUpdateRequest hopUpdateRequest = new()
        {
            Id = 500
        };
    
        //Act
        await Assert.ThrowsAsync<ArgumentException>(() => _hopService.UpdateHopAsync(hopUpdateRequest));
    }
    
    [Fact]
    public async Task UpdateHop_NullHopName()
    {
        //Arrange
        HopAddRequest hop = new()
        {
            Id = 1,
            Name = "TestHop",
            AlphaAcid = 1.1,
        };
    
        HopUpdateRequest hopUpdateRequest = new()
        {
            Id = 1,
            Name = null,
            AlphaAcid = 1.2,
        };
    
        await _hopService.AddHopAsync(hop);
    
        //Act
        await Assert.ThrowsAsync<ArgumentException>(() => _hopService.UpdateHopAsync(hopUpdateRequest));
    }

    [Fact]
    public async Task UpdateHop_ValidHop()
    {
        //Arrange
        HopAddRequest hop = new()
        {
            Id = 1,
            Name = "TestHop",
            AlphaAcid = 1.1,
        };

        HopUpdateRequest hopUpdateRequest = new()
        {
            Id = 1,
            Name = "TestHop",
            AlphaAcid = 1.2,
        };

        await _hopService.AddHopAsync(hop);

        HopResponse hopResponse = await _hopService.UpdateHopAsync(hopUpdateRequest);

        //Assert
        Assert.True(hopResponse.Id == hopUpdateRequest.Id);
        Assert.True(hopResponse.Name == hopUpdateRequest.Name);
        Assert.True(hopResponse.AlphaAcid == hopUpdateRequest.AlphaAcid);
    }

    #endregion
    
    #region GetById

    [Fact]
    public async Task GetHopById_InvalidHopId()
    {
        //Arrange
        HopAddRequest hopAddRequest = new()
        {
            Id = 1,
            Name = "TestHop",
            AlphaAcid = 1.1,
        };
        
        HopResponse addedHop =  await _hopService.AddHopAsync(hopAddRequest);
        
        //Act
        await Assert.ThrowsAsync<ArgumentException>(() => _hopService.GetHopByIdAsync(500));
    }
    
    [Fact]
    public async Task GetHopById_ValidHopId()
    {
        //Arrange
        HopAddRequest hopAddRequest = new()
        {
            Id = 1,
            Name = "TestHop",
            AlphaAcid = 1.1,
        };
        
        HopResponse hop = await _hopService.AddHopAsync(hopAddRequest);
        
        Assert.Equal("TestHop", hop.Name);
    }
    
    [Fact]
    public async Task GetHopById_IdLessThanZero()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _hopService.GetHopByIdAsync(-1));
    }
    

    #endregion
    #region GetAllHops
    
    [Fact]
    public async Task GetAllHops()
    {
        //Arrange
        HopAddRequest hopAddRequest = new()
        {
            Id = 1,
            Name = "TestHop",
            AlphaAcid = 1.1,
        };
        
        HopAddRequest hopAddRequest2 = new()
        {
            Id = 2,
            Name = "TestHop2",
            AlphaAcid = 1.1,
        };
        
        await _hopService.AddHopAsync(hopAddRequest);
        await _hopService.AddHopAsync(hopAddRequest2);
        
        //Act
        List<HopResponse> hops = await _hopService.GetAllHopsAsync();
        
        //Assert
        Assert.True(hops.Count == 2);
    }
    
    #endregion
    
    #region GetFilteredHops
    
    [Fact]
    public async Task GetFilteredHops_ValidTest()
    {
        //Arrange
        HopAddRequest hopAddRequest = new()
        {
            Id = 1,
            Name = "TestHop",
            AlphaAcid = 1.1,
        };
        
        HopAddRequest hopAddRequest2 = new()
        {
            Id = 2,
            Name = "Mosaic",
            AlphaAcid = 1.1,
        };
        
        await _hopService.AddHopAsync(hopAddRequest);
        await _hopService.AddHopAsync(hopAddRequest2);
        
        //Act
        List<HopResponse> hops = await _hopService.GetFilteredHopsAsync("name", "TestHop");
        
        //Assert
        Assert.True(hops.Count == 1);
    }
    
    [Fact]
    public async Task GetFilteredHops_InvalidSearch()
    {
        //Arrange
        HopAddRequest hopAddRequest = new()
        {
            Id = 1,
            Name = "TestHop",
            AlphaAcid = 1.1,
        };
        
        HopAddRequest hopAddRequest2 = new()
        {
            Id = 2,
            Name = "Mosaic",
            AlphaAcid = 1.1,
        };
        
        await _hopService.AddHopAsync(hopAddRequest);
        await _hopService.AddHopAsync(hopAddRequest2);
        
        //Act
        List<HopResponse> hops = await _hopService.GetFilteredHopsAsync("name", "Citra");
        
        //Assert
        Assert.True(hops.Count == 2);
    }
    
    [Fact]
    public async Task GetFilteredHops_InvalidProperty()
    {
        //Arrange
        HopAddRequest hopAddRequest = new()
        {
            Id = 1,
            Name = "TestHop",
            AlphaAcid = 1.1,
        };
        
        HopAddRequest hopAddRequest2 = new()
        {
            Id = 2,
            Name = "Mosaic",
            AlphaAcid = 1.1,
        };
        
        await _hopService.AddHopAsync(hopAddRequest);
        await _hopService.AddHopAsync(hopAddRequest2);
        
        //Act
        await Assert.ThrowsAsync<NullReferenceException>(() => _hopService.GetFilteredHopsAsync("invalid", "Citra"));
    }
    
    #endregion
}