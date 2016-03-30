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
    /// Filtr walidacyjny kampanii
    /// </summary>
    public class CampaignValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Campaign campaign = validationContext.ObjectInstance as Campaign;

            if (value != null && campaign != null)
            {
                switch (validationContext.MemberName)
                {
                    //    ///Sprawdzenie czy data rozpoczęcia jest wcześniejsza od daty zakończenia
                    //case "StartDate":
                    //    DateTime startDate = (DateTime)value;
                    //    if (startDate > campaign.EndDate)
                    //    {
                    //        return new ValidationResult("Data rozpoczęcia musi być wcześniejsza od daty zakończenia.", new string[] { validationContext.MemberName, "StartDate" });
                    //    }
                    //    break;

                    //    ///Sprawdzenie czy data zakończenia jest późniejsza niż data rozpoczęcia
                    //case "EndDate":
                    //    DateTime endDate = (DateTime)value;
                    //    if (endDate < campaign.StartDate)
                    //    {
                    //        return new ValidationResult("Data zakończenia musi być późniejsza od daty rozpoczęcia.", new string[] { validationContext.MemberName, "EndDate" });
                    //    }
                    //    break;

                        // Sprawdzenie unikalności nazwy
                    case "Name":
                        var name = (string)value;
                        if (name != null && name.Length > 0)
                        {
							using (var Context = new AdServContext())
                            {
                                if (Context.Campaigns.Count(c => c.Name == name && c.Id != campaign.Id) > 0)
                                {
                                    return new ValidationResult("Kampania o podanej nazwie już istnieje.", new string[] { validationContext.MemberName });
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
