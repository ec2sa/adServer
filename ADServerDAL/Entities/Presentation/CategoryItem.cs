using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADServerDAL.Entities.Presentation
{
    /// <summary>
    /// Reprezentacja obiektu kategorii
    /// </summary>
    public class CategoryItem : PresentationItem
    {
        /// <summary>
        /// Identyfikator
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Nazwa kategorii
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Kod kategorii
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Lista powiązanych kampanii
        /// </summary>
        public List<CampaignItem> Campaigns { get; set; }

    }
}