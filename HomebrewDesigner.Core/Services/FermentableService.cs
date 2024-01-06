using System.Reflection;
using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;
using HomebrewDesigner.Core.Helpers;
using HomebrewDesigner.Core.RepositoryContracts;
using HomebrewDesigner.Core.ServiceContracts;

namespace HomebrewDesigner.Core.Services;

public class FermentableService : IFermentableService
{
    private readonly IFermentableRepository _fermentableRepository;
    
    public FermentableService(IFermentableRepository fermentableRepository)
    {
        _fermentableRepository = fermentableRepository;
    }

    public async Task<FermentableResponse> AddFermentableAsync(FermentableAddRequest request)
    {
        if (request is null)
        {
            throw new ArgumentException("Fermentable object cannot be null.");
        }
        
        Fermentables fermentableToAdd = request.ToFermentable();
        
        ValidationHelper.ModelValidation(request);
        
        foreach (var ferm in await _fermentableRepository.GetAllFermentablesAsync())
        {
            if (string.Equals(ferm.Name, fermentableToAdd.Name, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new ArgumentException("Fermentable name already exists.");
            }
        }

        await _fermentableRepository.AddFermentableAsync(fermentableToAdd);
        

        return fermentableToAdd.ToFermentableResponse();
    }

    public async Task<List<FermentableResponse>> GetAllFermentablesAsync()
    {
        List<Fermentables> fermentables = await _fermentableRepository.GetAllFermentablesAsync();
        return fermentables.Select(f => f.ToFermentableResponse()).ToList();
    }

    public async Task<FermentableResponse> UpdateFermentableAsync(FermentableUpdateRequest request)
    {
        if (request is null)
        {
            throw new ArgumentException("Error: Fermentables object cannot be null.");
        }
        
        List<Fermentables> fermentables = await _fermentableRepository.GetAllFermentablesAsync();
        
        Fermentables fermentablesToUpdate = fermentables.FirstOrDefault(g => g.Id == request.Id) ??
                                            throw new ArgumentException($"Fermentables with id {request.Id} not found.");
        
        ValidationHelper.ModelValidation(request);
        
        if (fermentablesToUpdate is null)
        {
            throw new ArgumentException($"Fermentables object not found");
        }

        Fermentables fermentable = await  _fermentableRepository.UpdateFermentableAsync(fermentablesToUpdate, request);
        
        return fermentable.ToFermentableResponse();
    }

    public async Task<FermentableResponse> GetFermentableByIdAsync(int id)
    {
        if (id < 0)
        {
            throw new ArgumentException($"Field {nameof(id)} cannot be less than 0.");
        }

        Fermentables fermentable = await  _fermentableRepository.GetFermentableByIdAsync(id);
        
        return fermentable.ToFermentableResponse();
    }
    
    /*******ToDo Tighten up this code************/
    public async Task<List<FermentableResponse>> GetFilteredFermentableAsync(string? searchBy, string? searchString)
    {
        List<Fermentables> fermentables = await _fermentableRepository.GetAllFermentablesAsync();
        
        PropertyInfo? property = null;

        List<PropertyInfo> propertyInfo = typeof(FermentableResponse).GetProperties().ToList();

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
            return fermentables.Select(y => y.ToFermentableResponse()).ToList();
        }

        IEnumerable<FermentableResponse> fermentable = fermentables.Select(y => y.ToFermentableResponse());

        List<FermentableResponse> filteredFermentable = fermentable
            .Where(h => property.GetValue(h).ToString().ToLower()
                .Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
        
        if (filteredFermentable.Any())
        {
            return filteredFermentable;
        }

        return fermentables.Select(y => y.ToFermentableResponse()).ToList();
    }
}