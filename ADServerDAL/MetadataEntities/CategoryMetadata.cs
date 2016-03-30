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
    /// Metadane kategorii
    /// </summary>
    public partial class CategoryMetadata
    {
        /// <summary>
        /// Identyfikator
        /// </summary>
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        /// <summary>
        /// Walidacja nazwy kategorii
        /// </summary>
        [Required(ErrorMessage = "Pole {0} wymagane")]
        [CategoryValidation]
        [Display(Name = "Nazwa")]
        [StringLength(150, ErrorMessage = "Maksymalna dopuszczalna długość pola {0} wynosi {1}")]
        public string Name { get; set; }

        /// <summary>
        /// Walidacja kodu
        /// </summary>
        [Required(ErrorMessage = "Pole {0} wymagane")]
        [Display(Name = "Kod")]
        [CategoryValidationAttribute]
        [StringLength(50, ErrorMessage = "Maksymalna dopuszczalna długość pola {0} wynosi {1}")]
        public string Code { get; set; }
    }
}
