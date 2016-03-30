using ADServerDAL.Abstract;
using ADServerDAL.Entities;
using ADServerDAL.Entities.Presentation;
using ADServerDAL.Filters;
using ADServerDAL.Models;
using ADServerDAL.Models.Base;
using ADServerManagementWebApplication.Extensions;
using ADServerManagementWebApplication.Infrastructure;
using ADServerManagementWebApplication.Infrastructure.ErrorHandling;
using ADServerManagementWebApplication.Models;
using System.Linq;
using System.Web.Mvc;
using Antlr.Runtime.Misc;

namespace ADServerManagementWebApplication.Controllers
{
	/// <summary>
	/// Kontroler do obsługi zestawień
	/// </summary>
	[Authorize]
	public class StatisticsStatementsController : AdServerBaseController
	{
		#region - Fields -
		/// <summary>
		/// Repozytorium statystyk
		/// </summary>
		private IStatisticRepository _repository;

		private readonly IUsersRepository _usersRepository;
		#endregion

		#region - Constructors -
		public StatisticsStatementsController(IStatisticRepository repository, IUsersRepository userRepository)
		{
			_repository = repository;
			_usersRepository = userRepository;
		}
		#endregion

		#region - Actions -

		/// <summary>
		/// Akcja domyślna
		/// </summary>
		[AdServerActionException]
		public ActionResult Index()
		{
			var id = User.GetUserIDInt();
			var u = _usersRepository.Users.Single(it => it.Id == id);
			ViewBag.AdPoints = u.AdPoints;
			return View();
		}


