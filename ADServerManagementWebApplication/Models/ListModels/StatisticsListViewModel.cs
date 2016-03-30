using System.Linq;
using ADServerDAL.Filters;
using ADServerDAL.Models.Base;

namespace ADServerManagementWebApplication.Models
{
    /// <summary>
    /// Model dla listy statystyk
    /// </summary>
    public class StatisticsListViewModel : ListViewModel
    {
        /// <summary>
        /// Lista statystyk
        /// </summary>
		public IQueryable<Entity> Statistics { get { return Query; } }

        /// <summary>
        /// Lista filtrów
        /// </summary>
		public StatisticsListViewModelFilter Filters { get { return (StatisticsListViewModelFilter)FilerBase; } }
    }
}