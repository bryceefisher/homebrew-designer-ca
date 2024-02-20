using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;
using HomebrewDesigner.Core.RepositoryContracts;
using HomebrewDesigner.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace HomebrewDesigner.Infrastructure.Repositories;

public class HopRepository : IRepository<Hop, HopUpdateRequest>
{
    private readonly ApplicationDbContext _db;
    
    public HopRepository(ApplicationDbContext db)
    {
        _db = db;
    }


    public async Task<Hop> AddAsync(Hop hop)
    {
        await _db.Hops.AddAsync(hop);
        await _db.SaveChangesAsync();

        return hop;
    }

    public async Task<List<Hop>> GetAllAsync()
    {
        return await _db.Hops.ToListAsync();
    }

    public async Task<Hop> UpdateAsync(Hop hop, HopUpdateRequest request)
    {
        Hop hopToUpdate = _db.Hops.FirstOrDefault(h => h.Id == request.Id) ?? throw new InvalidOperationException();
        
        hopToUpdate.Id = request.Id;
        hopToUpdate.Name = request.Name;
        hopToUpdate.AlphaAcid = request.AlphaAcid;
        await _db.SaveChangesAsync();

        return hopToUpdate;
    }

    public async Task<Hop> GetByIdAsync(int id)
    {
        Hop hop = await _db.Hops.FirstOrDefaultAsync(h => h.Id == id) ?? throw new InvalidOperationException();

        return hop;
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}