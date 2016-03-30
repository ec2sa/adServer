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
    public class EFStatistic_CategoryRepository : EFBaseRepository, IStatistics_CategoryRepository
    {
        /// <summary>
        /// Kolekcja dostępnych powiązań statystyka-kategoria
        /// </summary>
        public IQueryable<Statistics_CategoryItem> Statistics_Category
        {
            get
            {
	            var q1 = 
						from s in Context.Categories
						from ss in s.Statistics
						select new {s.Id, ss};

                var query = from q in q1
                            select new Statistics_CategoryItem
                            {
                                ID = q.Id,
                                CategoryId = q.Id,
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
