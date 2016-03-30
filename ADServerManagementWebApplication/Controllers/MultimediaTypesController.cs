using ADServerDAL.Abstract;
using ADServerDAL.Concrete;
using ADServerDAL.Entities.Presentation;
using ADServerDAL.Filters;
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
	/// Kontroler do obsługi typów multimedialnych
	/// </summary>
	[Authorize(Roles = "Admin")]
	public class MultimediaTypesController : AdServerBaseController
	{
		#region - Fields -

		/// <summary>
		/// Repozytorium typów multimedialnych
		/// </summary>
		private ITypeRepository _repository;

		private IUsersRepository _usersRepository;

		#endregion - Fields -

		#region - Constructors -

		public MultimediaTypesController(ITypeRepository repository, IUsersRepository usersRepository)
		{
			_repository = repository;
			_usersRepository = usersRepository;
		}

		#endregion - Constructors -

		#region - Actions -

		/// <summary>
		/// Lista typów multimedialnych
		/// </summary>
		/// <param name="page">Numer żądanej strony</param>
		/// <param name="sortExpression">Pole do sortowania</param>
		/// <param name="accending">Czy sortować rosnąco</param>
		[AdServerActionExceptionAttribute]
		public ActionResult Index(int? page,
								  string sortExpression,
								  bool? ascending)
		{
			if (Request.HttpMethod == "GET")
				return RedirectToAction("Index", "Default", new { act = "Index", ctr = "MultimediaTypes" });
			var viewModel = CreateModel<MultimediaTypesListViewModel, MultimediaTypesListViewModelFilter, ADServerDAL.Models.Type>(
					FilterSettingsKey.MultimediaTypesListViewModelFilter, PageSettingsKey.MultimediaTypesPageSettings, page,
					sortExpression, ascending, _repository.Types);

			var id = User.GetUserIDInt();
			var u = _usersRepository.Users.Single(it => it.Id == id);
			ViewBag.AdPoints = u.AdPoints;

			return View("Index", viewModel);
		}

		/// <summary>
		/// Lista typów (dla filtrowania)
		/// </summary>
		/// <param name="model"></param>
		[AdServerActionExceptionAttribute]
		[HttpPost]
		public ActionResult List(MultimediaTypesListViewModelFilter model)
		{
			// Zapamiętaj filtry
			if (Session != null)
			{
				Session[FilterSettingsKey.MultimediaTypesListViewModelFilter.ToString()] = model;
				PageSettings.RemoveFromSession(PageSettingsKey.MultimediaTypesPageSettings);
			}
			return Json(true);
		}

		/// <summary>
		/// Wyczyszczenie filtrów
		/// </summary>
		/// <param name="model"></param>
		[AdServerActionExceptionAttribute]
		[HttpPost]
		public ActionResult ClearFilters(MultimediaTypesListViewModel model)
		{
			// Usuń filtry
			if (Session != null)
			{
				Session.Remove(FilterSettingsKey.MultimediaTypesListViewModelFilter.ToString());
				PageSettings.RemoveFromSession(PageSettingsKey.MultimediaTypesPageSettings);
			}
			return RedirectToAction("Index");
		}

		/// <summary>
		/// Edycja typu multimedialnego
		/// </summary>
		/// <param name="id"></param>
		[AdServerActionExceptionAttribute]
		public ActionResult Edit(int? id)
		{
			var ids = User.GetUserIDInt();
			var u = _usersRepository.Users.Single(it => it.Id == ids);
			ViewBag.AdPoints = u.AdPoints;

			// Pobierz typ z repozytorium
			var type = _repository.GetById(id ?? 0);

			// Zbuduj i zwróć model
			var viewModel = new MultimediaTypeViewMode
			{
				MultimediaType = type ?? new ADServerDAL.Models.Type()
			};

			return View(viewModel);
		}

		/// <summary>
		/// Zapis typu multimedialnego
		/// </summary>
		/// <param name="viewModel"></param>
		[AdServerActionExceptionAttribute]
		[HttpPost]
		public ActionResult Edit(MultimediaTypeViewMode viewModel)
		{
			// Sprawdź status modelu
			if (ModelState.IsValid)
			{
				try
				{
					// Zapisz obiekt do repozytorium
					var response = _repository.Save(viewModel.MultimediaType);
					if (response != null && !response.Accepted)
					{
						foreach (var r in response.Errors)
						{
							var key = string.IsNullOrEmpty(r.Property) ? "" : ("MultimediaType." + r.Property);
							ModelState.AddModelError(key, r.Message);
						}
						return View(viewModel);
					}
				}
				catch (Exception ex)
				{
					// Obsłuż błędy
					DbValidationErrorHandler.ModelHandleException(ex, ModelState, "MultimediaType");
					return View(viewModel);
				}

				return RedirectToAction("Index", "Default", new { ctr = "MultimediaTypes" });
			}
			else
			{
				return View(viewModel);
			}
		}

		/// <summary>
		/// Usuwanie typu multimedialnego
		/// </summary>
		/// <param name="id"></param>
		[AdServerActionExceptionAttribute]
		[HttpPost]
		public ActionResult Delete(int id)
		{
			// Usuń typ z repozytorium
			var response = _repository.Delete(id);

			// Sprawdź status operacji
			if (!response.Accepted)
			{
				foreach (var err in response.Errors)
				{
					Error(err.Message);
				}
			}
			return RedirectToAction("Index", "Default", new { ctr = "MultimediaTypes" });
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
		}

		/// <summary>
		/// Customowe filtrowanie
		/// </summary>
		/// <typeparam name="T">Typ filtru</typeparam>
		/// <param name="query">Zbiór encji</param>
		/// <param name="filter">filtr</param>
		/// <returns></returns>
		protected override T FilterSettingsVirtual<T, Q>(ref IQueryable<Q> _query, T _filter)
		{
			
			var query = (IQueryable<ADServerDAL.Models.Type>)_query;
			dynamic filter1 = _filter;
			MultimediaTypesListViewModelFilter filter = filter1;
			var doFiltering = filter != null && filter.Filtering;

			if (doFiltering)
			{
				if (!string.IsNullOrEmpty(filter.FilterName))
				{
					query = query.Where(q => q.Name.ToLower().Contains(filter.FilterName.ToLower()));
				}

				if (filter.FilterWidth.HasValue)
				{
					query = query.Where(q => q.Width == filter.FilterWidth.Value);
				}

				if (filter.FilterHeight.HasValue)
				{
					query = query.Where(q => q.Height == filter.FilterHeight.Value);
				}
			}
			_query = (IQueryable<Q>)query;
			filter1 = filter;
			return (T)filter1;
		}

		#endregion - Overriden methods -
	}
}