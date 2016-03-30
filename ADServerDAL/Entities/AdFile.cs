using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADServerDAL.Entities
{
    /// <summary>
    /// Klasa pomocnicza dla algorytmu wybierania obiektu multimedialnego
    /// </summary>
    public class AdFile
    {
        /// <summary>
        /// Identyfikator obiektu multimedialnego
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Nazwa obiektu multimedialnego
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Zawartość obiektu
        /// </summary>
        public byte[] Contents { get; set; }

        /// <summary>
        /// Typ mime obiektu
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// Szerokość obiektu
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Wysokość obiektu
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Status kampanii, do której należy obiekt
        /// </summary>
        public int StatusCode { get; set; }

		/// <summary>
		/// Adres odnośnika
		/// </summary>
		public string URL { get; set; }

	    public int CmpId { get; set; }
    }
}
