using System.Reflection;
using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;
using HomebrewDesigner.Core.Helpers;
using HomebrewDesigner.Core.RepositoryContracts;
using HomebrewDesigner.Core.ServiceContracts;
using ArgumentException = System.ArgumentException;

namespace HomebrewDesigner.Core.Services;

public class YeastService : IService<YeastAddRequest, YeastUpdateRequest, YeastResponse>
{
    private readonly IRepository<Yeast, YeastUpdateRequest> _yeastRepository;
    
    public YeastService(IRepository<Yeast, YeastUpdateRequest> yeastRepository)
    {
        _yeastRepository = yeastRepository;
    }
      

    public async Task<YeastResponse> AddAsync(YeastAddRequest? request)
    {
        if (request is null)
        {
            throw new ArgumentException($"Yeast object cannot be null. {nameof(request)}");
        }

        Yeast yeast = request.ToYeast();

        ValidationHelper.ModelValidation(request);

        foreach (var yeasts in await _yeastRepository.GetAllAsync())
        {
            if (yeasts.Name == yeast.Name)
            {
                throw new ArgumentException($"Yeast with name {yeast.Name} already exists.");
            }
        }

        await _yeastRepository.AddAsync(yeast);
        
        return yeast.ToYeastResponse();
    }

    public async Task<List<YeastResponse>> GetAllAsync()
    {
        List<Yeast> yeasts = await _yeastRepository.GetAllAsync();

        return yeasts.Select(y => y.ToYeastResponse()).ToList();
    }

    public async Task<YeastResponse> UpdateAsync(YeastUpdateRequest? request)
    {
        if (request is null)
        {
            throw new ArgumentException($"Yeasts object cannot be null. {nameof(request)}");
        }

        Yeast yeastToUpdate = await _yeastRepository.GetByIdAsync(request.Id);

        ValidationHelper.ModelValidation(request);

        if (yeastToUpdate is null)
        {
            throw new ArgumentException($"Yeast object not found");
        }
        
        Yeast yeast = await _yeastRepository.UpdateAsync(yeastToUpdate, request);
        

        return yeast.ToYeastResponse();
    }

    public async Task<YeastResponse> GetByIdAsync(int id)
    {
        
        if (id < 0)
        {
            throw new ArgumentException($"Field {nameof(id)} cannot be less than 0.");
        }

        Yeast yeast = await _yeastRepository.GetByIdAsync(id);
        
        return yeast.ToYeastResponse();
    }
    
    /*******ToDo Tighten up this code************/
    public async Task<List<YeastResponse>> GetFilteredAsync(string? searchBy, string? searchString)
    {
        List<Yeast> yeasts = await _yeastRepository.GetAllAsync();
        
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

        List<YeastResponse> filteredYeast = yeast
            .Where(h => property.GetValue(h).ToString().ToLower()
                .Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
        
        if (filteredYeast.Any())
        {
            return filteredYeast;
        }

        return yeasts.Select(y => y.ToYeastResponse()).ToList();
    }
}