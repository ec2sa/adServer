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
    /// Interfejs factory obsługujący kategorie
    /// </summary>
    public interface ICategoryRepository : IDisposable
    {
        /// <summary>
        /// Kolekcja wszystkich kategorii
        /// </summary>
        IQueryable<Category> Categories { get; }

        /// <summary>
        /// Zwraca identyfikatory kategorii powiązanych z daną kampanią
        /// </summary>
        /// <param name="campaignId">Identyfikator kampanii</param>        
        List<int> CategoriesToCampaign(int campaignId);

        /// <summary>
        /// Zapisuje kategorię do bazy danych
        /// </summary>
        /// <param name="category">Obiekt kategorii do zapisu</param>        
        ApiResponse Save(Category category);

        /// <summary>
        /// Zwraca obiekt kategorii na podstawie zadanego identyfikatora
        /// </summary>
        /// <param name="id">Identyfikator kategorii</param>        
        Category GetById(int id);

        /// <summary>
        /// Usuwa kategorię z bazy danych na podstawie zadanego identyfikatora
        /// </summary>
        /// <param name="id">Identyfikator kategorii</param>        
        ApiResponse Delete(int id);

        /// <summary>
        /// Możliwość ustawienia innego kontekstu niż wbudowany
        /// </summary>
        /// <param name="context">Nowy kontekst EF</param>
        void SetContext(System.Data.Entity.DbContext context);
    }
}
