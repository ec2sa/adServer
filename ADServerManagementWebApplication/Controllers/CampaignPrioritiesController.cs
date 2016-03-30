using System.CodeDom;
using ADServerDAL;
using ADServerDAL.Abstract;
using ADServerDAL.Concrete;
using ADServerDAL.Entities;
using ADServerDAL.Entities.Presentation;
using ADServerDAL.Filters;
using ADServerDAL.Helpers;
using ADServerDAL.Models;
using ADServerDAL.Models.Base;
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
	/// Kontroler do obsługi priorytetów
	/// </summary>
	[Authorize(Roles = "Admin")]
	public class CampaignPrioritiesController : AdServerBaseController
	{
		#region - Fields -

		/// <summary>
		/// Repozytorium priorytetów
		/// </summary>
		private IPriorityRepository _repository;

		private IUsersRepository _usersRepository;
		#endregion

		#region - Constructors -
		/// <summary>
		/// 
		/// </summary>
		/// <param name="repository"></param>

		public CampaignPrioritiesController(IPriorityRepository repository, IUsersRepository usersRepository)
		{
			_repository = repository;
			_usersRepository = usersRepository;
		}
		#endregion

		#region - Actions -
		/// <summary>
		/// Lista priorytetów
		/// </summary>
		/// <param name="page">Numer żądanej strony</param>
		/// <param name="sortExpression">Pole do sortowania</param>
		/// <param name="ascending">Czy sortować rosnąco</param>
		[AdServerActionExceptionAttribute]
		public ActionResult Index(int? page,
								  string sortExpression,
								  bool? ascending)
		{
			if (Request.HttpMethod == "GET")
				return RedirectToAction("Index", "Default", new { act = "Index", ctr = "CampaignPriorities" });
			
			var viewModel = CreateModel<CampaignPrioritiesListViewModel, CampaignPrioritiesListViewModelFilter, Priority>(FilterSettingsKey.CampaignPrioritiesControllerFilterList, PageSettingsKey.CampaignPrioritiesPageSettings, page, sortExpression, ascending, _repository.Priorities);

			var id = User.GetUserIDInt();
			var u = _usersRepository.Users.Single(it => it.Id == id);
			ViewBag.AdPoints = u.AdPoints;

			return View("Index", viewModel);
		}

		/// <summary>
		/// Lista priorytetów (jako odpowiedź na filtrowanie)
		/// </summary>
		/// <param name="model"></param>
		[AdServerActionExceptionAttribute]
		[HttpPost]
		public ActionResult List(CampaignPrioritiesListViewModelFilter model)
		{
			// Zapamiętanie aktualnych filtrów
			if (Session != null)
			{
				Session[FilterSettingsKey.CampaignPrioritiesControllerFilterList.ToString()] = model;
				PageSettings.RemoveFromSession(PageSettingsKey.CampaignPrioritiesPageSettings);
			}
			return Json(true);
		}

		/// <summary>
		/// Wyczyszczenie filtrów
		/// </summary>
		/// <param name="model"></param>
		[AdServerActionExceptionAttribute]
		[HttpPost]
		[MultipleButton(Name = "action", Argument = "ClearFilters")]
		public ActionResult ClearFilters(CampaignPrioritiesListViewModel model)
		{
			// Usunięcie filtrów z sesji
			if (Session != null)
			{
				Session.Remove(FilterSettingsKey.CampaignPrioritiesControllerFilterList.ToString());
				PageSettings.RemoveFromSession(PageSettingsKey.CampaignPrioritiesPageSettings);
			}
			return RedirectToAction("Index");
		}

		/// <summary>
		/// Edycja priorytetu
		/// </summary>
		/// <param name="id"></param>
		[AdServerActionExceptionAttribute]
		public ActionResult Edit(int? id)
		{
			var ids = User.GetUserIDInt();
			var u = _usersRepository.Users.Single(it => it.Id == ids);
			ViewBag.AdPoints = u.AdPoints;
			//Zbudowanie i zwrócenie modelu
			var viewModel = new CampaignPriorityViewModel
			{
				Priority = _repository.GetById(id ?? 0) ?? new Priority()
			};

			return View(viewModel);
		}

		/// <summary>
		/// Edycja priorytetu
		/// </summary>
		/// <param name="viewModel"></param>
		[AdServerActionExceptionAttribute]
		[HttpPost]
		public ActionResult Edit(CampaignPriorityViewModel viewModel)
		{
			// Sprawdzenie stanu modelu          
			if (ModelState.IsValid)
			{
				try
				{
					// Zapis obiektu do repozytorium
					var response = _repository.Save(viewModel.Priority);
					if (response != null && !response.Accepted)
					{
						foreach (var r in response.Errors)
						{
							var key = string.IsNullOrEmpty(r.Property) ? "" : ("Priority." + r.Property);
							ModelState.AddModelError(key, r.Message);
						}
						return View(viewModel);
					}
				}
				catch (Exception ex)
				{
					// Obsługa błędów
					DbValidationErrorHandler.ModelHandleException(ex, ModelState, "Priority");
					return View(viewModel);
				}

				return RedirectToAction("Index", "Default", new {ctr = "CampaignPriorities"});
			}
			// Niepoprawne dane - zwróc formularz użytkownikowi
			Error("Niepoprawne dane");
			return View(viewModel);
		}

		/// <summary>
		/// Usuwanie priorytetu
		/// </summary>
		/// <param name="ID"></param>
		[AdServerActionExceptionAttribute]
		[HttpPost]
		public ActionResult Delete(int id)
		{
			// Usunięcie priorytetu z repozytorium
			var response = _repository.Delete(id);

			// Sprawdzenie stanu operacji
			if (!response.Accepted && TempData != null)
				foreach (var err in response.Errors)
				{
					Error(err.Message);
				}
			return RedirectToAction("Index", "Default", new { ctr = "CampaignPriorities" });
		}
		#endregion

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
		}

		/// <summary>
		/// Customowe filtrowanie
		/// </summary>
		/// <typeparam name="T">Typ filtru</typeparam>
		/// <param name="repo">Zbiór encji</param>
		/// <param name="filter">filtr</param>
		/// <returns></returns>
		protected override T FilterSettingsVirtual<T, Q>(ref IQueryable<Q> _query, T _filter)
		{
			var query = (IQueryable<Priority>)_query;
			dynamic filter1 = _filter;
			CampaignPrioritiesListViewModelFilter filter = filter1;
			var doFiltering = filter != null && filter.Filtering;
			if (doFiltering)
			{
				if (!string.IsNullOrEmpty(filter.FilterName))
				{
					query = query.Where(q => q.Name.ToLower().Contains(filter.FilterName.ToLower()));
				}

				if (filter.FilterCode.HasValue)
				{
					query = query.Where(q => q.Code == filter.FilterCode.Value);
				}
			}

			_query = (IQueryable<Q>)query;
			filter1 = filter;

			return (T)filter1;
		}
		#endregion
	}
}