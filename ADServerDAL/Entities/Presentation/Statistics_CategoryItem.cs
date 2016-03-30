using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADServerDAL.Entities.Presentation
{
    /// <summary>
    /// Reprezentacja obiektu statystyki
    /// </summary>
    public class Statistics_CategoryItem : PresentationItem
    {
        /// <summary>
        /// Identyfikator
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Identyfikator statystyki
        /// </summary>
        public int StatisticsId { get; set; }

        /// <summary>
        /// Identyfikator kategorii
        /// </summary>
        public int CategoryId { get; set; }
    }
}