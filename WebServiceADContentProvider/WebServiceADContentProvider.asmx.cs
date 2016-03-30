using System.Web;
using ADEngineMultimediaSelectionAlgorythm;
using ADServerDAL;
using ADServerDAL.Concrete;
using ADServerDAL.Entities;
using ADServerDAL.Helpers;
using ADServerDAL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;

namespace WebServiceADContentProvider
{
	/// <summary>
	/// Summary description for WebServiceADContentProvider
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
	[ScriptService]
	public class WebServiceADContentProvider : WebService
	{
		#region - Fields -
		private static readonly object LogToken = new object();
		#endregion

		#region - Public methods -

		/// <summary>
		/// Zwraca do klienta obiekt multimedialny, wyszukany w bazie w oparciu o parametry obiektu requestowego.
		/// Plik obiektu multimedialnego jest zwracany w postaci bajtów zapisanych w Context.Response.BinaryWrite(response.File.Contents)
		/// </summary>
		/// <param name="sessionId">Identyfikator sesji, w ramach której ma nastąpićwyszukanie obiektu</param>
		/// <param name="categoryCodes">Lista kategorii, do których musi należeń wyszukany obiekt</param>
		/// <param name="width">Szerokość, jaką musi mieć wyszukany obiekt</param>
		/// <param name="height">Wysokość, jaką powinien mieć wyszukany obiekt</param>
		/// <param name="referrer">Nośnik/źródło żądania</param>
		/// <param name="firstName">Imię klienta</param>
		/// <param name="lastName">Nazwisko klienta</param>
		/// <param name="companyName">Nazwa firmy klienta</param>
		/// <param name="email">Email klienta</param>
		/// <param name="pesel">Pesel klienta</param>
		/// <param name="additionalInfo">Informacje dodatkowe</param>
		/// <param name="userId">Id reklamodawcy</param>
		[WebMethod]
		public int GetMultimediaObjectBytes(string sessionId,
											int id,
											string Data0,
											string Data1,
											string Data2,
											string Data3,
											DateTime date,
											int requestType)
		{
			List<string> errors = null;

			try
			{
				var request = new GetMultimediaObject_Request
				{
					ID = id,
					SessionId = sessionId,
					Data0 = Data0,
					Data1 = Data1,
					Data2 = Data2,
					Data3 = Data3,
					RequestDate = date,
					RequestSource = requestType
				};
				// pobranie obiektu multimedialnego
				var response = GetMultimediaObject(request, out errors);

				if (response != null)
				{
					// jeśli podczas pobrania obiektu wystąpiły błędy, zapisujemy je do pliku z logami
					if (errors != null && errors.Any())
					{
						foreach (var error in response.ErrorMessage)
						{
							SaveMessageInLogFile(error);
						}

						errors.Clear();
					}

					// jeśli istnieje plik obiektu multimedialnego, zwracamy go w postaci bajtów
					if (response.File != null && response.File.Contents != null)
					{
						Context.Response.ContentType = response.File.MimeType;
						Context.Response.BinaryWrite(response.File.Contents);
						Context.Response.StatusCode = 200;
						return 1;
					}
				}
			}
			catch (Exception ex)
			{
				var hierarchy = new List<Exception>();
				ExceptionsHandlingHelper.HierarchizeError(ex, ref hierarchy);

				if (hierarchy.Count > 0)
				{
					if (errors == null)
					{
						errors = new List<string>();
					}

					errors.AddRange(hierarchy.Select(s => s.Message + Environment.NewLine + s.StackTrace).Distinct().AsEnumerable());
				}

				SaveErrorInLogFile(ex);
			}

			return 0;
		}

