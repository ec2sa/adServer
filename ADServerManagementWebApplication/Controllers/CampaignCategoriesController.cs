using System.Web.Mvc.Filters;
using ADServerDAL.Abstract;
using ADServerDAL.Entities.Presentation;
using ADServerDAL.Filters;
using ADServerDAL.Models;
using ADServerManagementWebApplication.Infrastructure;
using ADServerManagementWebApplication.Infrastructure.ErrorHandling;
using ADServerManagementWebApplication.Models;
using System.Linq;
using ADServerManagementWebApplication.Extensions;
using System.Web.Mvc;

namespace ADServerManagementWebApplication.Controllers
{
	/// <summary>
	/// Kontroler do obsługi kategorii
	/// </summary>
	[Authorize(Roles = "Admin")]
	public class CampaignCategoriesController : AdServerBaseController
	{
		#region - Fields -

		/// <summary>
		/// Repozytorium kategorii
		/// </summary>
		private ICategoryRepository _repository;
		private IUsersRepository _usersRepository;
		#endregion

		#region - Constructors -
		/// <summary>
		/// Konstruktor
		/// </summary>
		public CampaignCategoriesController(ICategoryRepository repository, IUsersRepository usersRepository)
		{
			_repository = repository;
			_usersRepository = usersRepository;
		}
		#endregion

		#region - Actions -
		/// <summary>
		/// Lista kategorii
		/// </summary>
		/// <param name="page">Numer żądanej strony</param>
		/// <param name="sortExpression">Pole sortowane</param>
		/// <param name="accending">Kierunek sortowania</param>        
		[AdServerActionExceptionAttribute]
		public ActionResult Index(int? page,
								  string sortExpression,
								  bool? ascending)
		{
			if (Request.HttpMethod == "GET")
				return RedirectToAction("Index", "Default", new { act = "Index", ctr = "CampaignCategories" });
			// Zbudowanie modelu listy
			var viewModel = CreateModel<CampaignCategoriesListViewModel, CampaignCategoriesListViewModelFilter, Category>(FilterSettingsKey.CampaignCategoriesControllerFilterList, PageSettingsKey.CampaignCategoriesPageSettings, page, sortExpression, ascending, _repository.Categories);


			var id = User.GetUserIDInt();
			var u = _usersRepository.Users.Single(it => it.Id == id);
			ViewBag.AdPoints = u.AdPoints;
			User.UpdateAdPoints(u.AdPoints);

			return View("Index", viewModel);
		}

		/// <summary>
		/// Lista kategorii (filtrowanie)
		/// </summary>
		/// <param name="model">Model listy</param>
		[AdServerActionExceptionAttribute]
		[HttpPost]
		public ActionResult List(CampaignCategoriesListViewModelFilter model)
		{
			// Zapamietanie aktualnych filtrów
			if (Session != null)
			{
				Session[FilterSettingsKey.CampaignCategoriesControllerFilterList.ToString()] = model;
				PageSettings.RemoveFromSession(PageSettingsKey.CampaignCategoriesPageSettings);
			}
			return Json(true);
		}

		/// <summary>
		/// Wyczyszczenie filtrów listy
		/// </summary>
		/// <param name="model">Model listy</param>
		[AdServerActionExceptionAttribute]
		[HttpPost]
		[MultipleButton(Name = "action", Argument = "ClearFilters")]
		public ActionResult ClearFilters(CampaignCategoriesListViewModel model)
		{
			// Usunięcie filtrów
			if (Session != null)
			{
				Session.Remove(FilterSettingsKey.CampaignCategoriesControllerFilterList.ToString());
				PageSettings.RemoveFromSession(PageSettingsKey.CampaignCategoriesPageSettings);
			}
			return RedirectToAction("Index");
		}

		/// <summary>
		/// Edycja kategorii
		/// </summary>
		/// <param name="ID">Identyfikator kategorii</param>
		[AdServerActionExceptionAttribute]
		public ActionResult Edit(int? ID)
		{
			// Pobranie kategorii do edycji z repozytorium
			var category = _repository.GetById(ID ?? 0);

			// Zbudowanie modelu dla edycji kategorii
			var viewModel = new CampaignCategoryViewModel
			{
				Category = category ?? new Category()
			};
			var ids = User.GetUserIDInt();
			var u = _usersRepository.Users.Single(it => it.Id == ids);
			ViewBag.AdPoints = u.AdPoints;
			return View(viewModel);
		}

		/// <summary>
		/// Usuwanie kategorii
		/// </summary>
		/// <param name="ID">Identyfikator kategorii</param>
		[AdServerActionExceptionAttribute]
		[HttpPost]
		public ActionResult Delete(int ID)
		{
			// Usunięcie kategorii
			ApiResponse response = _repository.Delete(ID);
			if (!response.Accepted)
			{
				if (TempData != null)
				{
					TempData["message"] = response.Errors.First().Message;
				}
			}
			return RedirectToAction("Index", "Default", new { ctr = "CampaignCategories" });
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
		/// <param name="_query">Zbiór encji</param>
		/// <param name="_filter">filtr</param>
		/// <returns></returns>
		protected override T FilterSettingsVirtual<T, Q>(ref IQueryable<Q> _query, T _filter)
		{
			var query = (IQueryable<Category>)_query;
			dynamic filter1 = _filter;
			CampaignCategoriesListViewModelFilter filter = filter1;
			var doFiltering = filter != null && filter.Filtering;

			if (doFiltering)
			{
				if (!string.IsNullOrEmpty(filter.FilterName))
				{
					query = query.Where(q => q.Name.ToLower().Contains(filter.FilterName.ToLower()));
				}

				if (!string.IsNullOrEmpty(filter.FilterCode))
				{
					query = query.Where(q => q.Code.ToLower().Contains(filter.FilterCode.ToLower()));
				}
			}
			_query = (IQueryable<Q>)query;
			filter1 = filter;
			return (T)filter1;
		}

		#endregion
	}
}