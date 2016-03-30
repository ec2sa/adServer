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
    /// Filtr walidacyjny priorytetu
    /// </summary>
    public class PriorityValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var priority = validationContext.ObjectInstance as Priority;

            if (value != null && priority != null)
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
                                if (Context.Priorities.Count(c => c.Name == name && c.Id != priority.Id) > 0)
                                {
                                    return new ValidationResult("Priorytet o podanej nazwie już istnieje.", new string[] { validationContext.MemberName });
                                }
                            }
                        }
                        break;

                        // Sprawdzenie unikalności kodu
                    case "Code":
                        var code = (int)value;
						using (var Context = new AdServContext())
                        {
                            if (Context.Priorities.Count(c => c.Code == code && c.Id != priority.Id) > 0)
                            {
                                return new ValidationResult("Priorytet o podanym kodzie już istnieje.", new string[] { validationContext.MemberName });
                            }
                        }
                        break;
                }
            }

            return ValidationResult.Success;
        }
    }
}
