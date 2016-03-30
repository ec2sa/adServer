using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ADServerDAL.Entities.Presentation;
using ADServerDAL.Filters;
using ADServerDAL.Models;
using ADServerDAL.Models.Base;

namespace ADServerManagementWebApplication.Models
{
	/// <summary>
	/// Model dla listy uprawnień
	/// </summary>
	public class RoleListViewModel : ListViewModel
    {
        /// <summary>
        /// Lista obiektów
        /// </summary>
        public IQueryable<Entity> Roles { get { return Query; } }

		/// <summary>
		/// Lista filtrów
		/// </summary>
		public RoleListViewModelFilter Filters { get { return (RoleListViewModelFilter)FilerBase; } }
    }
}