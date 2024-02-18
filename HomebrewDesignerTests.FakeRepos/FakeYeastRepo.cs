using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;
using HomebrewDesigner.Core.RepositoryContracts;

namespace HomebrewDesignerTests.FakeRepos;

public class FakeYeastRepo : IRepository<Yeast, YeastUpdateRequest>
{
    // Create a list to hold hop objects.
    private readonly List<Yeast> _yeast = new();
    
    public async Task<Yeast> AddAsync(Yeast yeast)
    {
        _yeast.Add(yeast);
        return await Task.Run(() => yeast);
    }

    public async Task<List<Yeast>> GetAllAsync()
    {
        return await Task.Run(() => _yeast);
    }

    public Task<Yeast> UpdateAsync(Yeast yeast, YeastUpdateRequest request)
    {
        Yeast yeastToUpdate = _yeast.Find(y => y.Id == yeast.Id) ?? throw new ArgumentException("Yeast not found.");
        
        yeastToUpdate.Name = request.Name;
        yeastToUpdate.Code = request.Code;
        yeastToUpdate.Flocculation = request.Flocculation.ToString();
        yeastToUpdate.Form = request.Form.ToString();
        yeastToUpdate.Lab = request.Lab;
        yeastToUpdate.Type = request.Type.ToString();
        
        return Task.Run(() => yeastToUpdate);
    }

    public async Task<Yeast> GetByIdAsync(int id)
    {
        return await Task.Run(() => _yeast.Find(y => y.Id == id)) ?? throw new ArgumentException("Yeast not found");
    }
}