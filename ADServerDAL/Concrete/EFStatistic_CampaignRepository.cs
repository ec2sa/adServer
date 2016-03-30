using ADServerDAL.Abstract;
using ADServerDAL.Entities.Presentation;
using ADServerDAL.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADServerDAL.Helpers;

namespace ADServerDAL.Concrete
{
    /// <summary>
    /// Implementacja repozytorium powiązań statystyk z kampaniami
    /// </summary>
    public class EFStatistic_CampaignRepository : EFBaseRepository, IStatistics_CampaignRepository
    {
        /// <summary>
        /// Kolekcja dostępnych statystyk
        /// </summary>
        public IQueryable<Statistics_CampaignItem> Statistics_Campaign
        {
            get
            {
	            var q1 = 
					from s in Context.Campaigns
		            from ss in s.Statistics
		            join u in Context.Users on ss.UserId equals u.Id
		            select new {s.Id, ss};

                var query = from q in q1
                            select new Statistics_CampaignItem
                            {
                                ID = q.Id,
                                CampaignId = q.Id,
                                StatisticsId = q.ss.Id
                            };

                return query;
            }
        }

        /// <summary>
        /// Możliwość ustawienia innego kontekstu niż wbudowany
        /// </summary>
        /// <param name="context">Nowy kontekst EF</param>
        public void SetContext(System.Data.Entity.DbContext context)
        {
            base.SetNewContext(context);
        }
    }
}
