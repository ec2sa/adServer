using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADServerDAL.Entities.Presentation
{
    /// <summary>
    /// Klasa wykorzystywana jako odpowiedź API
    /// </summary>
    public class ApiResponse
    {
        public ApiResponse()
        {
            Errors = new List<ApiValidationErrorItem>();
        }

        /// <summary>
        /// Lista błędów wykrytych podczas danej operacji
        /// </summary>
        public List<ApiValidationErrorItem> Errors { get; set; }

        /// <summary>
        /// Określa czy wystąpiły błędy podczas danej operacji
        /// </summary>
        public bool Accepted { get; set; }
    }
}