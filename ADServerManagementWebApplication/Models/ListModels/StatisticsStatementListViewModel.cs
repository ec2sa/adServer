using ADServerDAL.Entities.Presentation;
using ADServerDAL.Filters;
using System.Collections.Generic;

namespace ADServerManagementWebApplication.Models
{
    /// <summary>
    /// Model dla zestawień
    /// </summary>
    public class StatisticsStatementListViewModel : ListViewModel
    {
        #region - Properties -
        /// <summary>
        /// Lista zestawień
        /// </summary>
        public List<StatisticsStatementItem> Statement { get; set; }

        /// <summary>
        /// Filtry
        /// </summary>
        public StatisticsStatementListViewModelFilter Filters { get; set; }

        /// <summary>
        /// Typ zestawienia
        /// </summary>
        public StatisticsStatementType StatementType { get; set; }

        /// <summary>
        /// Tytuł zestawienia
        /// </summary>
        public string StatementTitle { get; set; }

        /// <summary>
        /// Adres URL
        /// </summary>
        public string DestinationURL { get; set; } 
        #endregion

        #region - Constructors -
        public StatisticsStatementListViewModel()
        {
            Statement = new List<StatisticsStatementItem>();
            Filters = new StatisticsStatementListViewModelFilter();
        } 
        #endregion
    }
}