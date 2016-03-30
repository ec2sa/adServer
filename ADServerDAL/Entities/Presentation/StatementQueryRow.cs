using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADServerDAL.Entities.Presentation
{
    /// <summary>
    /// Klasa pomocnicza wykorzystywana w zestawieniach
    /// </summary>
    public class StatementQueryRow
    {
        /// <summary>
        /// Identyfikator rekordu statyki
        /// </summary>
        public int StatisticId { get; set; }

        /// <summary>
        /// Identyfikator źródła
        /// </summary>
        public int Source { get; set; }

        /// <summary>
        /// Identyfikator obiektu
        /// </summary>
        public int ObjectId { get; set; }

        /// <summary>
        /// Nazwa obiektu
        /// </summary>
        public string ObjectName { get; set; }

		/// <summary>
		/// Przekierowano
		/// </summary>
		public bool Clicked { get; set; }

		/// <summary>
		/// Wykorzystane AdPoints
		/// </summary>
		public decimal AdPoints { get; set; }

		/// <summary>
		/// AdPoints kampanii
		/// </summary>
		public decimal cAdPoints { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}] Id: {1} | StatID: {2} | Source: {3} | Clicked: {4}", ObjectName, ObjectId, StatisticId, Source, Clicked);
        }
    }
}
