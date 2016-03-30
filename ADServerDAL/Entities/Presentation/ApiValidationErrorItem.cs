using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADServerDAL.Entities.Presentation
{
    /// <summary>
    /// Klasa przechowująca informacje o błędzie operacji
    /// </summary>
    public class ApiValidationErrorItem
    {
        /// <summary>
        /// Komunikat o błędzie
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Pole w modelu, którego dotyczy błąd
        /// </summary>
        public string Property { get; set; }
    }
}