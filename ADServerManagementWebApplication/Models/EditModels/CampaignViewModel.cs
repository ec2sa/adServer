using System.Collections.Generic;
using ADServerDAL;
using System.Web.Mvc;
using ADServerDAL.Models;

namespace ADServerManagementWebApplication.Models
{
    /// <summary>
    /// Model do ecyji kampanii
    /// </summary>
    public class CampaignViewModel
    {
        /// <summary>
        /// Encja kampanii
        /// </summary>
        public Campaign Campaign { get; set; }

        /// <summary>
        /// Lista priorytetów do wyboru
        /// </summary>
        public SelectList Priorities { get; set; }

        /// <summary>
        /// Adres powrotny z formularza edycyjnego
        /// </summary>
        public string ReturnURL { get; set; }

		/// <summary>
		/// Lista użytkowników
		/// </summary>
		public SelectList Users { get; set; }
    }
}