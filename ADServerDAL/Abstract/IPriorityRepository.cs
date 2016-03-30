
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
    /// Interfejs factory obsługuący priorytety
    /// </summary>
    public interface IPriorityRepository : IDisposable
    {
        /// <summary>
        /// Kolekcja wszystkich priorytetów
        /// </summary>
        IQueryable<Priority> Priorities { get; }

        /// <summary>
        /// Pobiera encję priorytetu wg podanego identyfikatora
        /// </summary>
        /// <param name="id">Identyfikator priorytetu</param>        
        Priority GetById(int id);

        /// <summary>
        /// Usuwa wskazany priorytet z bazy danych
        /// </summary>
        /// <param name="id">Identyfikator priorytetu</param>
        ApiResponse Delete(int id);

        /// <summary>
        /// Zapisuje dany priorytet do bazy danych
        /// </summary>
        /// <param name="priority">Obiekt priorytetu</param>
        ApiResponse Save(Priority priority);

        /// <summary>
        /// Możliwość ustawienia innego kontekstu niż wbudowany
        /// </summary>
        /// <param name="context">Nowy kontekst EF</param>
        void SetContext(System.Data.Entity.DbContext context);
    }
}
