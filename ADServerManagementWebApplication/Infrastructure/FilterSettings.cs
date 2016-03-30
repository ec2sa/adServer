using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADServerManagementWebApplication.Infrastructure
{
    public static class FilterSettings
    {
        /// <summary>
        /// Usuwana obiekty filtrów z sesji za wyjątkiem zadanego klucza
        /// </summary>
        /// <param name="exception">Wyjątek - klucz, który nie zostanie usunięty z sesji</param>
        public static void RemoveFromSessionExcept(FilterSettingsKey exception)
        {
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null)
            {
                string[] namesToRemove = Enum.GetNames(typeof(FilterSettingsKey)).Except(new string[] { exception.ToString() }).ToArray();
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
    }
}