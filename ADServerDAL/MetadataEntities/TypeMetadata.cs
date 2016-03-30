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
    /// Metadane typu obiektu multimedialnego
    /// </summary>
    public partial class TypeMetadata
    {
        /// <summary>
        /// Identyfikator
        /// </summary>
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        /// <summary>
        /// Walidacja nazwy typu
        /// </summary>
        [Required(ErrorMessage = "Pole {0} wymagane")]
        [Display(Name = "Nazwa")]
        [TypeValidationAttribute]
        [StringLength(150, ErrorMessage = "Maksymalna dopuszczalna długość pola {0} wynosi {1}")]
        public string Name { get; set; }

        /// <summary>
        /// Walidacja szerokości
        /// </summary>
        [Required(ErrorMessage = "Pole {0} wymagane")]
        [Display(Name = "Szerokość")]
        [Range(1, int.MaxValue, ErrorMessage = "Niepoprawna wartość pola {0}")]
        public int Width { get; set; }

        /// <summary>
        /// Walidacja wysokości
        /// </summary>
        [Required(ErrorMessage = "Pole {0} wymagane")]
        [Display(Name = "Wysokość")]
        [Range(1, int.MaxValue, ErrorMessage = "Niepoprawna wartość pola {0}")]
        public int Height { get; set; }
    }
}
