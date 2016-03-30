using System.Configuration;
using System.Web.Http;

namespace ADServerManagementWebApplication.Infrastructure
{
    /// <summary>
    /// Bazowy kontroler API
    /// </summary>
    public class AdServerBaseApiController : ApiController
    {
        #region - Overriden methods -
        protected override void Dispose(bool disposing)
        {
            OnDisposeController();

            base.Dispose(disposing);
        } 
        #endregion

        #region - Virtual methods -
        /// <summary>
        /// Metoda pozwalająca na zwolenienie zasobów kontrolerów
        /// </summary>
        protected virtual void OnDisposeController()
        {

        } 
        #endregion

        #region - Properties
        /// <summary>
        /// Określa czy pliki mają być przechowywane w FILESTREAM
        /// </summary>
        public bool FILESTREAM_OPTION
        {
            get
            {
                var key = "FILESTREAM_OPTION";
                var urlKey = ConfigurationManager.AppSettings[key];

                if (urlKey != null && !string.IsNullOrEmpty(urlKey))
                {
                    bool value = false;
                    bool.TryParse(urlKey.ToString(), out value);
                    return value;
                }

                return false;
            }
        }
        #endregion
    }
}