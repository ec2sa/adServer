using System.Linq;
using ADServerDAL.Filters;
using ADServerDAL.Models.Base;

namespace ADServerManagementWebApplication.Models
{
	/// <summary>
	/// Model dla listy obiektów
	/// </summary>
	public class MultimediaObjectsListViewModel : ListViewModel
	{
		/// <summary>
		/// Lista obiektów
		/// </summary>
		public IQueryable<Entity> MultimediaObjects { get { return Query; } }

		/// <summary>
		/// Filtry
		/// </summary>
		public MultimediaObjectListViewModelFilter Filters { get { return (MultimediaObjectListViewModelFilter)FilerBase; } }
	}
}