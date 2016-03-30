using ADServerDAL.Abstract;
using ADServerDAL.Entities.Presentation;
using System.Linq;

namespace ADServerDAL.Concrete
{
    /// <summary>
    /// Impelementacja repozytorium obiektów będących powiązaniem obiektu multimedialnego z kampanią
    /// </summary>
    public class EFMultimediaObject_CampaignRepository : EFBaseRepository, IMultimediaObject_CampaignRepository
    {
        /// <summary>
        /// Kolekcja wszystkich obiektów
        /// </summary>
        public IQueryable<MultimediaObject_CampaignItem> MultimediaObject_Campaign
        {
            get
            {
	            var q1 = from c in Context.Campaigns
		            from cc in c.MultimediaObjects
		            select new {c.Id, cc};

                var query = from q in q1
                            select new MultimediaObject_CampaignItem
                            {
                               ID = q.Id,
                               CampaignId = q.Id,
                               MultimediaObjectId = q.cc.Id
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
