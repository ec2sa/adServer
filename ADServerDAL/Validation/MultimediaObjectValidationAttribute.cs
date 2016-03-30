using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADServerDAL.Models;

namespace ADServerDAL.Validation
{
    /// <summary>
    /// Filtr walidacyjny obiektu multimedialnego
    /// </summary>
    public class MultimediaObjectValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var multimediaObject = validationContext.ObjectInstance as MultimediaObject;

            if (value != null && multimediaObject != null)
            {
                switch (validationContext.MemberName)
                {
                        // Sprawdzenie unikalności nazwy obiektu
                    case "Name":
                        var name = (string)value;
                        if (!string.IsNullOrEmpty(name) && name.Length > 0)
                        {
							using (var Context = new AdServContext())
                            {
                                if (Context.MultimediaObjects.Count(c => c.Name == name && c.Id != multimediaObject.Id) > 0)
                                {
                                    return new ValidationResult("Obiekt o podanej nazwie już istnieje.", new[] { validationContext.MemberName });
                                }
                            }
                        }
                        break;
                }
            }

            return ValidationResult.Success;
        }
    }
}
