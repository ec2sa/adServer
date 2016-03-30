using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADServerManagementWebApplication.Infrastructure
{
    /// <summary>
    /// Klasa do przechowywania informacji o zapamiętanych ustawieniach list (numer aktualnej strony, sortowane pole, kierunek sortowania)
    /// </summary>
    public class PageSettings
    {
        /// <summary>
        /// Numer zapamiętanej strony
        /// </summary>
        public int Page { get; set; }


        /// <summary>
        /// Pole sortowane
        /// </summary>
        public string SortExpression { get; set; }


        /// <summary>
        /// Kierunek sortowania
        /// </summary>
        public bool Accending { get; set; }


        /// <summary>
        /// Pobiera obiekt ustawień strony z sesji na podstawie zadanego klucza
        /// </summary>
        /// <param name="key">Klucz identyfikacyjny</param>        
        public static PageSettings GetFromSession(PageSettingsKey key)
        {
            PageSettings pageSettings = null;
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null)
            {
                if (System.Web.HttpContext.Current.Session[key.ToString()] != null)
                {
                    pageSettings = (PageSettings)System.Web.HttpContext.Current.Session[key.ToString()];
                }
            }
            return pageSettings;
        }

        /// <summary>
        /// Usuwana obiekty ustawień stron z sesji za wyjątkiem zadanego klucza
        /// </summary>
        /// <param name="exception">Wyjątek - klucz, który nie zostanie usunięty z sesji</param>
        public static void RemoveFromSessionExcept(PageSettingsKey exception)
        {
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null)
            {
                string[] namesToRemove = Enum.GetNames(typeof(PageSettingsKey)).Except(new string[] { exception.ToString() }).ToArray();
                foreach (string name in namesToRemove)
                {
                    if (System.Web.HttpContext.Current.Session[name] != null)
                    {
                        System.Web.HttpContext.Current.Session[name] = null;
                        System.Web.HttpContext.Current.Session.Remove(name);
                    }
                }
            }
        }

        /// <summary>
        /// Usuwa obiekt ustawień strony z sesji dla zadanego klucza
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveFromSession(PageSettingsKey key)
        {
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null)
            {
                if (System.Web.HttpContext.Current.Session[key.ToString()] != null)
                {
                    System.Web.HttpContext.Current.Session[key.ToString()] = null;
                    System.Web.HttpContext.Current.Session.Remove(key.ToString());
                }
            }
        }

    }
}