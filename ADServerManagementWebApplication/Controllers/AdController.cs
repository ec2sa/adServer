using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ADEngineMultimediaSelectionAlgorythm;
using ADServerDAL.Concrete;
using ADServerDAL.Entities;
using ADServerDAL.Helpers;
using ADServerDAL.Models;
using System.Web.Security;

namespace ADServerManagementWebApplication.Controllers
{
    [AllowAnonymous]
    public class AdController : Controller
	{
		#region - Fields -

		private static readonly object LogToken = new object();

		#endregion - Fields -

		#region - Actions -

		/// <summary>
		/// Zwraca Id obiektu MM
		/// </summary>
		/// <param name="categoryCodes">Lista kodów kategorii, do których ma należeń wyszukany obiekt</param>
		/// <param name="width">Szerokość, jaką musi mieć wyszukany obiekt</param>
		/// <param name="height">Wysokość, jaką musi mieć wyszukany obiekt</param>
		/// <param name="companyName">Nazwa firmy klienta</param>
		/// <param name="sessionId">identyfikator sesji, w ramach której ma się odbyć wyszukanie obiektu</param>
		/// <param name="Id">Id nośnika</param>
		/// <param name="viewId">Identyfikator obiektu</param>
		/// <returns>Zawartość pliku obiektu multimedialnego (w postaci bajtów)</returns>
		[OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
		public ActionResult Index(string data0, string data1, string data2, string data3, int Id, string viewId)
		{
			var errors = new List<string>();
			var nameCookie = "AdServer" + viewId;
			try
			{
				var selectionRequest = new MultimediaObjectSelection.MultimediaObjectsSelectionParams
				{
					ID = Id,
					Data0 = data0,
					Data1 = data1,
					Data2 = data2,
					Data3 = data3,
					RequestDate = DateTime.Now,
					RequestSource = (int)Statistic.RequestSourceType.WWW
				};
				var ips = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
				selectionRequest.RequestIP = !string.IsNullOrEmpty(ips) ? ips.Split(',')[0] : Request.ServerVariables["REMOTE_ADDR"];

				var sessionId = Request.Cookies.Get(nameCookie) == null ? Guid.NewGuid().ToString() : Request.Cookies[nameCookie]["sessionId"];

				selectionRequest.SessionId = sessionId;

				var cookie = new HttpCookie(nameCookie);
				cookie.Values.Add("sessionId", sessionId);

				using (var ctx = new AdServContext())
				{
					var repositories = EFRepositorySet.CreateRepositorySet(ctx);

					try
					{
						var mos = new MultimediaObjectSelection(repositories);
                        List<string> err = new List<string>();

						const string key = "FILESTREAM_OPTION";
						var urlKey = ConfigurationManager.AppSettings[key];

						bool filestreamOption = false;
                        if (urlKey != null && !string.IsNullOrEmpty(urlKey))
                        {
                            bool.TryParse(urlKey, out filestreamOption);
                        }

                        bool add = true;
                        if (Request.UrlReferrer != null)
                        {
                            if (Request.Url.Host == Request.UrlReferrer.Host)
                            {
                                add = false;
                            }
                        }
						AdFile response = mos.GetMultimediaObject(selectionRequest, filestreamOption,add, out err);
						cookie.Values.Add("cmp", response.CmpId.ToString());

						if (err != null && err.Count > 0)
						{
							errors.AddRange(err);
						}
						else
						{
							cookie.Values.Add("Id", response.ID.ToString());
							cookie.Values.Add("StatusCode", response.StatusCode.ToString());
							Response.AppendCookie(cookie);
							return File(response.Contents, response.MimeType);
						}
					}
					catch (Exception ex)
					{
						var hierarchy = new List<Exception>();
						ExceptionsHandlingHelper.HierarchizeError(ex, ref hierarchy);
						if (hierarchy.Count > 0)
						{
							errors.AddRange(hierarchy.Select(s => s.Message + Environment.NewLine + s.StackTrace).Distinct().AsEnumerable());
						}

						SaveErrorInLogFile(selectionRequest, ex);
					}
				}
			}
			catch (Exception ex)
			{
				var hierarchy = new List<Exception>();
				ExceptionsHandlingHelper.HierarchizeError(ex, ref hierarchy);
				if (hierarchy.Count > 0)
				{
					errors.AddRange(hierarchy.Select(s => s.Message + Environment.NewLine + s.StackTrace).Distinct().AsEnumerable());
				}

				SaveErrorInLogFile(ex);
			}
			return null;
		}

		/// <summary>
		/// Zwraca url obiektu oraz aktualizuje statystyke
		/// </summary>
		/// <returns>Zawartość pliku obiektu multimedialnego (w postaci bajtów)</returns>
		[OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
		public ActionResult URL(int Id, string viewId = "")
		{
			var nameCookie = "AdServer" + viewId;
			var cookie = Request.Cookies[nameCookie];
            //if (cookie == null)
            //    return Redirect("http://" + Request.Url.Authority);
			using (var ctx = new AdServContext())
			{
				var repositories = EFRepositorySet.CreateRepositorySet(ctx);

				try
				{
					var selectionRequest = new MultimediaObjectSelection.MultimediaObjectsSelectionParams
					{
						RequestDate = DateTime.Now,
						SessionId = string.IsNullOrEmpty(cookie.Values["sessionId"]) ? "web request" : cookie.Values["sessionId"],
						RequestSource = (int)Statistic.RequestSourceType.WWW,
						ID = Id
					};

					var ips = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
					selectionRequest.RequestIP = !string.IsNullOrEmpty(ips) ? ips.Split(',')[0] : Request.ServerVariables["REMOTE_ADDR"];

					var mos = new MultimediaObjectSelection(repositories);

					const string key = "FILESTREAM_OPTION";
					var urlKey = ConfigurationManager.AppSettings[key];

					if (urlKey != null && !string.IsNullOrEmpty(urlKey))
					{
						bool filestreamOption;
						bool.TryParse(urlKey, out filestreamOption);
					}

                    bool add = true;
                    if (Request.UrlReferrer != null)
                    {
                        if (Request.Url.Host == Request.UrlReferrer.Host)
                        {
                            add = false;
                        }
                    }
                    var response = mos.GetMmObjectUrl(int.Parse(cookie.Values["Id"]), int.Parse(cookie.Values["StatusCode"]), int.Parse(cookie.Values["cmp"]), selectionRequest,add);
                        if (response != null)
                            return Redirect(response);
					Redirect(Request.Url.Authority);
				}
				catch
				{
				}
			}

			return Redirect(Request.Url.Authority);
		}

		#endregion - Actions -

		#region - Private methods -

		/// <summary>
		/// Zapisuje do pliku informację o wyjątku
		/// </summary>
		/// <param name="selectionParams">Parametry requesta</param>
		/// <param name="ex">Obiekt wyjątku</param>
		private static void SaveErrorInLogFile(MultimediaObjectSelection.MultimediaObjectsSelectionParams selectionParams, Exception ex)
		{
			try
			{
				if (selectionParams == null || ex == null)
					return;

				var exceptions = new List<Exception>();
				ExceptionsHandlingHelper.HierarchizeError(ex, ref exceptions);

				var message = string.Format("{3}Exception: {0}. User agent: {1}, Request source: {2}{3}{4}{3}{3}",
											ex.Message,
											selectionParams.RequestSource,
											Environment.NewLine,
											ex.StackTrace,
											DateTime.Now);

				var innerMessages = new List<string>();
				if (exceptions.Count > 0)
				{
					innerMessages.AddRange(exceptions.Select(exInner => Environment.NewLine + "[MESSAGE] " + exInner.Message + Environment.NewLine + exInner.StackTrace));
				}

				if (innerMessages.Count > 0)
				{
					message += Environment.NewLine + Environment.NewLine + string.Join(Environment.NewLine, innerMessages.ToArray());
					innerMessages.Clear();
				}

				SaveMessageInLogFile(message);
			}
			catch (Exception err)
			{
				System.Diagnostics.Trace.TraceError(err.Message);
			}
		}

		/// <summary>
		/// Zapisuje do pliku informację o wyjątku
		/// </summary>
		/// <param name="ex">Obiekt wyjątku</param>
		private static void SaveErrorInLogFile(Exception ex)
		{
			try
			{
				if (ex == null)
					return;

				var message = string.Format("{0}{1}Exception: {2}. {3}{3}",
											DateTime.Now,
											Environment.NewLine,
											ex.Message,
											ex.StackTrace);

				var exceptions = new List<Exception>();
				ExceptionsHandlingHelper.HierarchizeError(ex, ref exceptions);

				var innerMessages = new List<string>();
				if (exceptions.Count > 0)
				{
					innerMessages.AddRange(exceptions.Select(exInner => ex.Message + Environment.NewLine + ex.StackTrace));
				}

				if (innerMessages.Count > 0)
				{
					message += Environment.NewLine + Environment.NewLine + string.Join(Environment.NewLine, innerMessages.ToArray());
					innerMessages.Clear();
				}

				SaveMessageInLogFile(message);
			}
			catch (Exception err)
			{
				System.Diagnostics.Trace.TraceError(err.Message);
			}
		}

		/// <summary>
		/// Zapisuje komunikat w pliku z logami.
		/// </summary>
		/// <param name="messageToLog">Komunikat, jaki ma zostać zalogowany w pliku logów</param>
		private static void SaveMessageInLogFile(string messageToLog)
		{
			try
			{
				if (messageToLog == null && messageToLog.Length > 0)
					return;

				var logPath = System.Web.HttpContext.Current.Server.MapPath("~/Logs");
				try
				{
					// Próba utworzenia katalogu z logami
					if (!Directory.Exists(logPath))
					{
						Directory.CreateDirectory(logPath);
					}
				}
				catch (Exception ex)
				{
					logPath = System.Web.HttpContext.Current.Server.MapPath("~/");
					System.Diagnostics.Trace.TraceError(ex.Message);
				}

				lock (LogToken)
				{
					// Zapisanie informacji do pliku
					StreamWriter sw;

					var fileName = DateTime.Now.ToShortDateString() + "_Log.txt";

					using (sw = new StreamWriter(logPath + "\\" + fileName, true, Encoding.Default))
					{
						sw.WriteLine(messageToLog);
						sw.WriteLine(new string('*', 50));
						sw.Flush();
					}

					try
					{
						sw.Close();
					}
					catch
					{ }
				}
			}
			catch (Exception err)
			{
				System.Diagnostics.Trace.TraceError(err.Message);
			}
		}

		#endregion - Private methods -
	}
}