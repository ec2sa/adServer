using System.Net.Http;
using System.Web.Routing;
using ADServerDAL.Abstract;
using ADServerDAL.Entities;
using ADServerDAL.Filters;
using ADServerDAL.Models;
using ADServerManagementWebApplication.Extensions;
using ADServerManagementWebApplication.Infrastructure;
using ADServerManagementWebApplication.Infrastructure.ErrorHandling;
using ADServerManagementWebApplication.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ADServerManagementWebApplication.Controllers
{
	/// <summary>
	/// Kontroler do obsługi kampanii
	/// </summary>
	[Authorize]
	public class CampaignController : AdServerBaseController
	{
		#region - Fields -

		/// <summary>
		/// Repozytorium kampanii
		/// </summary>
		private ICampaignRepository _repository;

		/// <summary>
		/// Repozytorium priorytetów
		/// </summary>
		private IPriorityRepository _priorityRepository;

		private readonly IUsersRepository _usersRepository;

		private string UserRole
		{
			get { return User.GetRole(); }
		}

		public int UserID { get { return User.GetUserIDInt(); } }

		#endregion - Fields -

		#region - Constructors -

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="repository">Repozytorium kampanii</param>
		/// <param name="priorityRepository">Repozytorium priorytetów</param>
		/// <param name="usersRepository">Repozytorium użytkowników</param>
		public CampaignController(ICampaignRepository repository, IPriorityRepository priorityRepository, IUsersRepository usersRepository)
		{
			_repository = repository;
			_priorityRepository = priorityRepository;
			_usersRepository = usersRepository;
		}

		#endregion - Constructors -

		#region - Actions -
		
		/// <summary>
		/// Lista kampanii
		/// </summary>
		/// <param name="page">Numer żądanej strony</param>
		/// <param name="sortExpression">Pole do sortowania</param>
		/// <param name="accending">Czy sortować rosnąco</param>
		[AdServerActionException]
		public ActionResult Index(int? page,
								  string sortExpression,
								  bool? ascending)
		{
			if (Request.HttpMethod == "GET")
				return RedirectToAction("Index", "Default", new {act = "Index", ctr = "Campaign"});
			User.UpdateAdPoints(_usersRepository);

			var viewModel =
				CreateModel<CampaignListViewModel, CampaignListViewModelFilter, Campaign>(
					FilterSettingsKey.CampaignControllerFilterList, PageSettingsKey.CampaignPageSettings, page, sortExpression,
					ascending, _repository.Campaigns);

			var id = User.GetUserIDInt();
			var u = _usersRepository.Users.Single(it => it.Id == id);
			ViewBag.AdPoints = u.AdPoints;

			return View("Index", viewModel);
		}

		/// <summary>
		/// Lista kampanii (jako odpowiedź na filtrowanie);
		/// </summary>
		/// <param name="model"></param>
		[AdServerActionException]
		[HttpPost]
		public ActionResult List(CampaignListViewModelFilter model)
		{
			if (Session == null)
				return Json(true);

			Session[FilterSettingsKey.CampaignControllerFilterList.ToString()] = model;
			PageSettings.RemoveFromSession(PageSettingsKey.CampaignPageSettings);
			return Json(true);
		}

		/// <summary>
		/// Przywrócenie informacji o słownikach potrzebnych do zbudowania modelu
		/// </summary>
		/// <param name="model">Obiekt modelu</param>
		private void RestoreDictionariesState(CampaignListViewModel model)
		{
			if (model.Filters == null)
				return;

			if (model.Filters.Priorities == null)
			{
				var selectedId = model.Filters.FilterPriorityId;
				model.Filters.Priorities = new SelectList(_priorityRepository.Priorities, "Id", "Name");
				model.Filters.FilterPriorityId = selectedId;
			}

			if (model.Filters.YesNo == null)
			{
				var active = model.Filters.FilterActive;
				model.Filters.YesNo = new SelectList(YesNoDictionary.GetList(), "Value", "Key");
				model.Filters.FilterActive = active;
			}
		}

		/// <summary>
		/// Wyczyszczenie filtrów
		/// </summary>
		/// <param name="model">Obiekt modelu</param>
		[AdServerActionException]
		[HttpPost]
		public ActionResult ClearFilters(CampaignListViewModel model)
		{
			// Usunięcie filtrów z sesji
			if (Session != null)
			{
				Session.Remove(FilterSettingsKey.CampaignControllerFilterList.ToString());
				PageSettings.RemoveFromSession(PageSettingsKey.CampaignPageSettings);
			}
			return RedirectToAction("Index");
		}

		/// <summary>
		/// Edycja kampanii
		/// </summary>
		/// <param name="id">Identyfikator kampanii</param>
		/// <param name="returnUrl">Adres powrotny</param>
		[AdServerActionException]
		public ActionResult Edit(int? id, string returnUrl)
		{
			var ids = User.GetUserIDInt();
			var u = _usersRepository.Users.Single(it => it.Id == ids);
			ViewBag.AdPoints = u.AdPoints;

			if (id != null)
			{
				id = UserRole == "Admin" || _repository.Campaigns.FirstOrDefault(it => it.Id == id && it.UserId == UserID) != null ? id : 0;
			}

			// Pobranie obiektu kampanii z repozytorium
			var campaign = _repository.GetById(id ?? 0);

			// Zbudowanie modelu
			var viewModel = new CampaignViewModel
			{
				Campaign =
					campaign ??
					new Campaign
					{
						StartDate = DateTime.Now.Date,
						EndDate = DateTime.Now.AddDays(7).Date,
						IsActive = true,
						UserId = UserID
					},
				Priorities = new SelectList(_priorityRepository.Priorities, "Id", "Name"),
				Users = new SelectList(_usersRepository.Users, "Id", "Name", UserID),

				ReturnURL = returnUrl ?? (Url.Content("~") + "?ctr=Campaign&act=index")
			};

			return View(viewModel);
		}

		/// <summary>
		/// Usunięcie kampanii
		/// </summary>
		/// <param name="ID">Identyfikator kampanii</param>
		[AdServerActionException]
		[HttpPost]
		public ActionResult Delete(int ID)
		{
			// Usunięcia kampanii z bazy danych
			var response = _repository.Delete(ID);

			// Sprawdzenie statusu operacji
			if (!response.Accepted)
			{
				if (TempData != null)
				{
					Error(response.Errors.First().Message);
				}
			}
			return RedirectToAction("Index", "Default", new {ctr = "Campaign"});
		}

		#endregion - Actions -

		#region - Overriden methods -

		/// <summary>
		/// Zwalnianie zasobów
		/// </summary>
		protected override void OnDisposeController()
		{
			if (_repository != null)
			{
				_repository.Dispose();
				_repository = null;
			}

			if (_priorityRepository != null)
			{
				_priorityRepository.Dispose();
				_priorityRepository = null;
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
			var query = (IQueryable<Campaign>)_query;
			dynamic filter1 = _filter;
			CampaignListViewModelFilter filter = filter1;
			query = query.Where(q => UserRole == "Admin" || UserID == q.UserId);

			if (filter != null)
			{
				if (filter.FilterActive.HasValue)
				{
					query = query.Where(q => q.IsActive == filter.FilterActive.Value);
				}
				if (filter.FilterStartDateFrom.HasValue)
				{
					query = query.Where(q => filter.FilterStartDateFrom.Value <= q.StartDate);
				}
				if (filter.FilterStartDateTo.HasValue)
				{
					query = query.Where(q => filter.FilterStartDateTo.Value >= q.StartDate);
				}
				if (filter.FilterEndDateFrom.HasValue)
				{
					query = query.Where(q => filter.FilterEndDateFrom.Value <= q.EndDate);
				}
				if (filter.FilterEndDateTo.HasValue)
				{
					query = query.Where(q => filter.FilterEndDateTo.Value >= q.EndDate);
				}
				if (filter.FilterPriorityId.HasValue && filter.FilterPriorityId.Value != SelectListExt.EmptyOptionValue)
				{
					var priorityId = filter.FilterPriorityId.Value;
					query = query.Where(q => q.PriorityId == priorityId);
				}
				if (!string.IsNullOrEmpty(filter.FilterName))
				{
					query = query.Where(q => q.Name.ToLower().Contains(filter.FilterName.ToLower()));
				}
				if (!string.IsNullOrEmpty(filter.FilterLogin))
				{
					query = query.Where(q => q.User.Name.ToLower().Contains(filter.FilterLogin.ToLower()));
				}
			}

			filter.Priorities = new SelectList(_priorityRepository.Priorities.ToList(), "Id", "Name");
			filter.YesNo = new SelectList(YesNoDictionary.GetList(), "Value", "Key");
			_query = (IQueryable<Q>)query;
			filter1 = filter;
			return (T)filter1;
		}

		#endregion - Overriden methods -
	}
}