		/// <summary>
		/// Zwraca do klienta obiekt multimedialny, wyszukany w bazie w oparciu o parametry obiektu requestowego
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[WebMethod]
		[ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
		public string GetMultimediaObject(GetMultimediaObject_Request request)
		{
			List<string> errors;
			GetMultimediaObject_Response response = GetMultimediaObject(request, out errors);
			string result = SerializeResponse(response, errors);
			return result;
		}
		[WebMethod]
		public string MultimediaObjectUrlClicked(GetMultimediaObject_Request request, int id, int statusCode)
		{
			var ret = "";
			try
			{
				StaticClicked(request, id, statusCode);
			}
			catch (Exception e)
			{
				ret = e.Message;
			}
			return ret;
		}
		#endregion

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

				var message = string.Format("{5}{3}Exception: {0}. User agent: {1}, Request source: {2}{3}{4}{3}{3}",
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


				string logPath = System.Web.HttpContext.Current.Server.MapPath("~/Logs");
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

					string fileName = DateTime.Now.ToShortDateString() + "_Log.txt";

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
					catch { }
				}
			}
			catch (Exception err)
			{
				System.Diagnostics.Trace.TraceError(err.Message);
			}
		}

		/// <summary>
		/// Pobiera obiekt multimedialny, który zostanie zwrócony do klienta
		/// </summary>
		/// <param name="request">Parametry, na podtswie których zostanie przeprowadzone wyszukiwanie obiektu multimedialnego</param>
		/// <param name="errors">Lista komunikaktów błędów - jesli wystąpiły błędy</param>
		/// <returns>Wyszukany obiekt multimedialny opakowany w obiekt odpowiedniego typu</returns>
		private static GetMultimediaObject_Response GetMultimediaObject(GetMultimediaObject_Request request, out List<string> errors)
		{
			var response = new GetMultimediaObject_Response();
			errors = new List<string>();
			var hierarchy = new List<Exception>();
			try
			{
				if (request != null)
				{
					var selectionRequest = new MultimediaObjectSelection.MultimediaObjectsSelectionParams
					{
						Data0 = request.Data0,
						Data1 = request.Data1,
						Data2 = request.Data2,
						Data3 = request.Data3,
						ID = request.ID,
						RequestDate = DateTime.Now,
						SessionId = request.SessionId,
						RequestSource =
							(int)
								(System.Web.HttpContext.Current.Request.UserAgent == null
									? Statistic.RequestSourceType.Desktop
									: Statistic.RequestSourceType.WWW),
						RequestIP = HttpContext.Current.Request.UserHostAddress
					};
					using (var ctx = new AdServContext())
					{
						var repositories = EFRepositorySet.CreateRepositorySet(ctx);

						try
						{
							var mos = new MultimediaObjectSelection(repositories);
							List<string> err;
							const string key = "FILESTREAM_OPTION";
							var urlKey = ConfigurationManager.AppSettings[key];

							var filestreamOption = false;
							if (urlKey != null && !string.IsNullOrEmpty(urlKey))
							{
								bool.TryParse(urlKey, out filestreamOption);

							}

							response.File = mos.GetMultimediaObject(selectionRequest, filestreamOption, out err);

							if (err != null && err.Count > 0)
							{
								errors.AddRange(err);
							}
						}
						catch (Exception ex)
						{
							ExceptionsHandlingHelper.HierarchizeError(ex, ref hierarchy);
							if (hierarchy.Count > 0)
							{
								errors.AddRange(hierarchy.Select(s => s.Message + Environment.NewLine + s.StackTrace).Distinct().AsEnumerable());
							}
							SaveErrorInLogFile(selectionRequest, ex);
						}
					}
				}
				else
				{
					errors.Add("Parametr requesta nie może być nullem.");
				}

			}
			catch (Exception ex)
			{
				ExceptionsHandlingHelper.HierarchizeError(ex, ref hierarchy);
				if (hierarchy.Count > 0)
				{
					errors.AddRange(hierarchy.Select(s => s.Message + Environment.NewLine + s.StackTrace).Distinct().AsEnumerable());
				}
				SaveErrorInLogFile(ex);
			}

			return response;
		}

