using HomebrewDesigner.Core.DTO;
using HomebrewDesigner.Core.Enums;
using HomebrewDesigner.Core.Services;
using HomebrewDesigner.Core.ServiceContracts;
using HomebrewDesignerTests.FakeRepos;
using Xunit;
using Xunit.Abstractions;

namespace HomebrewDesignerTests.Tests;

public class YeastTests
{
    private readonly IService<YeastAddRequest, YeastUpdateRequest, YeastResponse> _yeastService;

    public YeastTests()
    {
        var fakeYeastRepo = new FakeYeastRepo();
        _yeastService = new YeastService(fakeYeastRepo);
    }

    #region Add

    [Fact]
    public async Task AddYeast_NullYeast()
    {
        //Arrange
        YeastAddRequest? yeastAddRequest = null;

        //Act
        await Assert.ThrowsAsync<ArgumentException>(() => _yeastService.AddAsync(yeastAddRequest));
    }

    [Fact]
    public async Task AddYeast_ValidYeast()
    {
        YeastAddRequest? yeastAddRequest = new()
        {
            Id = 1,
            Name = "TestYeast",
            Lab = "TestLab",
            Code = "TestCode",
            Type = Enum.Parse<YeastTypeEnum>("Ale"),
            Form = Enum.Parse<YeastFormEnum>("Liquid"),
            Flocculation = Enum.Parse<YeastFlocEnum>("Low"),
        };

        YeastResponse yeastResponse = await _yeastService.AddAsync(yeastAddRequest);

        YeastResponse yeastToCompare = await _yeastService.GetByIdAsync(1);

        Assert.True(yeastResponse.Id == yeastToCompare.Id);
    }

    [Fact]
    public async Task AddYeast_YeastExists()
    {
        YeastAddRequest? yeastAddRequest = new()
        {
            Id = 1,
            Name = "TestYeast",
            Lab = "TestLab",
            Code = "TestCode",
            Type = Enum.Parse<YeastTypeEnum>("Ale"),
            Form = Enum.Parse<YeastFormEnum>("Liquid"),
            Flocculation = Enum.Parse<YeastFlocEnum>("Low"),
        };

        YeastResponse yeastResponse = await _yeastService.AddAsync(yeastAddRequest);

        await Assert.ThrowsAsync<ArgumentException>(() => _yeastService.AddAsync(yeastAddRequest));
    }

    [Fact]
    public async Task AddYeast_InvalidName()
    {
        YeastAddRequest? yeastAddRequest = new()
        {
            Id = 1,
            Name = "",
            Lab = "TestLab",
            Code = "TestCode",
            Type = Enum.Parse<YeastTypeEnum>("Ale"),
            Form = Enum.Parse<YeastFormEnum>("Liquid"),
            Flocculation = Enum.Parse<YeastFlocEnum>("Low"),
        };

        await Assert.ThrowsAsync<ArgumentException>(() => _yeastService.AddAsync(yeastAddRequest));
    }

    #endregion

    #region GetById

