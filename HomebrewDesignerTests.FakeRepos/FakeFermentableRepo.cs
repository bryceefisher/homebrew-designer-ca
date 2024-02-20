using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;
using HomebrewDesigner.Core.RepositoryContracts;

namespace HomebrewDesignerTests.FakeRepos;

public class FakeFermentableRepo : IRepository<Fermentables, FermentableUpdateRequest>
{
    // Create a list to hold hop objects.
    private readonly List<Fermentables> _fermentables = new();
    
    public async Task<Fermentables> AddAsync(Fermentables fermentable)
    {
        _fermentables.Add(fermentable);
        return await Task.Run(() => fermentable);
    }

    public async Task<List<Fermentables>> GetAllAsync()
    {
        return await Task.Run(() => _fermentables);
    }

    public Task<Fermentables> UpdateAsync(Fermentables fermentable, FermentableUpdateRequest request)
    {
        Fermentables fermentableToUpdate = _fermentables.Find(f => f.Id == fermentable.Id) ?? throw new ArgumentException("Fermentable not found.");
        
        fermentableToUpdate.Name = request.Name;
        fermentableToUpdate.Color = request.Color;
        fermentableToUpdate.Origin = request.Origin.ToString();
        fermentableToUpdate.Type = request.Type.ToString();
        fermentableToUpdate.PotentialGravity = request.PotentialGravity;
        fermentableToUpdate.MaxInBatch = request.MaxInBatch;
        
        return Task.Run(() => fermentableToUpdate);
    }

    public async Task<Fermentables> GetByIdAsync(int id)
    {
        return await Task.Run(() => _fermentables.Find(f => f.Id == id)) ?? throw new ArgumentException("Fermentable not found");
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}