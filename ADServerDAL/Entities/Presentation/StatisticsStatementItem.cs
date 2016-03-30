using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ADServerDAL.Models;
using ADServerDAL.Models.Base;

namespace ADServerDAL.Entities.Presentation
{
   
    /// <summary>
    /// Reprezentacja obiektu zestawienia
    /// </summary>
    public class StatisticsStatementItem : Entity
    {
        /// <summary>
        /// Całkowita liczba wyświetleń
        /// </summary>
        public int TotalDisplayCount { get; set; }

        /// <summary>
        /// Liczba wyświetleń z aplikacji WWW
        /// </summary>
        public int WWWDisplayCount { get; set; }

        /// <summary>
        /// Liczba wyświetleń z aplikacji desktop
        /// </summary>
        public int DesktopDisplayCount { get; set; }

        /// <summary>
        /// Rodzaj zestawienia
        /// </summary>
        public StatisticsStatementType Type { get; set; }

        /// <summary>
        /// Lista kategorii
        /// </summary>
        public Dictionary<string, int> Categories { get; set; }

		/// <summary>
		/// Ilość przekierowań
		/// </summary>
		public int ClickedCount { get; set; }

		/// <summary>
		/// Ilość wykorzystanych AdPoints
		/// </summary>
		public decimal AdPointsCount { get; set; }
		
		/// <summary>
		/// AdPoints kampanii
		/// </summary>
		public decimal? cAdPoints { get; set; }

	    public IQueryable<Statistic> Statistics { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}] {1}  | Total: {2}| WWW: {3} | Desktop: {4} | Clicked {5}", this.Type.ToString(),
                                                                                    this.Name,
                                                                                    this.TotalDisplayCount,
                                                                                    this.WWWDisplayCount,
                                                                                    this.DesktopDisplayCount,
																					this.ClickedCount);
        }
    }


    /// <summary>
    /// Definicja typów zestawień
    /// </summary>
    public enum StatisticsStatementType
    {
        /// <summary>
        /// Obiekty
        /// </summary>
        MultimediaObject,

        /// <summary>
        /// Kampanie
        /// </summary>
        Campaign,
		
		/// <summary>
		/// Nośniki
		/// </summary>
		Device
    }
}