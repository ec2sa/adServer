using ADServerDAL.Abstract;
using ADServerDAL.Filters;
using ADServerDAL.Models;
using ADServerManagementWebApplication.Extensions;
using ADServerManagementWebApplication.Infrastructure;
using ADServerManagementWebApplication.Infrastructure.ErrorHandling;
using ADServerManagementWebApplication.Models;
using System.Linq;
using System.Web.Mvc;

namespace ADServerManagementWebApplication.Controllers
{
	/// <summary>
	/// Kontrol do obsługi statystyk
	/// </summary>
	[Authorize(Roles = "Admin")]
	public class StatisticsController : AdServerBaseController
	{
		#region - Fields -
		/// <summary>
		/// Repozytorium statystyk
		/// </summary>
		private IStatisticRepository repository = null;

		private IUsersRepository _usersRepository;
		#endregion

		#region - Constructors -
		public StatisticsController(IStatisticRepository repository, IUsersRepository usersRepository)
		{
			this.repository = repository;
			_usersRepository = usersRepository;
		}
		public int UserID { get { return User.GetUserIDInt(); } }
		public bool roleAdmin { get { return User.IsInRole("Admin"); } }

		#endregion
		
		#region - Actions -

		/// <summary>
		/// Lista statystyk
		/// </summary>
		/// <param name="page">Numer żądanej strony</param>
		/// <param name="sortExpression">Pole do sortowania</param>
		/// <param name="ascending">Czy sortować rosnąco</param>
		[AdServerActionExceptionAttribute]
		public ActionResult Index(int? page,
								  string sortExpression,
								  bool? ascending)
		{
			ViewBag.Prefix = "1";
			if (Request.HttpMethod == "GET")
				return RedirectToAction("Index", "Default", new { act = "Index", ctr = "Statistics" });

			var viewModel = CreateModel<StatisticsListViewModel, StatisticsListViewModelFilter, Statistic>(FilterSettingsKey.StatisticsControllerFilterList, PageSettingsKey.StatisticsPageSettings, page, sortExpression, ascending, repository.Statistics);

			var id = User.GetUserIDInt();
			var u = _usersRepository.Users.Single(it => it.Id == id);
			ViewBag.AdPoints = u.AdPoints;

			return View("Index", viewModel);
		}

		/// <summary>
		/// Lista statystyka (dla filtrowania)
		/// </summary>
		/// <param name="model">Obiekt modelu</param>
		[AdServerActionExceptionAttribute]
		[HttpPost]
		public ActionResult List(StatisticsListViewModelFilter model)
		{
			if (Session != null)
			{
				Session[FilterSettingsKey.StatisticsControllerFilterList.ToString()] = model;
				PageSettings.RemoveFromSession(PageSettingsKey.StatisticsPageSettings);
			}

			return Json(true);
		}

		/// <summary>
		/// Wyczyszczenie filtrów
		/// </summary>
		/// <param name="model"></param>
		[AdServerActionExceptionAttribute]
		[HttpPost]
		public ActionResult ClearFilters(StatisticsListViewModel model)
		{
			// Usuń filtry z sesji
			if (Session != null)
			{
				Session.Remove(FilterSettingsKey.StatisticsControllerFilterList.ToString());
				PageSettings.RemoveFromSession(PageSettingsKey.StatisticsPageSettings);
			}
			return RedirectToAction("Index", "Default", new { ctr = "Statistics" });
		}
		#endregion

		#region Overriden methods

		/// <summary>
		/// Customowe filtrowanie
		/// </summary>
		/// <typeparam name="T">Typ filtru</typeparam>
		/// <param name="_query">Zbiór encji</param>
		/// <param name="_filter">filtr</param>
		/// <returns></returns>
		protected override T FilterSettingsVirtual<T, Q>(ref IQueryable<Q> _query, T _filter)
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

		/// <summary>
		/// Zwalnianie zasobów
		/// </summary>
		protected override void OnDisposeController()
		{
			if (repository != null)
			{
				repository.Dispose();
				repository = null;
			}
		}

		#endregion
	}
}