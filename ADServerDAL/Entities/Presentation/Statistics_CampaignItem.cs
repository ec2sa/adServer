﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADServerDAL.Entities.Presentation
{
    /// <summary>
    /// Reprezentacja obiektu statystyki
    /// </summary>
    public class Statistics_CampaignItem : PresentationItem
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
        /// Identyfikator kampanii
        /// </summary>
        public int CampaignId { get; set; }
    }
}