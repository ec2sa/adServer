using ADServerDAL.Entities.Presentation;
using System;
using System.Linq;

namespace ADServerDAL.Abstract
{
    /// <summary>
    /// Interfejs factory obsługujący powiązania kampanii z kategoriami
    /// </summary>
    public interface ICampaign_CategoryRepository : IDisposable
    {
        /// <summary>
        /// Kolekcja wszystkich obiektów będących powiazaniami kampanii z kategorią
        /// </summary>
        IQueryable<Campaign_CategoryItem> Campaign_Category { get; }

        /// <summary>
        /// Możliwość ustawienia innego kontekstu niż wbudowany
        /// </summary>
        /// <param name="context">Nowy kontekst EF</param>
        void SetContext(System.Data.Entity.DbContext context);
    }
}
