using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ADServerDAL
{
    /// <summary>
    /// Rozszerzenie encji statystyki
    /// </summary>
    public partial class Statistics
    {
        /// <summary>
        /// Definicja rodzaju źródła żądania
        /// </summary>
        public enum RequestSourceType
        {
            /// <summary>
            /// Aplikacje WWW
            /// </summary>            
            WWW = 0,

            /// <summary>
            /// Aplikacje desktop
            /// </summary>
            Desktop = 1
        }
    }
}
