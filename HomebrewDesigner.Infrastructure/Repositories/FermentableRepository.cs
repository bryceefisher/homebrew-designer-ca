using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;
using HomebrewDesigner.Core.RepositoryContracts;
using HomebrewDesigner.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace HomebrewDesigner.Infrastructure.Repositories;

public class FermentableRepository : IFermentableRepository
{
    
    private readonly ApplicationDbContext _db;
    
    public FermentableRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public async Task<Fermentables> AddFermentableAsync(Fermentables fermentable)
    {
        _db.Fermentables.Add(fermentable);
        await _db.SaveChangesAsync();

        return fermentable;
    }

    public Task<List<Fermentables>> GetAllFermentablesAsync()
    {
        return _db.Fermentables.ToListAsync();
    }

    public async Task<Fermentables> UpdateFermentableAsync(Fermentables fermentable, FermentableUpdateRequest request)
    {
        Fermentables fermentablesToUpdate = await _db.Fermentables.FirstOrDefaultAsync(f => f.Id == fermentable.Id) ?? throw new InvalidOperationException() ;
        
        fermentablesToUpdate.Id = request.Id;
        fermentablesToUpdate.Name = request.Name;
        fermentablesToUpdate.Color = request.Color;
        fermentablesToUpdate.PotentialGravity = request.PotentialGravity;
        await _db.SaveChangesAsync();

        return fermentablesToUpdate;
    }

    public async Task<Fermentables> GetFermentableByIdAsync(int id)
    {
        return await _db.Fermentables.FirstOrDefaultAsync(f => f.Id == id);
    }
}