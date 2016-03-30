using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADServerDAL.Entities.Presentation
{
    /// <summary>
    /// Reprezentacja obiektu kampanii
    /// </summary>
    public class Campaign_CategoryItem : PresentationItem
    {
        /// <summary>
        /// Identyfikator
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Identyfikator kampanii
        /// </summary>
        public int CampaignId { get; set; }

        /// <summary>
        /// Identyfikator kategorii
        /// </summary>
        public int CategoryId { get; set; }
    }
}