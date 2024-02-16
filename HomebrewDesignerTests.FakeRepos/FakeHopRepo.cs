using System.Reflection;
using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;
using HomebrewDesigner.Core.Helpers;
using HomebrewDesigner.Core.RepositoryContracts;
namespace HomebrewDesignerTests.FakeRepos;




public class FakeHopRepo : IHopRepository
{
    // Create a list to hold hop objects.
    private readonly List<Hop> _hops = new();
    
    public async Task<Hop> AddHopAsync(Hop hop)
    {
        _hops.Add(hop);
        return await Task.Run(() => hop);
    }

    public async Task<List<Hop>> GetAllHopsAsync()
    {
        return await Task.Run(() => _hops);
    }

    public Task<Hop> UpdateHopAsync(Hop hop, HopUpdateRequest request)
    {
        Hop hopToUpdate = _hops.Find(h => h.Id == hop.Id) ?? throw new ArgumentException("Hop not found.");
        
        hopToUpdate.Name = request.Name;
        hopToUpdate.AlphaAcid = request.AlphaAcid;
        
        return Task.Run(() => hopToUpdate);
    }

    public async Task<Hop> GetHopByIdAsync(int id)
    {
        return await Task.Run(() => _hops.Find(h => h.Id == id)) ?? throw new ArgumentException("Hop not found");
    }
    
    
}
