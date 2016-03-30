using ADServerDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceADContentProvider
{
    public class GetMultimediaObject_Response
    {
        #region - Properties -
        /// <summary>
        /// Obiekt multimedialny zwrócony przez webservice
        /// </summary>
        public AdFile File { get; set; }

		/// <summary>
		/// Adres URL odnośnika
		/// </summary>
		public string URL { get; set; }

		/// <summary>
		/// Id obiektu
		/// </summary>
		public int ID { get; set; }

        /// <summary>
        /// Komunikaty o błędach
        /// </summary>
        public List<string> ErrorMessage { get; set; }

        /// <summary>
        /// Informacja, czy pojawiły się błędy, czy może nie
        /// </summary>
        public bool ErrorsOccured { get; set; }
        #endregion
    }
}