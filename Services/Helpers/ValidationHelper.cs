using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Services.Helpers;

public static class ValidationHelper
{
    public static void ModelValidation(object obj)
    {
        var  validationContext = new ValidationContext(obj);

        var validationResults = new List<ValidationResult>();
        
        var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);

        if (!isValid)
        {
            throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
        }
    }
}