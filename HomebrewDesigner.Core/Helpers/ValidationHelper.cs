using System.ComponentModel.DataAnnotations;

namespace HomebrewDesigner.Core.Helpers;

public class ValidationHelper
{
    /// <summary>
    ///Validates attributes of provided object (PersonAddRequest).
    /// </summary>
    /// <param name="obj">Object (PersonAddRequest to validate)</param>
    /// <exception cref="ArgumentException"></exception>
  public static void ModelValidation(object obj)
    {
        //Model Validations
        ValidationContext validationContext = new ValidationContext(obj);
        List<ValidationResult> results = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(obj, validationContext, results, true);

        if (!isValid)
        {
            throw new ArgumentException(results.FirstOrDefault()?.ErrorMessage);
        }
    }
}