using System.Linq;
using ADServerDAL.Entities.Presentation;
using ADServerDAL.Filters;
using System.Collections.Generic;
using ADServerDAL.Models;
using ADServerDAL.Models.Base;

namespace ADServerManagementWebApplication.Models
{
	/// <summary>
	/// Model dla listy priorytetów
	/// </summary>
	public class CampaignPrioritiesListViewModel : ListViewModel
	{
		/// <summary>
		/// Lista priorytetów
		/// </summary>
		public IQueryable<Entity> Priorities
		{
			get { return Query; }
		}

		/// <summary>
		/// Filtry
		/// </summary>
		public CampaignPrioritiesListViewModelFilter Filters
		{
			get { return (CampaignPrioritiesListViewModelFilter)FilerBase; }
		}
	}
}