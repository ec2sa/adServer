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
    /// Interfejs factory obsługujący obiekty mulimedialne
    /// </summary>
    public interface IMultimediaObjectRepository : IDisposable
    {
        /// <summary>
        /// Kolekcja wszystkich obiektów
        /// </summary>
        IQueryable<MultimediaObject> MultimediaObjects { get; }

        /// <summary>
        /// Lista identyfikatorów obiektów przypisanych do danej kampanii
        /// </summary>
        /// <param name="campaignId">Identyfikator kampanii</param>       
        IQueryable<int> ObjectsToCampaign(int campaignId);

        /// <summary>
        /// Zapisuje obiekt multimedialny do bazy
        /// </summary>
        /// <param name="multimediaObject">Obiekt multimedialny</param>
        ApiResponse Save(MultimediaObject multimediaObject);

        /// <summary>
        /// Zwraca obiekt multimedialny na podstawie identyfikatora
        /// </summary>
        /// <param name="id">Identyfikator obiektu multimedialnego</param> 
        MultimediaObject GetById(int id);

        /// <summary>
        /// Usuwa obiekt multimedialny na podstawie identyfikatora
        /// </summary>
        /// <param name="id">Identyfikator obiektu multimedialnego</param>      
        ApiResponse Delete(int id);

        /// <summary>
        /// Zwraca obiekt multimedialny z wypełnionym polem miniaturki zdjęcia
        /// </summary>
        /// <param name="id">Identyfikator obiektu multimedialnego</param> 
        MultimediaObject GetThumbnail(int id);

        /// <summary>
        /// Możliwość ustawienia innego kontekstu niż wbudowany
        /// </summary>
        /// <param name="context">Nowy kontekst EF</param>
        void SetContext(System.Data.Entity.DbContext context);
    }
}
