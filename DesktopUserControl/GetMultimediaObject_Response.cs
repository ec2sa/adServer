using System.Collections.Generic;
using DesktopUserControl;
public class GetMultimediaObject_Response
    {
        #region - Properties -
        /// <summary>
        /// Obiekt multimedialny zwrócony przez webservice
        /// </summary>
        public AdFile File { get; set; }

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