		/// <summary>
		/// Serializacja odpowiedzi
		/// </summary>
		/// <param name="response">Informacje do serializacji</param>
		/// <param name="errors">Lista dotychczasowych błędów</param>
		private string SerializeResponse(GetMultimediaObject_Response response, List<string> errors)
		{
			string result;

			try
			{
				// Przypisz listę błędów do obiektu
				response.ErrorsOccured = errors != null && errors.Count > 0;
				response.ErrorMessage = new List<string>();

				if (errors != null && errors.Count > 0)
				{
					response.ErrorMessage.AddRange(errors);
				}

				// Serializuj obiekt
				result = JsonConvert.SerializeObject(response);
			}

			// Serializacja nie powiodła się
			catch (Exception ex)
			{
				// Zaloguj wyjątek do pliku logów
				SaveErrorInLogFile(ex);

				// Wyczyść obiekt
				response = new GetMultimediaObject_Response();

				// Wyczyść dotychczasowe błędy
				errors = new List<string>();

				// Pobierz aktualne błędy
				var hierarchy = new List<Exception>();
				ExceptionsHandlingHelper.HierarchizeError(ex, ref hierarchy);
				if (hierarchy.Count > 0)
				{
					errors.AddRange(hierarchy.Select(s => s.Message + Environment.NewLine + s.StackTrace).Distinct().AsEnumerable());
				}

				response.ErrorsOccured = errors.Count > 0;
				response.ErrorMessage = new List<string>();
				if (errors.Count > 0)
				{
					response.ErrorMessage.AddRange(errors);
				}

				// Ponowna serializacja
				result = JsonConvert.SerializeObject(response);
			}

			return result;
		}

		private void StaticClicked(GetMultimediaObject_Request request, int id, int statusCode)
		{
			var errors = new List<string>();
			var hierarchy = new List<Exception>();
			try
			{
				if (request != null)
				{
					var selectionRequest = new MultimediaObjectSelection.MultimediaObjectsSelectionParams
					{
						Data0 = request.Data0,
						Data1 = request.Data1,
						Data2 = request.Data2,
						Data3 = request.Data3,
						ID = id,
						RequestDate = DateTime.Now,
						SessionId = request.SessionId,
						RequestSource =
							(int)
								(System.Web.HttpContext.Current.Request.UserAgent == null
									? Statistic.RequestSourceType.Desktop
									: Statistic.RequestSourceType.WWW),
						RequestIP = HttpContext.Current.Request.UserHostAddress
					};

					using (var ctx = new AdServContext())
					{
						var repositories = EFRepositorySet.CreateRepositorySet(ctx);

						try
						{
							var mos = new MultimediaObjectSelection(repositories);

							const string key = "FILESTREAM_OPTION";
							var urlKey = ConfigurationManager.AppSettings[key];

							if (urlKey != null && !string.IsNullOrEmpty(urlKey))
							{
								bool filestreamOption;
								bool.TryParse(urlKey, out filestreamOption);
							}

							mos.SaveStatisticsEntry(selectionRequest, new AdFile { ID = id, StatusCode = statusCode }, true);
						}
						catch (Exception ex)
						{
							ExceptionsHandlingHelper.HierarchizeError(ex, ref hierarchy);
							if (hierarchy.Count > 0)
							{
								errors.AddRange(hierarchy.Select(s => s.Message + Environment.NewLine + s.StackTrace).Distinct().AsEnumerable());
							}
							SaveErrorInLogFile(selectionRequest, ex);
						}
					}
				}
				else
				{
					errors.Add("Parametr requesta nie może być nullem.");
				}
			}
			catch (Exception ex)
			{
				ExceptionsHandlingHelper.HierarchizeError(ex, ref hierarchy);
				if (hierarchy.Count > 0)
				{
					errors.AddRange(hierarchy.Select(s => s.Message + Environment.NewLine + s.StackTrace).Distinct().AsEnumerable());
				}
				SaveErrorInLogFile(ex);
			}
		}
		#endregion
	}
}