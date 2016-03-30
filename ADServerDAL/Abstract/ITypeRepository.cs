using ADServerDAL.Entities;
using ADServerDAL.Entities.Presentation;
using ADServerDAL.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADServerDAL.Abstract
{
    /// <summary>
    /// Interfejs factory obsługujący typy obiektów mulimedialnych
    /// </summary>
    public interface ITypeRepository : IDisposable
    {
        /// <summary>
        /// Kolekcja dostępnych typów
        /// </summary>
        IQueryable<Models.Type> Types { get; }
        
        /// <summary>
        /// Pobiera prezenter typ na podstawie zadanego identyfikatora
        /// </summary>
        /// <param name="id">Identyfikator typu</param>
        MultimediaTypeItem GetPresenterById(int id);

        /// <summary>
        /// Pobiera obiekt typu na podstawie zadanego identyfikatora
        /// </summary>
        /// <param name="id">Identyfikator typu</param>        
        Models.Type GetById(int id);

        /// <summary>
        /// Zapisywanie typu do bazy danych
        /// </summary>
        /// <param name="type">Obiekt typu</param>        
		ApiResponse Save(Models.Type type);

        /// <summary>
        /// Usuwanie typu z bazy danych na podstawie zadanego identyfikatora
        /// </summary>
        /// <param name="id">Identyfikator typu</param>
        ApiResponse Delete(int id);

        /// <summary>
        /// Możliwość ustawienia innego kontekstu niż wbudowany
        /// </summary>
        /// <param name="context">Nowy kontekst EF</param>
        void SetContext(System.Data.Entity.DbContext context);
    }
}
