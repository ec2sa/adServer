using ADServerDAL.Abstract;
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
	/// Kontroler do osługi obiektów multimedialnych
	/// </summary>
	[Authorize]
	public class MultimediaObjectsController : AdServerBaseController
	{
		#region - Fields -

		/// <summary>
		/// Repozytorium obiektów multimedialnych
		/// </summary>
		private IMultimediaObjectRepository _repository;

		/// <summary>
		/// Repozytorium typów obiektów
		/// </summary>
		private ITypeRepository _typeRespository;
		private string UserRole { get { return User.GetRole(); } }
		public int UserId { get { return User.GetUserIDInt(); } }
		private readonly IUsersRepository _usersRepository;
		#endregion

		#region - Constructors -
		public MultimediaObjectsController(IMultimediaObjectRepository repository, ITypeRepository typeRespository, IUsersRepository usersRepository)
		{
			_repository = repository;
			_typeRespository = typeRespository;
			_usersRepository = usersRepository;
		}
		#endregion

		#region - Actions -
		/// <summary>
		/// Lista obiektów multimedialnych
		/// </summary>
		/// <param name="page">Numer żądanej strony</param>
		/// <param name="sortExpression">Nazwa pola do sortowania</param>
		/// <param name="accending">Czy sortować rosnąco</param>
		/// <param name="hiddenreturnUrl">Adres powrotny</param>
		[AdServerActionExceptionAttribute]
		public ActionResult Index(int? page,
								  string sortExpression,
								  bool? ascending,
								  string hiddenreturnUrl)
		{
			if (Request.HttpMethod == "GET")
				return RedirectToAction("Index", "Default", new { act = "Index", ctr = "MultimediaObjects" });
			User.UpdateAdPoints(_usersRepository);
			var viewModel =
				CreateModel<MultimediaObjectsListViewModel, MultimediaObjectListViewModelFilter, MultimediaObject>(
					FilterSettingsKey.MultimediaObjectsControllerFilterList,PageSettingsKey.MultimediaObjectsPageSettings,
					page, sortExpression, ascending, _repository.MultimediaObjects);

			var id = User.GetUserIDInt();
			var u = _usersRepository.Users.Single(it => it.Id == id);
			ViewBag.AdPoints = u.AdPoints;

			return View("Index", viewModel);
		}

		/// <summary>
		/// Lista obiektów multimedialnych (jako odpowiedź na filtrowanie)
		/// </summary>
		/// <param name="model"></param>
		[AdServerActionExceptionAttribute]
		[HttpPost]
		//[MultipleButton(Name = "action", Argument = "List")]
		public ActionResult List(MultimediaObjectListViewModelFilter model)
		{
			// Zapamietaj filtry
			if (Session != null)
			{
				Session[FilterSettingsKey.MultimediaObjectsControllerFilterList.ToString()] = model;
				PageSettings.RemoveFromSession(PageSettingsKey.MultimediaObjectsPageSettings);
			}
			return Json(true);// RedirectToAction("Index");
		}

		/// <summary>
		/// Usunięcie filtrów
		/// </summary>
		/// <param name="model"></param>
		[AdServerActionExceptionAttribute]
		[HttpPost]
		[MultipleButton(Name = "action", Argument = "ClearFilters")]
		public ActionResult ClearFilters(MultimediaObjectsListViewModel model)
		{
			if (Session != null)
			{
				Session.Remove(FilterSettingsKey.MultimediaObjectsControllerFilterList.ToString());
				PageSettings.RemoveFromSession(PageSettingsKey.MultimediaObjectsPageSettings);
			}
			return RedirectToAction("Index");
		}

		/// <summary>
		/// Edycja obiektu multimedialnego
		/// </summary>
		/// <param name="id"></param>
		/// <param name="returnUrl"></param>
		[AdServerActionExceptionAttribute]
		public ActionResult Edit(int? id, string returnUrl)
		{
			var ids = User.GetUserIDInt();
			var u = _usersRepository.Users.Single(it => it.Id == ids);
			ViewBag.AdPoints = u.AdPoints;

			if (id != null)
			{
				id = UserRole == "Admin" || _repository.MultimediaObjects.FirstOrDefault(it => it.Id == id && it.UserId == UserId) != null ? id : 0;
			}
			// Pobierz obiekt z repozytorium
			var multimediaObject = _repository.GetById(id ?? 0);

			var user = _usersRepository.Users.Single(it => it.Id == UserId);
			var t =
				_typeRespository.Types.Select(it => new { Value = it.Id, Text = it.Name + " szer: " + it.Width + "px wys: " + it.Height + "px" });

			// Zbuduj i zwróć model
			var viewModel = new MultimediaObjectViewModel
			{
				MultimediaObject = multimediaObject ?? new MultimediaObject
				{
					FileName = "brak",
					MimeType = "brak",
					Url = user.Url
				},
				MultimediaTypes = new SelectList(t, "Value", "Text"),
				Guid = Guid.NewGuid().ToString(),
				Users = new SelectList(_usersRepository.Users.ToList(), "Id", "Name", UserId),
				ReturnURL = returnUrl ?? Url.Content("~") + "?ctr=MultimediaObjects&act=index" 
				
			};

			// Zapamietaj maksymalny rozmiar obiektu JSON do komunikacji z serwerem
			ViewBag.MaxJsonLength = UploadController.GetJsonMaxLength();

			return View(viewModel);
		}

		/// <summary>
		/// Pobiera miniaturkę zdjęcia
		/// </summary>
		/// <param name="id">Identyfikator obiektu multiemdialnego</param>
		[AdServerActionExceptionAttribute]
		public FileContentResult GetThumbnail(int id)
		{
			// Pobierz obiekt multimedialny z repozytorium
			var multimediaObject = _repository.GetThumbnail(id);
			if (multimediaObject != null && multimediaObject.Thumbnail != null)
			{
				// Jeżeli obiekt istnieje i ma przypisaną miniaturkę zbuduj odpowiedź
				return File(multimediaObject.Thumbnail, multimediaObject.MimeType);
			}
			return null;
		}

		/// <summary>
		/// Usuwanie obiektu multimedialnego
		/// </summary>
		/// <param name="id"></param>
		[AdServerActionExceptionAttribute]
		[HttpPost]
		public ActionResult Delete(int id)
		{
			// Usuń obiekt z repozytorium
			var response = _repository.Delete(id);

			// Sprawdź status operacji
			if (!response.Accepted)
			{
				foreach (var err in response.Errors)
				{
					Error(err.Message);
				}
			}
			return RedirectToAction("Index", "Default", new {ctr = "MultimediaObjects"});
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

			if (_typeRespository != null)
			{
				_typeRespository.Dispose();
				_typeRespository = null;
			}
		}

		/// <summary>
		/// Customowe filtrowanie
		/// </summary>
		/// <typeparam name="T">Typ filtru</typeparam>
		/// <typeparam name="Q">Typ obiektów w Query</typeparam>
		/// <param name="query">Zbiór encji</param>
		/// <param name="filter">filtr</param>
		/// <returns></returns>
		protected override T FilterSettingsVirtual<T, Q>(ref IQueryable<Q> _query, T _filter)
		{
			var query = (IQueryable<MultimediaObject>)_query;
			dynamic filter1 = _filter;
			MultimediaObjectListViewModelFilter filter = filter1;
			
			query = query.Where(q => UserRole == "Admin" || q.UserId == UserId);

			if (filter != null)
			{
				if (!string.IsNullOrEmpty(filter.FilterName))
				{
					query = query.Where(q => q.Name.ToLower().Contains(filter.FilterName.ToLower()));
				}

				if (!string.IsNullOrEmpty(filter.FilterFileName))
				{
					query = query.Where(q => q.FileName.ToLower().Contains(filter.FilterFileName.ToLower()));
				}

				if (!string.IsNullOrEmpty(filter.FilterMime))
				{
					query = query.Where(q => q.MimeType.ToLower().Contains(filter.FilterMime.ToLower()));
				}

				if (!string.IsNullOrEmpty(filter.FilterType))
				{
					query = query.Where(q => q.Type.Name.ToLower().Contains(filter.FilterType.ToLower()));
				}

				if (!string.IsNullOrEmpty(filter.FilterLogin))
				{
					query = query.Where(q => q.User.Name.ToLower().Contains(filter.FilterLogin.ToLower()));
				}
			}
			_query = (IQueryable<Q>)query;
			filter1 = filter;
			return (T)filter1;
		}
		#endregion
	}
}
