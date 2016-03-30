using ADServerDAL;
using ADServerDAL.Models;

namespace ADServerManagementWebApplication.Models
{
    /// <summary>
    /// Model do edycji priorytetu
    /// </summary>
    public class CampaignPriorityViewModel
    {
        /// <summary>
        /// Encja priorytetu
        /// </summary>
        public Priority Priority { get; set; }
    }
}