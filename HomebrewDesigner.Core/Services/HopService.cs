using System.Reflection;
using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;
using HomebrewDesigner.Core.Helpers;
using HomebrewDesigner.Core.RepositoryContracts;
using HomebrewDesigner.Core.ServiceContracts;
using ArgumentException = System.ArgumentException;

namespace HomebrewDesigner.Core.Services;

public class HopService : IService<HopAddRequest, HopUpdateRequest, HopResponse>
{
    // Create a list to hold hop objects.
    private readonly IHopRepository _hopRepository;

    public HopService(IHopRepository hopRepository)
    {
        _hopRepository = hopRepository;
    }

    public async Task<HopResponse> AddAsync(HopAddRequest? request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        Hop hopToAdd = request.ToHop();

        ValidationHelper.ModelValidation(request);


        foreach (var hop in await _hopRepository.GetAllHopsAsync())
        {
            if (hop.Name.ToLower() == hopToAdd.Name.ToLower())
            {
                throw new ArgumentException("Hop name already exists.");
            }
        }

        await _hopRepository.AddHopAsync(hopToAdd);

        return hopToAdd.ToHopResponse();
    }

    public async Task<List<HopResponse>> GetAllAsync()
    {
        List<Hop> hops = await _hopRepository.GetAllHopsAsync();

        return hops.Select(h => h.ToHopResponse()).ToList();
    }


    public async Task<HopResponse> UpdateAsync(HopUpdateRequest? request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        Hop hopToUpdate = await _hopRepository.GetHopByIdAsync(request.Id);

        //if null, throw exception
        if (hopToUpdate is null)
        {
            throw new ArgumentException("Hop with id {request.Id} not found.");
        }

        ValidationHelper.ModelValidation(request);

        await _hopRepository.UpdateHopAsync(hopToUpdate, request);

        return hopToUpdate.ToHopResponse();
    }

    public async Task<HopResponse> GetByIdAsync(int id)
    {
        if (id < 0)
        {
            throw new ArgumentException($"Field {nameof(id)} cannot be less than 0.");
        }

        Hop hop = await _hopRepository.GetHopByIdAsync(id);

        return hop.ToHopResponse();
    }

    /*******ToDo Tighten up this code************/

    public async Task<List<HopResponse>> GetFilteredAsync(string? searchBy, string? searchString)
    {
        List<Hop> hops = await _hopRepository.GetAllHopsAsync();

        PropertyInfo? property = null;

        List<PropertyInfo> propertyInfo = typeof(HopResponse).GetProperties().ToList();

        foreach (PropertyInfo prop in propertyInfo)
        {
            if (String.Equals(prop.ToString()?.Substring(prop.ToString().IndexOf(" ", StringComparison.Ordinal) + 1),
                    searchBy, StringComparison.OrdinalIgnoreCase))
            {
                property = prop;
                break;
            }
        }

        if (String.IsNullOrEmpty(searchString))
        {
            return hops.Select(h => h.ToHopResponse()).ToList();
        }

        IEnumerable<HopResponse> hop = hops.Select(h => h.ToHopResponse());

        List<HopResponse> filteredHops = hop
            .Where(h => property.GetValue(h).ToString().ToLower()
                .Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
        if (filteredHops.Any())
        {
            return filteredHops;
        }

        return hops.Select(h => h.ToHopResponse()).ToList();
    }
}