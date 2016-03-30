using ADServerDAL.Entities.Presentation;
using System;
using System.Linq;

namespace ADServerDAL.Abstract
{
    /// <summary>
    /// Interfejs factory obsługujący obiekty będące powiązaniami obiektu multimedialnego z kampanią
    /// </summary>
    public interface IMultimediaObject_CampaignRepository : IDisposable
    {
        /// <summary>
        /// Kolekcja wszystkich obiektów
        /// </summary>
        IQueryable<MultimediaObject_CampaignItem> MultimediaObject_Campaign { get; }

        /// <summary>
        /// Możliwość ustawienia innego kontekstu niż wbudowany
        /// </summary>
        /// <param name="context">Nowy kontekst EF</param>
        void SetContext(System.Data.Entity.DbContext context);
    }
}
