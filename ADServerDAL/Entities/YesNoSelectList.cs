using System.Collections.Generic;

namespace ADServerDAL.Entities
{
    /// <summary>
    /// Klasa pomocniczna dla list wyboru typu TAK/NIE
    /// </summary>
    public class YesNoDictionary
    {
        /// <summary>
        /// Zwraca słownik opcji TAK/NIE z możliwością określenia wyświetlanego nagłówka
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, bool> GetList()
        {
            var items = new Dictionary<string, bool> {{"nie", false}, {"tak", true}};
            return items;
        }
    }
}
