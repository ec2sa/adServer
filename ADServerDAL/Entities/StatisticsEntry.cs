using ADServerDAL.Entities.Presentation;
using System;
using System.Collections.Generic;
using ADServerDAL.Models;

namespace ADServerDAL.Entities
{
    /// <summary>
    /// Klasa przechowująca obiekt statystyk (odpowiednik jednego konkretnego wpisu w bazie z tabeli Statistics) 
    /// wraz z powiązanymi obiektami znajdującymi się w innych tabelach statystyk
    /// </summary>
    public class StatisticsEntry
    {
        public Statistic Statistics { get; set; }

        /// <summary>
        /// Lista powiązanych z daną statystyką kategorii (zapisanych w tabeli Statistics_Categories)
        /// </summary>
        public Dictionary<string, int> Categories { get; set; }

        public List<SelectedMultimediaObjectCampaign> SelectedMultimediaObjectCampaigns { get; set; }
    }

    public class SelectedMultimediaObjectCampaign
    {
        public int CampaignId { get; set; }

        public int PriorityCode { get; set; }

        public bool IsActiveCampaign { get; set; }

        public DateTime CampaignStartDate { get; set; }

        public DateTime CampaignEndDate { get; set; }
    }
}
