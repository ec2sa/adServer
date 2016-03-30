using System.Linq;
using ADServerDAL.Entities.Presentation;
using ADServerDAL.Filters;
using System.Collections.Generic;
using ADServerDAL.Models;
using ADServerDAL.Models.Base;

namespace ADServerManagementWebApplication.Models
{
	/// <summary>
	/// Model dla listy kategorii
	/// </summary>
	public class CampaignCategoriesListViewModel : ListViewModel
	{
		/// <summary>
		/// Lista kategorii
		/// </summary>
		public IQueryable<Entity> Categories
		{
			get { return Query; }
		}

		/// <summary>
		/// Filtry
		/// </summary>
		public CampaignCategoriesListViewModelFilter Filters
		{
			get { return (CampaignCategoriesListViewModelFilter)FilerBase; }
		}
	}
}