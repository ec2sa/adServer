using ADServerDAL;
using ADServerDAL.Models;

namespace ADServerManagementWebApplication.Models
{
    /// <summary>
    /// Model do edycji kategorii
    /// </summary>
    public class CampaignCategoryViewModel
    {
        /// <summary>
        /// Encja kategorii
        /// </summary>
        public Category Category { get; set; }
    }
}