		/// <summary>
		/// Zestawienia dla obiektów multimedialnych
		/// </summary>
		/// <param name="page">Numer żądanej strony</param>
		/// <param name="sortExpression">Pole do sortowania</param>
		/// <param name="ascending">Czy sortować rosnąco</param>
		[AdServerActionException]
		public ActionResult MultimediaObjectStatement(int? page,
								  string sortExpression,
								  bool? ascending)
		{

			// Odtwórz zapamiętane filtry
			StatisticsStatementListViewModelFilter filter = null;

			if (Session != null)
			{
				filter = Session[FilterSettingsKey.StatisticsStatementControllerFilterList.ToString()] as StatisticsStatementListViewModelFilter;
			}

			if (filter == null)
			{
				filter = new StatisticsStatementListViewModelFilter();
				var filtr2 = new StatisticsListViewModelFilter
				{
					FilterDateFrom = filter.FilterDateFrom,
					FilterDateTo = filter.FilterDateTo
				};
				Session[FilterSettingsKey.ObjDetailsFilterList.ToString()] = filtr2;
			}

			// Zapamiętaj aktualne filtry
			if (Session != null)
			{
				Session[FilterSettingsKey.StatisticsStatementControllerFilterList.ToString()] = filter;

				PageSettings.RemoveFromSessionExcept(PageSettingsKey.StatisticsStatementObjectsPageSettings);
				//FilterSettings.RemoveFromSessionExcept(FilterSettingsKey.StatisticsStatementControllerFilterList);

				PageSettings pageSettings = PageSettings.GetFromSession(PageSettingsKey.StatisticsStatementObjectsPageSettings);
				if (pageSettings != null)
				{
					if (!page.HasValue)
					{
						page = pageSettings.Page;
					}
					if (!ascending.HasValue)
					{
						ascending = pageSettings.Accending;
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
				pageSettings.Accending = !ascending.HasValue || ascending.Value;

				Session[PageSettingsKey.StatisticsStatementObjectsPageSettings.ToString()] = pageSettings;
			}

			// Zbuduj i zwróć model
			StatisticsStatementListViewModel model = CreateModel(StatisticsStatementType.MultimediaObject, page, sortExpression, ascending, filter);

			var id = User.GetUserIDInt();
			var u = _usersRepository.Users.Single(it => it.Id == id);
			ViewBag.AdPoints = u.AdPoints;

			return View("Index", model);
		}


		/// <summary>
		/// Zestawienia dla kampanii
		/// </summary>
		/// <param name="Page">Numer żądanej strony</param>
		/// <param name="SortExpression">Pole do sortowania</param>
		/// <param name="Ascending">Czy sortować rosnąco</param>
		[AdServerActionException]
		public ActionResult CampaignStatement(int? Page,
								  string SortExpression,
								  bool? Ascending)
		{
			// Odtwórz zapamiętane filtry
			StatisticsStatementListViewModelFilter filter = null;
			if (Session != null)
			{
				filter = Session[FilterSettingsKey.StatisticsStatementControllerFilterList.ToString()] as StatisticsStatementListViewModelFilter;
			}

			if (filter == null)
			{
				filter = new StatisticsStatementListViewModelFilter();
			}

			// Zapamiętaj aktualne filtry
			if (Session != null)
			{
				Session[FilterSettingsKey.StatisticsStatementControllerFilterList.ToString()] = filter;

				PageSettings.RemoveFromSessionExcept(PageSettingsKey.StatisticsStatementCampaignsPageSettings);
				FilterSettings.RemoveFromSessionExcept(FilterSettingsKey.StatisticsStatementControllerFilterList);

				PageSettings pageSettings = PageSettings.GetFromSession(PageSettingsKey.StatisticsStatementCampaignsPageSettings);
				if (pageSettings != null)
				{
					if (!Page.HasValue)
					{
						Page = pageSettings.Page;
					}
					if (!Ascending.HasValue)
					{
						Ascending = pageSettings.Accending;
					}
					if (string.IsNullOrEmpty(SortExpression))
					{
						SortExpression = pageSettings.SortExpression;
					}
				}
				else
				{
					pageSettings = new PageSettings();
				}

				pageSettings.Page = Page ?? 1;
				pageSettings.Accending = !Ascending.HasValue || Ascending.Value;
				// pageSettings.SortExpression = string.IsNullOrEmpty(SortExpression) ? "Id" : SortExpression;

				Session[PageSettingsKey.StatisticsStatementCampaignsPageSettings.ToString()] = pageSettings;
			}

			// Zbuduj i zwróć model
			StatisticsStatementListViewModel model = CreateModel(StatisticsStatementType.Campaign, Page, SortExpression, Ascending, filter);
			
			var id = User.GetUserIDInt();
			var u = _usersRepository.Users.Single(it => it.Id == id);
			ViewBag.AdPoints = u.AdPoints;

			return View("Index", model);
		}

		[AdServerActionException]
		public ActionResult DeviceStatement(int? page, string sortExpression, bool? ascending)
		{

			var ids = User.GetUserIDInt();
			var u = _usersRepository.Users.Single(it => it.Id == ids);
			ViewBag.AdPoints = u.AdPoints;

			var id = User.GetUserIDInt();
			if (User.IsInRole("Admin"))
			{
				id = 0;
			}
			var viewModel =
				CreateModel<StatisticsStatementListViewModel, StatisticsStatementListViewModelFilter, StatisticsStatementItem>(
					FilterSettingsKey.DevsListViewModelFilter, PageSettingsKey.StatisticsStatementDevicePageSettings, page, sortExpression,
					ascending, _repository.StatisticStatementDevice(id));

			return View("IndexSingle", viewModel);
		}

		[AdServerActionException]
		[HttpPost]
		public ActionResult List(StatisticsStatementListViewModelFilter model)
		{
			// Zapamiętanie aktualnych filtrów
			if (Session != null)
			{
				Session[FilterSettingsKey.DevsListViewModelFilter.ToString()] = model;
				PageSettings.RemoveFromSession(PageSettingsKey.StatisticsStatementDevicePageSettings);
			}
			return Json(true);
		}

		/// <summary>
		/// Lista zestawień wybranego typu (dla filtrowania)
		/// </summary>
		/// <param name="model"></param>
		[AdServerActionException]
		[HttpPost]
		[MultipleButton(Name = "action", Argument = "List")]
		public ActionResult List(StatisticsStatementListViewModel model)
		{
			// Zapamiętaj filtry
			if (Session != null)
			{
				Session[FilterSettingsKey.StatisticsStatementControllerFilterList.ToString()] = model.Filters;

				PageSettings.RemoveFromSession(PageSettingsKey.StatisticsStatementCampaignsPageSettings);
				PageSettings.RemoveFromSession(PageSettingsKey.StatisticsStatementObjectsPageSettings);

				Session[FilterSettingsKey.DevDetailsFilterList.ToString()] = new StatisticsListViewModelFilter
				{
					FilterDateFrom = model.Filters.FilterDateFrom,
					FilterDateTo = model.Filters.FilterDateTo
				};
				PageSettings.RemoveFromSession(PageSettingsKey.DevDetailsSettings);
			}

			// Wybierz i zbuduj model
			var actionName = ChooseAction(model.StatementType);
			return RedirectToAction(actionName);
		}

		/// <summary>
		/// Wczyść filtry
		/// </summary>
		/// <param name="model">Obiekt modelu</param>
		[AdServerActionException]
		[HttpPost]
		[MultipleButton(Name = "action", Argument = "ClearFilters")]
		public ActionResult ClearFilters(StatisticsStatementListViewModel model)
		{
			// Usuń filtry
			if (Session != null)
			{
				Session.Remove(FilterSettingsKey.StatisticsStatementControllerFilterList.ToString());
				PageSettings.RemoveFromSession(PageSettingsKey.StatisticsStatementCampaignsPageSettings);
				PageSettings.RemoveFromSession(PageSettingsKey.StatisticsStatementObjectsPageSettings);
				PageSettings.RemoveFromSession(PageSettingsKey.DevDetailsSettings);
				Session.Remove(FilterSettingsKey.DevDetailsFilterList.ToString());
			}

			// Wybierz i zwróć model
			var actionName = ChooseAction(model.StatementType);
			return RedirectToAction(actionName);
		}

		[AdServerActionException]
		[HttpPost]
		public ActionResult CmpStatement(int id, int? page, string sortExpression, bool? ascending)
		{
			var u = User.GetUserIDInt();
			var user = _usersRepository.Users.SingleOrDefault(it => it.Id == u);
			if (user == null)
				return null;
			if (!User.IsInRole("Admin") && user.Campaigns.SingleOrDefault(it => it.Id == id) == null)
				return null;

			ViewBag.Action = "CmpStatement";
            ViewBag.Id = id;
            ViewBag.Prefix = "Cmp";
            ViewBag.InnerId = id;
			var viewModel =
				CreateModel<StatisticsListViewModel, StatisticsListViewModelFilter, Statistic>(
					FilterSettingsKey.DevDetailsFilterList, PageSettingsKey.DevDetailsSettings, page, sortExpression,
					ascending, _repository.CmpStatistics(id), FilterSettingsDetailsVirtual);
			return PartialView("DetailsList", viewModel);

		}
		[AdServerActionException]
		[HttpPost]
		public ActionResult ObjStatement(int id, int? page, string sortExpression, bool? ascending)
		{
			var u = User.GetUserIDInt();
			var user = _usersRepository.Users.SingleOrDefault(it => it.Id == u);
			if (user == null)
				return null;
			if (!User.IsInRole("Admin") && user.MultimediaObjects.SingleOrDefault(it => it.Id == id) == null)
				return null;
			ViewBag.Action = "ObjStatement";
            ViewBag.Id = id;
            ViewBag.Prefix = "Obj";
            ViewBag.InnerId = id;
			var viewModel =
				CreateModel<StatisticsListViewModel, StatisticsListViewModelFilter, Statistic>(
					FilterSettingsKey.DevDetailsFilterList, PageSettingsKey.DevDetailsSettings, page, sortExpression,
					ascending, _repository.ObjStatistics(id), FilterSettingsDetailsVirtual);

			return PartialView("DetailsList", viewModel);
		}

		[AdServerActionException]
		[HttpPost]
		public ActionResult DevStatement(int id, int? page, string sortExpression, bool? ascending)
		{
			var u = User.GetUserIDInt();
			var user = _usersRepository.Users.SingleOrDefault(it => it.Id == u);
			if (user == null)
				return null;
			if (!User.IsInRole("Admin") && user.Devices.SingleOrDefault(it => it.Id == id) == null)
				return null;
			ViewBag.Action = "DevStatement";
            ViewBag.Id = id;
            ViewBag.Prefix = "Dev";
            ViewBag.InnerId = id;
			var viewModel =
				CreateModel<StatisticsListViewModel, StatisticsListViewModelFilter, Statistic>(
					FilterSettingsKey.DevDetailsFilterList, PageSettingsKey.DevDetailsSettings, page, sortExpression,
					ascending, _repository.DevStatistics(id), FilterSettingsDetailsVirtual);
			return PartialView("DetailsList", viewModel);
		}

		[AdServerActionException]
		[HttpPost]
		public ActionResult DevReport(int id)
		{
			var u = User.GetUserIDInt();
			var user = _usersRepository.Users.SingleOrDefault(it => it.Id == u);
			if (user == null)
				return null;
			if (!User.IsInRole("Admin") && user.Devices.SingleOrDefault(it => it.Id == id) == null)
				return null;
			ViewBag.Action = "DevReport";
			ViewBag.Id = id;
			ViewBag.Prefix = "Dev";
			ViewBag.InnerId = id;

			var viewModel = _repository.ReportStatistic(id);

			return PartialView("DevReport", viewModel);
		}
		#endregion

		#region - Private methods -

		/// <summary>
		/// Wybiera odpowiedną akcję
		/// </summary>
		/// <param name="statementType"></param>
		private string ChooseAction(StatisticsStatementType statementType)
		{
			switch (statementType)
			{
				// Zestawienia kampanii
				case StatisticsStatementType.Campaign:
					return "CampaignStatement";

				// Zestawienia obiektów multimedialnych
				case StatisticsStatementType.MultimediaObject:
					return "MultimediaObjectStatement";

				default:
					return "Index";
			}
		}


		/// <summary>
		/// Zbudowanie modelu
		/// </summary>
		/// <param name="statementType">Typ zestawienia</param>
		/// <param name="Page">Numer żądanej strony</param>
		/// <param name="SortExpression">Pole do sortowania</param>
		/// <param name="Ascending">Czy sortować rosnąco</param>
		/// <param name="filter">Obiekt filtrów</param>
		private StatisticsStatementListViewModel CreateModel(StatisticsStatementType statementType,
																  int? Page,
																  string SortExpression,
																  bool? Ascending,
																  StatisticsStatementListViewModelFilter filter)
		{
			var page = Page ?? 1;
			var sortExpression = string.IsNullOrEmpty(SortExpression) ? "TotalDisplayCount" : SortExpression;
			var ascending = Ascending.HasValue && Ascending.Value;
			const int itemsPerPage = 10;

			// Zbuduj obiekt informacyjny dla stronnicowania
			var paginationInfo = new AdPaginationInfo
			{
				Accending = ascending,
				ItemsPerPage = itemsPerPage,
				RequestedPage = page,
				SortExpression = sortExpression
			};

			var statements = _repository.StatisticsStatements(filter, paginationInfo, statementType, (User.IsInRole("Admin") ? 0 : User.GetUserIDInt()));

			var model = new StatisticsStatementListViewModel
			{
				StatementType = statementType,
				Statement = statements.ToList(),
				CurrentPage = paginationInfo.RequestedPage,
				SortExpression = paginationInfo.SortExpression,
				NumberOfResults = paginationInfo.OutResultsFound,
				ItemsPerPage = paginationInfo.ItemsPerPage,
				SortAccending = paginationInfo.Accending,
				Filters = filter ?? new StatisticsStatementListViewModelFilter()
			};
			// Określ adres powrotny w zależności od wybranego rodzaju zestawienia
			switch (statementType)
			{
				case StatisticsStatementType.Campaign:
					model.DestinationURL = "Campaign";
					model.StatementTitle = "Zestawienie kampanii";
					break;

				case StatisticsStatementType.MultimediaObject:
					model.DestinationURL = "MultimediaObjects";
					model.StatementTitle = "Zestawienie obiektów multimedialnych";
					break;

				default:
					model.StatementTitle = "Zestawienie";
					break;
			}

			return model;
		}

		#endregion

		#region - Overriden methods -
		/// <summary>
		/// Zwolnienie zasobów
		/// </summary>
		protected override void OnDisposeController()
		{
			if (_repository != null)
			{
				_repository.Dispose();
				_repository = null;
			}
		}

		/// <summary>
		/// Customowe filtrowanie
		/// </summary>
		/// <typeparam name="T">Typ filtru</typeparam>
		/// <param name="_query">Zbiór encji</param>
		/// <param name="_filter">filtr</param>
		/// <returns></returns>
		protected override T FilterSettingsVirtual<T, Q>(ref IQueryable<Q> _query, T _filter)
		{
			var query = (IQueryable<StatisticsStatementItem>)_query;
			dynamic filter1 = _filter;
			StatisticsStatementListViewModelFilter filter = filter1;
			var doFiltering = filter != null && filter.Filtering;
			if (doFiltering)
			{
				if (!string.IsNullOrEmpty(filter.FilterName))
				{
					query = query.Where(x => x.Name.ToLower().Contains(filter.FilterName.ToLower()));
				}
			}

			_query = (IQueryable<Q>)query;
			filter1 = filter;

			return (T)filter1;
		}

		/// <summary>
		/// Customowe filtrowanie
		/// </summary>
		/// <typeparam name="T">Typ filtru</typeparam>
		/// <param name="_query">Zbiór encji</param>
		/// <param name="_filter">filtr</param>
		/// <returns></returns>
		protected T FilterSettingsDetailsVirtual<T, Q>(ref IQueryable<Q> _query, T _filter)
		{
			var query = (IQueryable<Statistic>)_query;
			dynamic filter1 = _filter;
			StatisticsListViewModelFilter filter = filter1;
			var doFiltering = filter != null && filter.Filtering;
			if (doFiltering)
			{
				if (filter.FilterDateFrom.HasValue)
				{
					var dtS = filter.FilterDateFrom.Value.Date;
					query = query.Where(q => dtS <= q.RequestDate || dtS <= q.ResponseDate);
				}

				if (filter.FilterDateTo.HasValue)
				{
					var dtE = filter.FilterDateTo.Value.Date.AddDays(1).AddSeconds(-1);
					query = query.Where(q => dtE >= q.RequestDate || dtE >= q.ResponseDate);
				}

				if (!string.IsNullOrEmpty(filter.FilterMultimediaObjectName))
				{
					query = query.Where(q => q.MultimediaObject.Name.ToLower().Contains(filter.FilterMultimediaObjectName.ToLower()));
				}

				if (filter.FilterMultimediaObjectId.HasValue)
				{
					query = query.Where(q => q.MultimediaObjectId == filter.FilterMultimediaObjectId.Value);
				}

				if (!string.IsNullOrEmpty(filter.FilterRequestIP))
				{
					query = query.Where(q => q.RequestIP.ToLower().Contains(filter.FilterRequestIP.ToLower()));
				}
				if (!string.IsNullOrEmpty(filter.FilterCampaignName))
				{
					query = query.Where(q => q.Campaign.Name.ToLower().Contains(filter.FilterCampaignName.ToLower()));
				}
			}

			_query = (IQueryable<Q>)query;
			filter1 = filter;

			return (T)filter1;
		}
		#endregion
	}
}