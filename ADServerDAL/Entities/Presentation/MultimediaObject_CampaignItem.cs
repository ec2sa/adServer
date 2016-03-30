using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADServerDAL.Entities.Presentation
{
    /// <summary>
    /// Reprezentacja obiektu będącego odzwierciedleniem powiązania obiektu multimedialnego z kampanią
    /// </summary>
    public class MultimediaObject_CampaignItem : PresentationItem
    {
        /// <summary>
        /// Identyfikator
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Identyfikator obiektu multimedialnego
        /// </summary>
        public int MultimediaObjectId { get; set; }

        /// <summary>
        /// Identyfikator kampanii
        /// </summary>
        public int CampaignId { get; set; }
    }
}