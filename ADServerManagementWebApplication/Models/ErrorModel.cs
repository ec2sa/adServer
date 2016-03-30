
namespace ADServerManagementWebApplication.Models
{
    /// <summary>
    /// Model do przekazywania informacji o nieobsłużonym błędzie
    /// </summary>
    public class ErrorModel
    {
        /// <summary>
        /// Pełna treść błędu (komunikaty zagnieżdżone, stack trace)
        /// </summary>
        public string Log { get; set; }

        /// <summary>
        /// Główny komunikat błędu
        /// </summary>
        public string PrimaryMessage { get; set; }
    }
}