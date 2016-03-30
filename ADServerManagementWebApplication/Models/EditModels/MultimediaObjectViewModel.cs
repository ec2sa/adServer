using ADServerDAL;
using System.Web.Mvc;
using ADServerDAL.Models;

namespace ADServerManagementWebApplication.Models
{
    /// <summary>
    /// Model do edycji obiektu multimedialnego
    /// </summary>
    public class MultimediaObjectViewModel
    {
        /// <summary>
        /// Encja obiektu multimedialnego
        /// </summary>
        public MultimediaObject MultimediaObject { get; set; }

		/// <summary>
		/// Lista typów multimedialnych do wyboru
		/// </summary>
		public SelectList MultimediaTypes { get; set; }

		/// <summary>
		/// Lista użytkowników
		/// </summary>
		public SelectList Users { get; set; }

        /// <summary>
        /// Adres powrotny z formualrza edycyjnego
        /// </summary>
        public string ReturnURL { get; set; }

        /// <summary>
        /// Identyfikator żadania
        /// </summary>
        public string Guid { get; set; }
    }
}