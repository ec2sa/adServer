using System;
using System.Linq;
using ADServerDAL.Filters;
using ADServerDAL.Models.Base;

namespace ADServerManagementWebApplication.Models
{
    /// <summary>
    /// Klasa pomocnicza dla list
    /// </summary>
    public class ListViewModel
    {
        /// <summary>
        /// Numer aktualnej strony
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Pole do sortowania
        /// </summary>
        public string SortExpression { get; set; }

        /// <summary>
        /// Czy sortowanie rosnące
        /// </summary>
        public bool SortAccending { get; set; }

        /// <summary>
        /// Liczba znalezionych obiektów
        /// </summary>
        public int NumberOfResults { get; set; }

        /// <summary>
        /// Liczba obiektów na stronę
        /// </summary>
        public int ItemsPerPage { get; set; }

        /// <summary>
        /// Liczba stron
        /// </summary>
        public int TotalPages
        {
            get
            {
                int result = 0;
                if (ItemsPerPage != 0)
                {
                    result = (int)Math.Ceiling((double) NumberOfResults / (double)ItemsPerPage);
                }

                if (result == 0) result = 1;
                return result;
            }
        }

		/// <summary>
		/// Lista encji
		/// </summary>
		public IQueryable<Entity> Query { get; set; }

		/// <summary>
		/// Baza filtra
		/// </summary>
		public ViewModelFilterBase FilerBase { get; set; }
    }
}