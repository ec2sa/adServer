using ADServerDAL.Entities;
using ADServerDAL.Entities.Presentation;
using ADServerDAL.Filters;
using ADServerDAL.Helpers;
using ADServerDAL.Models.Base;
using ADServerManagementWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace ADServerManagementWebApplication.Infrastructure
{
	/// <summary>
	/// Bazowy kontroler
	/// </summary>
	public class AdServerBaseController : Controller
	{
		#region - Fields -

		private const string MenuKey = "MENU";
		private const string MenuXmlFilePath = "~/Infrastructure/Menu.xml";

		private static class Message
		{
			public const string Error = "Error";
			public const string Information = "Info";
			public const string Warning = "Warn";
		}

		/// <summary>
		/// Sesja dla środowiska testowego
		/// </summary>
		private HttpSessionStateBase _session;

		/// <summary>
		/// Domyślna liczba elementów na stronie dla list
		/// </summary>
		private int _itemsPerPage = 6;

		#endregion - Fields -

		#region - Properties -

		/// <summary>
		/// Na potrzeby mocka i testów jednostkowych w środowisku nie web-owym
		/// </summary>
		public new HttpSessionStateBase Session
		{
			get { return _session ?? (_session = base.Session ?? new CustomSession()); }
		}

		/// <summary>
		/// Domyślna liczba elementów na stronie dla list
		/// </summary>
		public int ItemsPerPage
		{
			get { return _itemsPerPage; }
			set
			{
				if (_itemsPerPage != value)
				{
					_itemsPerPage = value;
				}
			}
		}

		/// <summary>
		/// Określa czy pliki mają być przechowywane w FILESTREAM
		/// </summary>
		public bool FilestreamOption
		{
			get
			{
				const string key = "FILESTREAM_OPTION";
				var urlKey = ConfigurationManager.AppSettings[key];

				if (urlKey == null || string.IsNullOrEmpty(urlKey))
					return false;

				bool value;
				bool.TryParse(urlKey, out value);
				return value;
			}
		}

		#endregion - Properties -

		#region - Virtual methods-

		/// <summary>
		/// Metoda pozwalająca na zwolenienie zasobów
		/// </summary>
		protected virtual void OnDisposeController()
		{
		}

		#endregion - Virtual methods-

		#region - protected methods -

		/// <summary>
		/// Wyliczenie liczby elementów pomijanych na liście dla stronnicowania
		/// </summary>
		/// <param name="page">Żądany numer strony</param>
		/// <param name="count">Liczba wszystkich rekordów</param>
		/// <param name="itemsCount">Liczba rekordów na stronie</param>
		/// <param name="correntPageNo"></param>
		protected int GetSkip(int page, int count, int itemsCount, out int correntPageNo)
		{
			correntPageNo = page;

			if (page < 2)
			{
				return 0;
			}
			var skip = (page - 1) * count;

			if (skip < itemsCount)
				return skip;

			correntPageNo = 0;
			if (itemsCount != 0)
			{
				correntPageNo = ((int)Math.Ceiling(itemsCount / (double)count));
			}

			if (correntPageNo == 0) correntPageNo = 1;

			skip = (correntPageNo - 1) * count;
			return skip;
		}
		protected delegate T delegateFilter<T, Q>(ref IQueryable<Q> _query, T _filter);

		protected T CreateModel<T, U, Q>(
			FilterSettingsKey filterKey, PageSettingsKey settingsKey,
			int? page,
			string sortExpression,
			bool? accending,
			IQueryable<Q> repo)
			where T : ListViewModel
			where U : ViewModelFilterBase
			where Q : Entity
		{
			return CreateModel<T, U, Q>(filterKey, settingsKey, page, sortExpression, accending, repo, FilterSettingsVirtual);
		}
		/// <summary>
		/// Tworzenie modelu
		/// </summary>
		/// <typeparam name="T">Typ obiektu</typeparam>
		/// <typeparam name="U">TYp filtra</typeparam>
		/// <param name="filterKey">Klucz do zapisanego filtra w sesji</param>
		/// <param name="settingsKey">Klucz do zapisanych stawień w sesji</param>
		/// <param name="page">Strona</param>
		/// <param name="sortExpression">Wyrażenie sortowania</param>
		/// <param name="accending">rosnące?</param>
		/// <param name="repo">Analizowany zbiór</param>
		/// <returns></returns>
		protected T CreateModel<T, U, Q>(
										FilterSettingsKey filterKey, PageSettingsKey settingsKey,
										int? page,
										string sortExpression,
										bool? accending,
										IQueryable<Q> repo, delegateFilter<U,Q> method)
			where T : ListViewModel
			where U : ViewModelFilterBase
			where Q : Entity
		{
			var filter = PageSettingsOrder<U>(filterKey, settingsKey, ref page, ref sortExpression, ref accending);

			page = page ?? 1;
			sortExpression = string.IsNullOrEmpty(sortExpression) ? "Id" : sortExpression;
			accending = !accending.HasValue || accending.Value;

			// Przygotowanie informacji o stronnicowaniu
			var paginationInfo = new AdPaginationInfo
			{
				Accending = (bool)accending,
				ItemsPerPage = ItemsPerPage,
				RequestedPage = (int)page,
				SortExpression = sortExpression
			};

			// Pobranie danych z repozytorium
			var query = repo;

			filter = method(ref query, filter);

			paginationInfo.OutResultsFound = query.Count();
			int correctPageNo;
			var skip = GetSkip(paginationInfo.RequestedPage, paginationInfo.ItemsPerPage, paginationInfo.OutResultsFound, out correctPageNo);

			paginationInfo.RequestedPage = correctPageNo;

			if (PageSettings.GetFromSession(PageSettingsKey.CampaignCategoriesPageSettings) != null)
			{
				PageSettings.GetFromSession(PageSettingsKey.CampaignCategoriesPageSettings).Page = paginationInfo.RequestedPage;
			}

			query = query.OrderBy(paginationInfo.SortExpression, paginationInfo.Accending).Skip(skip).Take(paginationInfo.ItemsPerPage);
			var test = query.ToList();

			var ret = Activator.CreateInstance<T>();

			// Zbudowanie modelu listy
			ret.Query = (IQueryable<Entity>)query;
			ret.CurrentPage = paginationInfo.RequestedPage;
			ret.SortExpression = paginationInfo.SortExpression;
			ret.NumberOfResults = paginationInfo.OutResultsFound;
			ret.ItemsPerPage = paginationInfo.ItemsPerPage;
			ret.SortAccending = paginationInfo.Accending;
			ret.FilerBase = filter;

			return ret;
		}

		/// <summary>
		/// Customowe filtrowanie
		/// </summary>
		/// <typeparam name="T">Typ filtru</typeparam>
		/// <param name="repo">Zbiór encji</param>
		/// <param name="filter">filtr</param>
		/// <returns></returns>
		protected virtual T FilterSettingsVirtual<T, Q>(ref IQueryable<Q> _query, T _filter)
			where T : ViewModelFilterBase
			where Q : Entity
		{
			return Activator.CreateInstance<T>();
		}

		#endregion - protected methods -

		#region - Private methods -

		/// <summary>
		/// Zbudowanie menu na podstawie pliku XML
		/// </summary>
		private List<Menu> CreateMenu()
		{
			var doc = new XmlDocument();
			doc.Load(ControllerContext.HttpContext.Server.MapPath(MenuXmlFilePath));
			var root = doc.SelectSingleNode("/menu");
			var menu = CreateMenu(root);
			return menu;
		}

		/// <summary>
		/// Zbudowanie menu na podstawie pliku XML
		/// </summary>
		private static List<Menu> CreateMenu(XmlNode root)
		{
			var menu = new List<Menu>();
			var options = root.SelectNodes("option");

			if (options != null && options.Count > 0)
			{
				foreach (XmlNode o in options)
				{
					if (o.Attributes["name"] == null)
						continue;

					var m = new Menu();

					if (!o.HasChildNodes)
					{
						// Pobranie informacji o akcji i kontrolerze
						m.Action = o.Attributes["action"] == null ? string.Empty : o.Attributes["action"].Value;
						m.Controller = o.Attributes["controller"] == null ? string.Empty : o.Attributes["controller"].Value;
						m.OptionName = o.Attributes["name"] == null ? string.Empty : o.Attributes["name"].Value;
						m.Role = o.Attributes["Role"] == null ? new string[0] : o.Attributes["Role"].Value.Split(';');
						m.OnClick = o.Attributes["onClick"] == null ? "return true;" : o.Attributes["onClick"].Value.Replace("\\n", "<br />");
						m.ListView = o.Attributes["ListView"] != null && o.Attributes["ListView"].Value == "true";
						bool activeForAllActions;

						// Sprawdzenie czy opcja dostepna dla akcji
						if (o.Attributes["activeForAllAction"] != null && bool.TryParse(o.Attributes["activeForAllAction"].Value, out activeForAllActions))
						{
							m.ActiveForAllAction = activeForAllActions;
						}

						menu.Add(m);
					}
					else
					{
						m.Action = null;
						m.Controller = null;
						m.OptionName = o.Attributes["name"] == null ? string.Empty : o.Attributes["name"].Value;
						m.Submenu = CreateMenu(o);
						m.Role = o.Attributes["Role"] == null ? new string[0] : o.Attributes["Role"].Value.Split(';');
						menu.Add(m);
					}
				}
			}

			return menu;
		}

		/// <summary>
		/// Methoda zwraca filtr
		/// </summary>
		/// <typeparam name="T">Typ filtru</typeparam>
		/// <param name="filterKey">Klucz do zapisanego filtra w sesji</param>
		/// <param name="settingsKey">Klucz do zapisanych stawień w sesji</param>
		/// <param name="page">Strona</param>
		/// <param name="sortExpression">Wyrażenie sortowania</param>
		/// <param name="accending">rosnące?</param>
		/// <returns></returns>
		private T PageSettingsOrder<T>(FilterSettingsKey filterKey, PageSettingsKey settingsKey, ref int? page, ref string sortExpression, ref bool? accending)
		{
			var filter = default(T);
			// Pobranie aktualnych filtrów z sesji
			if (Session != null)
			{
				filter = (T)Session[filterKey.ToString()];
			}

			if (filter == null)
			{
				filter = Activator.CreateInstance<T>();
			}

			if (Session != null)
			{
				Session[filterKey.ToString()] = filter;

				PageSettings.RemoveFromSessionExcept(settingsKey);
				FilterSettings.RemoveFromSessionExcept(filterKey);

				var pageSettings = PageSettings.GetFromSession(settingsKey);
				if (pageSettings != null)
				{
					if (!page.HasValue)
					{
						page = pageSettings.Page;
					}
					if (!accending.HasValue)
					{
						accending = pageSettings.Accending;
					}
					if (string.IsNullOrEmpty(sortExpression))
					{
						sortExpression = pageSettings.SortExpression;
					}
				}
				else
				{
					pageSettings = new PageSettings();
				}

				pageSettings.Page = page ?? 1;
				pageSettings.Accending = !accending.HasValue || accending.Value;
				pageSettings.SortExpression = string.IsNullOrEmpty(sortExpression) ? "Id" : sortExpression;

				Session[settingsKey.ToString()] = pageSettings;
			}
			return filter;
		}

		#endregion - Private methods -

		#region - Overriden methods -

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (Session[MenuKey] == null)
			{
				Session[MenuKey] = CreateMenu();
			}

			if (Session[MenuKey] != null)
			{
				ViewBag.MenuOptions = Session[MenuKey] as List<Menu>;
			}
		}

		/// <summary>
		/// Zwalnianie zasobów
		/// </summary>
		protected sealed override void Dispose(bool disposing)
		{
			OnDisposeController();
			base.Dispose(disposing);
		}

		#endregion - Overriden methods -

		public void Error(string error)
		{
			if (TempData[Message.Error] == null)
				TempData[Message.Error] = new List<string>();

			((List<string>)(TempData[Message.Error])).Add(error);
		}

		public void Information(string error)
		{
			if (TempData[Message.Information] == null)
				TempData[Message.Information] = new List<string>();

			((List<string>)(TempData[Message.Information])).Add(error);
		}

		public void Warning(string error)
		{
			if (TempData[Message.Warning] == null)
				TempData[Message.Warning] = new List<string>();

			((List<string>)(TempData[Message.Warning])).Add(error);
		}

		public void PropError(IEnumerable<ApiValidationErrorItem> errors)
		{
			foreach (var r in errors)
			{
				var key = string.IsNullOrEmpty(r.Property) ? "" : (r.Property);
				ModelState.AddModelError(key, r.Message);
			}
		}
	}

	/// <summary>
	/// Klasa pomocnicza do obsługi sesji w środowisku nie-webowym
	/// </summary>
	public class CustomSession : HttpSessionStateBase
	{
		#region - Fields -

		/// <summary>
		/// Słownik symulujący sesję
		/// </summary>
		private readonly Dictionary<string, object> _dictionary;

		#endregion - Fields -

		#region - Properties -

		/// <summary>
		/// Słownik symulujący sesję
		/// </summary>
		public override object this[string name]
		{
			get
			{
				return _dictionary.ContainsKey(name) ? _dictionary[name] : null;
			}
			set
			{
				if (!_dictionary.ContainsKey(name))
				{
					_dictionary.Add(name, value);
				}
				else
				{
					_dictionary[name] = value;
				}
			}
		}

		#endregion - Properties -

		#region - Constructors -

		public CustomSession()
		{
			_dictionary = new Dictionary<string, object>();
		}

		#endregion - Constructors -
	}
}