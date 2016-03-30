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
    /// Filtr walidacyjny kategorii
    /// </summary>
    public class CategoryValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var category = validationContext.ObjectInstance as Category;

            if (value != null && category != null)
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
                                if (Context.Categories.Count(c => c.Name == name && c.Id != category.Id) > 0)
                                {
                                    return new ValidationResult("Kategoria o podanej nazwie już istnieje.", new string[] { validationContext.MemberName });
                                }
                            }
                        }
                        break;

                        // Sprawdzenie unikalności kodu
                    case"Code":
                        string code = (string)value;
                        if (code != null && code.Length > 0)
                        {
							using (AdServContext Context = new AdServContext())
                            {
                                if (Context.Categories.Count(c => c.Code == code && c.Id != category.Id) > 0)
                                {
                                    return new ValidationResult("Kategoria o podanym kodzie już istnieje.", new string[] { validationContext.MemberName });
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