    [Fact]
    public async Task GetYeastById_InvalidId()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _yeastService.GetByIdAsync(-1));
    }

    [Fact]
    public async Task GetYeastById_ValidId()
    {
        YeastAddRequest? yeastAddRequest = new()
        {
            Id = 1,
            Name = "TestYeast",
            Lab = "TestLab",
            Code = "TestCode",
            Type = Enum.Parse<YeastTypeEnum>("Ale"),
            Form = Enum.Parse<YeastFormEnum>("Liquid"),
            Flocculation = Enum.Parse<YeastFlocEnum>("Low"),
        };

        await _yeastService.AddAsync(yeastAddRequest);

        YeastResponse yeastResponse = await _yeastService.GetByIdAsync(1);

        Assert.True(yeastResponse.Id == yeastAddRequest.Id);
    }

    #endregion

    #region GetallYeast

    [Fact]
    public async Task GetAllYeast()
    {
        YeastAddRequest? yeastAddRequest = new()
        {
            Id = 1,
            Name = "TestYeast",
            Lab = "TestLab",
            Code = "TestCode",
            Type = Enum.Parse<YeastTypeEnum>("Ale"),
            Form = Enum.Parse<YeastFormEnum>("Liquid"),
            Flocculation = Enum.Parse<YeastFlocEnum>("Low"),
        };

        YeastAddRequest? yeastAddRequest2 = new()
        {
            Id = 2,
            Name = "TestYeast2",
            Lab = "TestLab",
            Code = "TestCode",
            Type = Enum.Parse<YeastTypeEnum>("Ale"),
            Form = Enum.Parse<YeastFormEnum>("Liquid"),
            Flocculation = Enum.Parse<YeastFlocEnum>("Low"),
        };

        await _yeastService.AddAsync(yeastAddRequest);
        await _yeastService.AddAsync(yeastAddRequest2);

        List<YeastResponse> yeastResponses = await _yeastService.GetAllAsync();

        Assert.True(yeastResponses.Count == 2);
    }

    #endregion

    #region UpdateYeast

    [Fact]
    public async Task UpdateYeast_NullYeast()
    {
        YeastUpdateRequest? yeastUpdateRequest = null;

        await Assert.ThrowsAsync<ArgumentException>(() => _yeastService.UpdateAsync(yeastUpdateRequest));
    }

    [Fact]
    public async Task UpdateYeast_ValidYeast()
    {
        YeastAddRequest? yeastAddRequest = new()
        {
            Id = 1,
            Name = "TestYeast",
            Lab = "TestLab",
            Code = "TestCode",
            Type = Enum.Parse<YeastTypeEnum>("Ale"),
            Form = Enum.Parse<YeastFormEnum>("Liquid"),
            Flocculation = Enum.Parse<YeastFlocEnum>("Low"),
        };

        await _yeastService.AddAsync(yeastAddRequest);

        YeastUpdateRequest? yeastUpdateRequest = new()
        {
            Id = 1,
            Name = "TestYeast2",
            Lab = "TestLab2",
            Code = "TestCode2",
            Type = Enum.Parse<YeastTypeEnum>("Ale"),
            Form = Enum.Parse<YeastFormEnum>("Liquid"),
            Flocculation = Enum.Parse<YeastFlocEnum>("Low")
        };

        YeastResponse yeastResponse = await _yeastService.UpdateAsync(yeastUpdateRequest);

        Assert.True(yeastResponse.Name == yeastUpdateRequest.Name);
    }

    [Fact]
    public async Task UpdateYeast_InvalidId()
    {
        YeastAddRequest yeastAddRequest = new()
        {
            Id = 1,
            Name = "TestYeast",
            Lab = "TestLab",
            Code = "TestCode",
            Type = Enum.Parse<YeastTypeEnum>("Ale"),
            Form = Enum.Parse<YeastFormEnum>("Liquid"),
            Flocculation = Enum.Parse<YeastFlocEnum>("Low")
        };

        await _yeastService.AddAsync(yeastAddRequest);

        YeastUpdateRequest yeastUpdateRequest = new()
        {
            Id = 2,
            Name = "TestYeast2",
            Lab = "TestLab2",
            Code = "TestCode2",
            Type = Enum.Parse<YeastTypeEnum>("Ale"),
            Form = Enum.Parse<YeastFormEnum>("Liquid"),
            Flocculation = Enum.Parse<YeastFlocEnum>("Low")
        };

        await Assert.ThrowsAsync<ArgumentException>(() => _yeastService.UpdateAsync(yeastUpdateRequest));
    }

    [Fact]
    public async Task UpdateYeast_InvalidName()
    {
        YeastAddRequest yeastAddRequest = new()
        {
            Id = 1,
            Name = "TestYeast",
            Lab = "TestLab",
            Code = "TestCode",
            Type = Enum.Parse<YeastTypeEnum>("Ale"),
            Form = Enum.Parse<YeastFormEnum>("Liquid"),
            Flocculation = Enum.Parse<YeastFlocEnum>("Low")
        };

        await _yeastService.AddAsync(yeastAddRequest);

        YeastUpdateRequest yeastUpdateRequest = new()
        {
            Id = 1,
            Name = "",
            Lab = "TestLab",
            Code = "TestCode",
            Type = Enum.Parse<YeastTypeEnum>("Ale"),
            Form = Enum.Parse<YeastFormEnum>("Liquid"),
            Flocculation = Enum.Parse<YeastFlocEnum>("Low")
        };

        await Assert.ThrowsAsync<ArgumentException>(() => _yeastService.UpdateAsync(yeastUpdateRequest));
    }

    #endregion

    #region GetFilteredYeast

    [Fact]
    public async Task GetFilteredYeast_ValidSearch()
    {
        YeastAddRequest yeastAddRequest = new()
        {
            Id = 1,
            Name = "TestYeast",
            Lab = "TestLab",
            Code = "TestCode",
            Type = Enum.Parse<YeastTypeEnum>("Ale"),
            Form = Enum.Parse<YeastFormEnum>("Liquid"),
            Flocculation = Enum.Parse<YeastFlocEnum>("Low")
        };

        await _yeastService.AddAsync(yeastAddRequest);

        List<YeastResponse> yeastResponses = await _yeastService.GetFilteredAsync("Name", "TestYeast");

        Assert.True(yeastResponses.Count == 1);
    }

    [Fact]
    public async Task GetFilteredYeast_InvalidProperty()
    {
        YeastAddRequest yeastAddRequest = new()
        {
            Id = 1,
            Name = "TestYeast",
            Lab = "TestLab",
            Code = "TestCode",
            Type = Enum.Parse<YeastTypeEnum>("Ale"),
            Form = Enum.Parse<YeastFormEnum>("Liquid"),
            Flocculation = Enum.Parse<YeastFlocEnum>("Low")
        };

        await _yeastService.AddAsync(yeastAddRequest);

        await Assert.ThrowsAsync<NullReferenceException>(
            () => _yeastService.GetFilteredAsync("Invalid", "InvalidSearch"));
    }

    [Fact]
    public async Task GetFilteredYeast_InvalidSearch()
    {
        YeastAddRequest yeastAddRequest = new()
        {
            Id = 1,
            Name = "TestYeast",
            Lab = "TestLab",
            Code = "TestCode",
            Type = Enum.Parse<YeastTypeEnum>("Ale"),
            Form = Enum.Parse<YeastFormEnum>("Liquid"),
            Flocculation = Enum.Parse<YeastFlocEnum>("Low")
        };

        YeastAddRequest yeastAddRequest2 = new()
        {
            Id = 2,
            Name = "TestYeast2",
            Lab = "TestLab",
            Code = "TestCode",
            Type = Enum.Parse<YeastTypeEnum>("Ale"),
            Form = Enum.Parse<YeastFormEnum>("Liquid"),
            Flocculation = Enum.Parse<YeastFlocEnum>("Low")
        };

        await _yeastService.AddAsync(yeastAddRequest);
        await _yeastService.AddAsync(yeastAddRequest2);

        List<YeastResponse> yeastResponses = await _yeastService.GetFilteredAsync("Name", "InvalidSearch");

        Assert.True(yeastResponses.Count == 2);
    }

    #endregion
}