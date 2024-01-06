using System.Reflection;
using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;
using HomebrewDesigner.Core.Helpers;
using HomebrewDesigner.Core.RepositoryContracts;
using HomebrewDesigner.Core.ServiceContracts;
using ArgumentException = System.ArgumentException;

namespace HomebrewDesigner.Core.Services;

public class YeastService : IYeastService
{
    private readonly IYeastRepository _yeastRepository;
    
    public YeastService(IYeastRepository yeastRepository)
    {
        _yeastRepository = yeastRepository;
    }
      

    public async Task<YeastResponse> AddYeastAsync(YeastAddRequest? request)
    {
        if (request is null)
        {
            throw new ArgumentException($"Yeast object cannot be null. {nameof(request)}");
        }

        Yeast yeast = request.ToYeast();

        ValidationHelper.ModelValidation(request);

        foreach (var yeasts in await _yeastRepository.GetAllYeastAsync())
        {
            if (yeasts.Name == yeast.Name)
            {
                throw new ArgumentException($"Yeast with name {yeast.Name} already exists.");
            }
        }

        await _yeastRepository.AddYeastAsync(yeast);
        
        return yeast.ToYeastResponse();
    }

    public async Task<List<YeastResponse>> GetAllYeastsAsync()
    {
        List<Yeast> yeasts = await _yeastRepository.GetAllYeastAsync();

        return yeasts.Select(y => y.ToYeastResponse()).ToList();
    }

    public async Task<YeastResponse> UpdateYeastAsync(YeastUpdateRequest? request)
    {
        if (request is null)
        {
            throw new ArgumentException($"Yeasts object cannot be null. {nameof(request)}");
        }

        Yeast yeastToUpdate = await _yeastRepository.GetYeastByIdAsync(request.Id);

        ValidationHelper.ModelValidation(request);

        if (yeastToUpdate is null)
        {
            throw new ArgumentException($"Yeast object not found");
        }
        
        Yeast yeast = await _yeastRepository.UpdateYeastAsync(yeastToUpdate, request);
        

        return yeast.ToYeastResponse();
    }

    public async Task<YeastResponse> GetYeastByIdAsync(int id)
    {
        
        if (id < 0)
        {
            throw new ArgumentException($"Field {nameof(id)} cannot be less than 0.");
        }

        Yeast yeast = await _yeastRepository.GetYeastByIdAsync(id);
        
        return yeast.ToYeastResponse();
    }
    
    /*******ToDo Tighten up this code************/
    public async Task<List<YeastResponse>> GetFilteredYeastAsync(string? searchBy, string? searchString)
    {
        List<Yeast> yeasts = await _yeastRepository.GetAllYeastAsync();
        
        PropertyInfo? property = null;

        List<PropertyInfo> propertyInfo = typeof(YeastResponse).GetProperties().ToList();

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
            return yeasts.Select(y => y.ToYeastResponse()).ToList();
        }

        IEnumerable<YeastResponse> yeast = yeasts.Select(y => y.ToYeastResponse());

        List<YeastResponse> filteredyeast = yeast
            .Where(h => property.GetValue(h).ToString().ToLower()
                .Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
        
        if (filteredyeast.Any())
        {
            return filteredyeast;
        }

        return yeasts.Select(y => y.ToYeastResponse()).ToList();
    }
}