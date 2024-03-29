using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;
using HomebrewDesigner.Core.RepositoryContracts;
using HomebrewDesigner.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace HomebrewDesigner.Infrastructure.Repositories;

public class FermentableRepository : IRepository<Fermentables, FermentableUpdateRequest>
{
    
    private readonly ApplicationDbContext _db;
    
    public FermentableRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public async Task<Fermentables> AddAsync(Fermentables fermentable)
    {
        _db.Fermentables.Add(fermentable);
        await _db.SaveChangesAsync();

        return fermentable;
    }

    public Task<List<Fermentables>> GetAllAsync()
    {
        return _db.Fermentables.ToListAsync();
    }

    public async Task<Fermentables> UpdateAsync(Fermentables fermentable, FermentableUpdateRequest request)
    {
        Fermentables fermentablesToUpdate = await _db.Fermentables.FirstOrDefaultAsync(f => f.Id == fermentable.Id) ?? throw new InvalidOperationException() ;
        
        fermentablesToUpdate.Id = request.Id;
        fermentablesToUpdate.Name = request.Name;
        fermentablesToUpdate.Color = request.Color;
        fermentablesToUpdate.PotentialGravity = request.PotentialGravity;
        await _db.SaveChangesAsync();

        return fermentablesToUpdate;
    }

    public async Task<Fermentables> GetByIdAsync(int id)
    {
        return await _db.Fermentables.FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<Fermentables> GetByNameAsync(string name)
    {
        Fermentables? fermentable = await _db.Fermentables.FirstOrDefaultAsync(f => f.Name == name) ?? null;

        return fermentable;
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}