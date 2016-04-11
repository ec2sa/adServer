using ADServerDAL.Entities;
using ADServerDAL.Entities.Presentation;
using ADServerDAL.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADServerDAL.Models;

namespace ADServerDAL.Abstract
{
    /// <summary>
    /// Interfejs factory obsługujący kampanie
    /// </summary>
    public interface ICampaignRepository : IDisposable
    {
        /// <summary>
        /// Kolekcja wszystkich kampanii
        /// </summary>
        IQueryable<Campaign> Campaigns { get; }

	    /// <summary>
	    /// Zapisuje kampanię do bazy danych
	    /// </summary>
	    /// <param name="campaign">Obiekt kampanii do zapisu</param>
	    /// <param name="decrement">Czy zmniejszanea wartość punktów</param>        
	    ApiResponse SaveCampaign(Campaign campaign, bool decrement = false);

        /// <summary>
        /// Zwraca identyfikatory kampanii powiązanych z daną kategorią
        /// </summary>
        /// <param name="categoryId">Identyfikator kategorii</param>        
        IEnumerable<int> CampaignsToCategory(int categoryId);

        /// <summary>
        /// Zwraca identyfikatory kampanii powiązanych z danym obiektem multimedialnym
        /// </summary>
        /// <param name="objectId"></param>        
        IEnumerable<int> CampaignsToObject(int objectId);

        /// <summary>
        /// Pobiera obiekt kampanii na podstawie zadanego identyfikatora
        /// </summary>
        /// <param name="id">Identyfikator kampanii</param>        
        Campaign GetById(int id);

        /// <summary>
        /// Usuwa kampanię z bazy danych na podstawie zadanego identyfikatora
        /// </summary>
        /// <param name="id">Identyfikator kampanii</param>        
        ApiResponse Delete(int id);

        /// <summary>
        /// Możliwość ustawienia innego kontekstu niż wbudowany
        /// </summary>
        /// <param name="context">Nowy kontekst EF</param>
        void SetContext(System.Data.Entity.DbContext context);

        Campaign GetByName(string Name);
    }
}
