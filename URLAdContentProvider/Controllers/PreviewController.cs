using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using URLAdContentProvider.Models;
using URLAdContentProvider.WebServiceADContentProvider;

namespace URLAdContentProvider.Controllers
{
	public class PreviewController : Controller
	{
		#region - Fields -

		private const string GUID_KEY = "GUID";

		#endregion - Fields -

		#region - Private methods -

		/// <summary>
		/// Pobiera z pliku konfiguracyjnego adres do webserwisu WebServiceAdContentProvider
		/// </summary>
		/// <returns></returns>
		private string GetWebServiceUrl(out string error)
		{
			error = null;
			const string key = "WebServiceADContentProviderURL";
			var urlKey = Request.Url.ToString();

			if (!string.IsNullOrEmpty(urlKey))
			{
				return urlKey;
			}
			error = string.Format("W pliku konfiguracyjnym nie znaleziono klucza '{0}' definiującego URL do webSerwisu WebServiceADContentProvider", key);

			return null;
		}

		/// <summary>
		/// Zwraca ciąg, mogący stanowić parametr GET
		/// </summary>
		/// <param name="categoryCodes">Oddzielone przecinkami kody kategorii</param>
		/// <returns>np. dla parametry wejsciowego "MTR,MED" zostanie wygenerowany ciąg "&categoryCode=MTR&categoryCode=MED"</returns>
		private static string CreateCategoryCodesGetParam(string categoryCodes)
		{
			var sb = new StringBuilder();

			if (string.IsNullOrEmpty(categoryCodes)) 
				return sb.ToString();
			var codes = categoryCodes.Split(',');

			foreach (var code in codes)
			{
				if (sb.Length > 0)
				{
					sb.Append("&");
				}

				sb.Append("categoryCodes=" + code);
			}

			return sb.ToString();
		}

		/// <summary>
		/// Pobiera z sesji guida, który jest używany jako sessionID w trakcie pobierania obiektu multimedialnego z webserwisu
		/// </summary>
		/// <returns>id sesji w postaci Guid</returns>
		private string GetGuid()
		{
			if (Session != null)
			{
				if (Session[GUID_KEY] == null)
				{
					Session[GUID_KEY] = Guid.NewGuid().ToString();
				}

				return Session[GUID_KEY].ToString();
			}

			return Guid.NewGuid().ToString();
		}

		#endregion - Private methods -

		#region - Actions -

		/// <summary>
		/// Wyświetla obiekt multimedialny pobrany za pomocą GET
		/// </summary>
		/// <returns>Widok wyświetlający obiekt multimedialhy</returns>
		public ActionResult Index()
		{
			string error;

			// przygotowanie obiektu będącego parametrem wejściowym dla webserwisowej metody zwracającej obiekt multimedialny
			var request = new GetMultimediaObject_Request
			{
				//CategoryCodes = new[] {"PRA"},
				//CompanyName = "QL",
				//Height = 100,
				//Width = 100,
				//Referrer = Request.Url.AbsoluteUri,
				//SessionId = GetGuid()
			};

			var adServiceModel = new AdServiceModel
			{
				Request = request,
				ServiceUrl = GetWebServiceUrl(out error),
				ServiceMethod = "Ad"
			};
			ViewBag.CategoryCodesList = CreateCategoryCodesGetParam(adServiceModel.CategoryCodesAsString);

			return View(adServiceModel);
		}

		/// <summary>
		/// Wyświetla obiekt multimedialny pobrany za pomocą GET, na podstawie parametrów wprowadzonych przez usera
		/// </summary>
		/// <param name="adServiceModel">model</param>
		/// <returns>Widok wyświetlający obiekt multimedialny</returns>
		[HttpPost]
		public ActionResult Index(AdServiceModel adServiceModel)
		{
			string error;
			adServiceModel.ServiceUrl = GetWebServiceUrl(out error);
			adServiceModel.ServiceMethod = "Ad";
			return View(adServiceModel);
		}

		#endregion - Actions -
	}
}