using ADServerDAL.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ADServerDAL
{
    /// <summary>
    /// Metadane priorytetu
    /// </summary>
    public partial class PriorityMetadata
    {
        /// <summary>
        /// Identyfikator
        /// </summary>
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        /// <summary>
        /// Walidacja nazwy priorytetu
        /// </summary>
        [Required(ErrorMessage = "Pole {0} wymagane")]
        [PriorityValidationAttribute]
        [Display(Name = "Nazwa")]
        [StringLength(150, ErrorMessage = "Maksymalna dopuszczalna długość pola {0} wynosi {1}")]
        public string Name { get; set; }

        /// <summary>
        /// Walidacja kodu priorytetu
        /// </summary>
        [Required(ErrorMessage = "Pole {0} wymagane")]
        [Display(Name = "Kod")]
        [PriorityValidationAttribute]
        [Range(0, int.MaxValue, ErrorMessage = "Niepoprawna wartość pola {0}")]
        public int Code { get; set; }
    }
}
