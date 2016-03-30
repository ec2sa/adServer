using ADServerDAL.Entities.Presentation;
using System;
using System.Linq;

namespace ADServerDAL.Abstract
{
    /// <summary>
    /// Interfejs factory obsługujący powiązanie statystyk z kampaniami
    /// </summary>
    public interface IStatistics_CategoryRepository : IDisposable
    {
        /// <summary>
        /// Kolekcja dostępnych statystyk
        /// </summary>
        IQueryable<Statistics_CategoryItem> Statistics_Category { get; }

        /// <summary>
        /// Możliwość ustawienia innego kontekstu niż wbudowany
        /// </summary>
        /// <param name="context">Nowy kontekst EF</param>
        void SetContext(System.Data.Entity.DbContext context);
    }
}
