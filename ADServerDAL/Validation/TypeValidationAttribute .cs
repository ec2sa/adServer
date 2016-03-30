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
    /// Filtr walidacyjny typu
    /// </summary>
    public class TypeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var type = validationContext.ObjectInstance as Models.Type;

            if (value != null && type != null)
            {      
                switch (validationContext.MemberName)
                {
                        // Sprawdzenie unikalności nazwy
                    case "Name":
                        var name = (string)value;
                        if (name != null && name.Length > 0)
                        {
							using (var Context = new AdServContext())
                            {
                                if (Context.Types.Count(c => c.Name == name && c.Id != type.Id) > 0)
                                {
                                    return new ValidationResult("Typ o podanej nazwie już istnieje.", new string[] { validationContext.MemberName });
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
