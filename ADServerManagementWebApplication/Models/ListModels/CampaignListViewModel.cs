using System.Linq;
using ADServerDAL.Entities.Presentation;
using ADServerDAL.Filters;
using System.Collections.Generic;
using ADServerDAL.Models;
using ADServerDAL.Models.Base;

namespace ADServerManagementWebApplication.Models
{
    /// <summary>
    /// Model dla listy kampanii
    /// </summary>
    public class CampaignListViewModel : ListViewModel
    {
        /// <summary>
        /// Lista kampanii
        /// </summary>
        public IQueryable<Entity> Campaigns { get { return Query; } }

        /// <summary>
        /// Filtry
        /// </summary>
        public CampaignListViewModelFilter Filters { get { return (CampaignListViewModelFilter)FilerBase; }  }
    }
}