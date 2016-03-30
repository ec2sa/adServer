using ADServerDAL.Concrete;
using ADServerDAL.Helpers;
using ADServerManagementWebApplication.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace ADServerManagementWebApplication.Infrastructure.ErrorHandling
{
    /// <summary>
    /// Filtr obsługujący wyjątki w aplikacji
    /// </summary>
    public class AdServerActionExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        #region - Public methods -
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                filterContext.ExceptionHandled = true;

                ///Pobranie ścieżki do katalogu z logami
                string logPath = filterContext.HttpContext.Server.MapPath("~/Logs");
                try
                {
                    ///Próba utworzenia katalogu z logami
                    if (!System.IO.Directory.Exists(logPath))
                    {
                        System.IO.Directory.CreateDirectory(logPath);
                    }
                }
                catch (Exception ex)
                {
                    logPath = filterContext.HttpContext.Server.MapPath("~/");
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }

                string primaryMessage;

                ///Zalogowanie błędu do pliku
                string logResult = LogException(filterContext.Exception, logPath, filterContext, out primaryMessage);

                ///Zbudowanie modelu błędu
                ErrorModel model = new ErrorModel
                {
                    Log = logResult,
                    PrimaryMessage = primaryMessage
                };

                ///Przekierowanie żadania do widoku z informacją o błędzie
                filterContext.Result = new ViewResult
                {
                    ViewName = "Error",
                    ViewData = new ViewDataDictionary<ErrorModel>(model)
                };
            }
        } 
        #endregion

        #region - Private methods -
        /// <summary>
        /// Zebranie informacji do zalgowania
        /// </summary>
        /// <param name="ex">Wyjątek</param>
        /// <param name="logPath">Ścieżka do katalogu logów</param>
        /// <param name="filterContext">Kontekst wyjątku</param>
        /// <param name="primaryMessage">Główny komunikat błędu</param>
        private string LogException(Exception ex, string logPath, ExceptionContext filterContext, out string primaryMessage)
        {
            List<Exception> hierarchy = new List<Exception>();
            primaryMessage = string.Empty;

            ///Obsługa błędów bazodanowych
            if (ex is System.Data.Entity.Validation.DbEntityValidationException)
            {
                ///Pobranie informacji o błędach EF
                using (DbValidationErrorHandler dbValExp = new DbValidationErrorHandler(ex as System.Data.Entity.Validation.DbEntityValidationException))
                {
                    if (dbValExp.HasErrors)
                    {
                        foreach (var exp in dbValExp.ValidationErrors)
                        {
                            hierarchy.Add(new Exception(exp.Message));
                        }

                        primaryMessage = string.Join(Environment.NewLine, dbValExp.ValidationErrors.Select(s => s.Message).ToArray());
                    }
                    else
                    {

                        primaryMessage = ex.Message;
                        ExceptionsHandlingHelper.HierarchizeError(ex, ref hierarchy);

                    }
                }
            }

            ///Obsługa zwykłych błędów
            else
            {
                primaryMessage = ex.Message;
                ExceptionsHandlingHelper.HierarchizeError(ex, ref hierarchy);
            }
            string destinationPath = logPath;
            return LogToFile(ref hierarchy, destinationPath, null, filterContext);

        }

        /// <summary>
        /// Zalogowanie błędu do pliku
        /// </summary>
        /// <param name="hierarchy">Hierarchia wyjątków</param>
        /// <param name="logPath">Ścieżka do katalogu logów</param>
        /// <param name="fileName">Nazwa pliku logu</param>
        /// <param name="filterContext">Kontekst wyjątku</param>
        private string LogToFile(ref List<Exception> hierarchy, string logPath, string fileName, ExceptionContext filterContext)
        {
            string logResult = string.Empty;

            try
            {
                ///Pobranie nazwy pliku loga
                logPath += "\\";
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = DateTime.Now.ToShortDateString() + "_Log.txt";
                }

                List<string> content = new List<string>();

                ///Zapisanie daty wystąpienia
                content.Add(string.Format("Date:{0}", DateTime.Now.ToString()));
                try
                {
                    ///Pobranie informacji o kontrolerze
                    string controllerName = (string)filterContext.RouteData.Values["controller"];
                    content.Add(string.Format("Controller:{0}", controllerName));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }

                try
                {
                    ///Pobranie informacji o akcji
                    string actionName = (string)filterContext.RouteData.Values["action"];
                    content.Add(string.Format("Action:{0}", actionName));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }

                ///Pobranie informacji o parametrach routingu
                foreach (var rv in filterContext.RouteData.Values)
                {
                    if (rv.Key.ToLower() != "controller" && rv.Key.ToLower() != "action")
                    {
                        string key = rv.Key;
                        string val = rv.Value == null ? string.Empty : rv.Value.ToString();
                        content.Add(string.Format("Route Key:{0} Value:{1}", key, val));
                    }
                }


                var context = filterContext.HttpContext;

                ///Zapisanie query string
                if (context.Request.QueryString != null && context.Request.QueryString.Count > 0)
                {
                    List<string> queryStringList = new List<string>();
                    foreach (string key in context.Request.QueryString.AllKeys)
                    {
                        queryStringList.Add(string.Format("{0}={1}", key, context.Request.QueryString[key]));
                    }

                    content.Add(string.Format("QueryString:{0}", string.Join("&", queryStringList.ToArray())));
                }

                ///Zapisanie ścieżki aplikacji WWW
                content.Add(string.Format("Application path:{0}", context.Request.ApplicationPath));

                ///Rodzaj żądania
                content.Add(string.Format("Http method:{0}", context.Request.HttpMethod));

                ///Czy żadanie AJAX
                content.Add(string.Format("Is ajax request:{0}", context.Request.IsAjaxRequest()));

                ///Czy żadanie lokalne
                content.Add(string.Format("Is local:{0}", context.Request.IsLocal));

                ///URL żądania
                content.Add(string.Format("Url:{0}", context.Request.Url));

                //Poprzedni URL
                content.Add(string.Format("Url referrer:{0}", context.Request.UrlReferrer));

                ///Przeglądarka/klient żądania
                content.Add(string.Format("User agent:{0}", context.Request.UserAgent));

                ///Adres host żądania
                content.Add(string.Format("User host address:{0}", context.Request.UserHostAddress));

                ///Nazwa host żadania
                content.Add(string.Format("User host name:{0}", context.Request.UserHostName));

                content.Add(string.Empty);

                ///Zalogowanie hierarchii wyjątków
                if (hierarchy != null && hierarchy.Count > 0)
                {
                    foreach (Exception ex in hierarchy)
                    {
                        content.Add("Typ błędu:" + ex.GetType().FullName);

                        string msg = string.Format("{0}Message:{1}{0}Stack trace:{2}{0}{3}{0}{4}{0}{5}{0}",
                                              System.Environment.NewLine,
                                              ex.Message,
                                              ex.Source,
                                              ex.TargetSite,
                                              ex.StackTrace,
                                              new string('*', 50));
                        content.Add(msg);
                    }
                }

                logResult = string.Join(Environment.NewLine, content.ToArray());
                content.Clear();

                ///Zapisanie informacji do pliku
                StreamWriter sw = null;

                using (sw = new StreamWriter(logPath + fileName, true))
                {
                    sw.WriteLine(logResult);
                    sw.Flush();
                }

                if (sw != null)
                {
                    try
                    {
                        sw.Close();
                    }
                    catch { }

                    try
                    {
                        sw = null;
                    }
                    catch { }
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
            }

            return logResult;
        } 
        #endregion
    }
}