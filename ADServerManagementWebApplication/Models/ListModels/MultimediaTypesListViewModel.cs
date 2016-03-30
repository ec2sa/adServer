using System.Linq;
using ADServerDAL.Entities.Presentation;
using ADServerDAL.Filters;
using System.Collections.Generic;
using ADServerDAL.Models;
using ADServerDAL.Models.Base;

namespace ADServerManagementWebApplication.Models
{
	/// <summary>
	/// Model dla listy typów
	/// </summary>
	public class MultimediaTypesListViewModel : ListViewModel
	{
		/// <summary>
		/// Lista typów
		/// </summary>
		public IQueryable<Entity> MultimediaTypes { get { return Query; } }

		/// <summary>
		/// Filtry
		/// </summary>
		public MultimediaTypesListViewModelFilter Filters
		{
			get
			{
				return (MultimediaTypesListViewModelFilter)FilerBase;
			}
		}
	}
}