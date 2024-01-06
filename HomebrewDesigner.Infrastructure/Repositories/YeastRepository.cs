using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;
using HomebrewDesigner.Core.RepositoryContracts;
using HomebrewDesigner.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace HomebrewDesigner.Infrastructure.Repositories;

public class YeastRepository : IYeastRepository
{
    private readonly ApplicationDbContext _db;
    
    public YeastRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public async Task<Yeast> AddYeastAsync(Yeast yeast)
    {
        await _db.Yeasts.AddAsync(yeast);
        await _db.SaveChangesAsync();

        return yeast;
    }

    public async Task<List<Yeast>> GetAllYeastAsync()
    {
        return await _db.Yeasts.ToListAsync();
    }

    public async Task<Yeast> GetYeastByIdAsync(int id)
    {
        return await _db.Yeasts.FirstOrDefaultAsync(y => y.Id == id) ?? throw new InvalidOperationException();
    }

    public async Task<Yeast> UpdateYeastAsync(Yeast yeast, YeastUpdateRequest request)
    {
        Yeast yeastToUpdate = await _db.Yeasts.FirstOrDefaultAsync(y => y.Id == yeast.Id) ?? throw new InvalidOperationException();
        
        yeastToUpdate.Id = request.Id;
        yeastToUpdate.Name = request.Name;
        yeastToUpdate.Code = request.Code;
        yeastToUpdate.Lab = request.Lab;
        yeastToUpdate.Type = request.Type.ToString();
        yeastToUpdate.Form = request.Form.ToString();
        yeastToUpdate.Flocculation = request.Flocculation.ToString();
        await _db.SaveChangesAsync();

        return yeastToUpdate;
    }
}