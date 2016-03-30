using System.Web.Routing;
using ADServerDAL;
using ADServerDAL.Abstract;
using ADServerDAL.Entities;
using ADServerDAL.Entities.Presentation;
using ADServerDAL.Filters;
using ADServerDAL.Helpers;
using ADServerDAL.Models;
using ADServerManagementWebApplication.Extensions;
using ADServerManagementWebApplication.Infrastructure;
using ADServerManagementWebApplication.Infrastructure.ErrorHandling;
using ADServerManagementWebApplication.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace ADServerManagementWebApplication.Controllers
{
	/// <summary>
	/// Klasa odpowiadająca za logowanie
	/// </summary>
	[Authorize]
	public class AccountController : AdServerBaseController
	{
		#region -Fields-

		/// <summary>
		/// repozytorium użytkowników
		/// </summary>
		private readonly IUsersRepository _repository;

		/// <summary>
		/// repozytorium ról
		/// </summary>
		private readonly IRoleRepository _roles;

		/// <summary>
		/// Manager użytkowników
		/// </summary>
		public IEnumerable<ApplicationUser> UserManager { get; private set; }

		/// <summary>
		/// Manager autentyfikacji
		/// </summary>
		private IAuthenticationManager AuthenticationManager
		{
			get { return HttpContext.GetOwinContext().Authentication; }
		}

		/// <summary>
		/// Statystyki
		/// </summary>
		private readonly IStatisticRepository _statisticRepository;

		#endregion -Fields-

		#region -Constructors-

		/// <summary>
		/// Logowanie
		/// </summary>
		/// <param name="repository">Repozytorium użytkowników</param>
		/// <param name="roles">Repozytorium użytkowników</param>
		/// <param name="statisticRepository">Repozytorium statystyk</param>
		public AccountController(IUsersRepository repository, IRoleRepository roles, IStatisticRepository statisticRepository)
		{
			_repository = repository;
			_roles = roles;
			_statisticRepository = statisticRepository;
			UserManager =
				from user in repository.Users
				select new ApplicationUser
				{
					Name = user.Name,
					Id = user.Id,
					LastName = user.LastName,
					FirstName = user.FirstName,
					Password = user.Password,
					Role = user.Role,
					Campaigns = user.Campaigns,
					IsBlocked = user.IsBlocked,
					AdPoints = user.AdPoints,
					Email = user.Email
				};
		}

		#endregion -Constructors-

		#region -Actions-
		#region Login

		/// <summary>
		/// Logowanie z przekierowanie
		/// </summary>
		/// <param name="returnUrl">Powrotny URL</param>
		/// <returns>Rezultat akcji</returns>
		[AllowAnonymous]
		[AdServerActionExceptionAttribute]
		public ActionResult Login(string returnUrl)
		{
			if (Request.IsAuthenticated)
				return RedirectToAction("Index", "Campaign");
			if (User.Identity.IsAuthenticated && returnUrl != null)
			{
				ModelState.AddModelError("", "Nie masz uprawnień do danej przestrzeni");
				return RedirectToAction("Index", "Campaign");
			}
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		/// <summary>
		/// Logowanie z przekierowaniem post
		/// </summary>
		/// <param name="model">Model logowania</param>
		/// <param name="returnUrl">Powrotny URL</param>
		/// <returns>Rezultat akcji</returns>
		[HttpPost]
		[AllowAnonymous]
		[AdServerActionExceptionAttribute]
		[ValidateAntiForgeryToken]
		public ActionResult Login(LoginViewModel model, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				var user =
					UserManager.FirstOrDefault(it => String.Equals(it.Name, model.UserName, StringComparison.CurrentCultureIgnoreCase));

				try
				{
					var chk = user != null && user.Name == "Admin" && user.Password == "Admin";
					if (user != null &&
						(chk ||
						 (new PasswordHasher()).VerifyHashedPassword(user.Password, model.Password) ==
						 PasswordVerificationResult.Success) && !user.IsBlocked)
					{
						SignIn(user, model.RememberMe);
						return RedirectToAction("Index", "Default", new { ctr = "Campaign" });
					}
					ModelState.AddModelError("UserName",
						user != null && user.IsBlocked ? "Użytkownik zablokowany!" : "Błędne dane logowania.");
				}
				catch
				{
					ModelState.AddModelError("Password", "Błąd hasła skontaktuj się z administratorem");
				}
			}

			return View(model);
		}

		/// <summary>
		/// Wylogowanie
		/// </summary>
		/// <returns>Rezultat akcji</returns>
		[AdServerActionExceptionAttribute]
		public ActionResult LogOff()
		{
			AuthenticationManager.SignOut();
			return RedirectToAction("Login", "Account");
		}

		#endregion Login

		#region Manage

		[AdServerActionExceptionAttribute]
		[Authorize(Roles = "Admin")]
		public ActionResult Manage(int? page,
			string sortExpression,
			bool? ascending,
			string hiddenreturnUrl)
		{
			User.UpdateAdPoints(_repository);

			var viewModel =
				CreateModel<UsersListViewModel, UserListViewModelFilter, User>(
					FilterSettingsKey.UserListViewModelFilter, PageSettingsKey.UsersPageSettings, page, sortExpression,
					ascending, _repository.Users);

			var id = User.GetUserIDInt();
			var u = _repository.Users.Single(it => it.Id == id);
			ViewBag.AdPoints = u.AdPoints;
			return PartialView("Manage", viewModel);
		}

		/// <summary>
		/// Zarządzanie kontem post
		/// </summary>
		/// <param name="model">Model uzytkownika</param>
		/// <returns>Rezultat akcji</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[AdServerActionExceptionAttribute]
		[Authorize]
		public ActionResult Pass(ManageUserViewModel model)
		{
			var user = UserManager.First(it => User.GetUserIDInt() == it.Id);
			var pass = new PasswordHasher();

			if (user != null && pass.VerifyHashedPassword(user.Password, model.OldPassword) == PasswordVerificationResult.Success)
			{
				var ret = new User
				{
					Id = user.Id,
					RoleId = user.Role.Id,
					FirstName = model.FirstName,
					LastName = model.LastName,
					Url = model.Url,
					AdditionalInfo = model.AdditionalInfo ?? "",
					CompanyAddress = model.CompanyAddress ?? "",
					CompanyName = model.CompanyName ?? "",
					Email = model.Email, 
					AdPoints = user.AdPoints
				};
				if (model.NewPassword != null)
					ret.Password = pass.HashPassword(model.NewPassword);

				var response = _repository.Save(ret);

				if (!response.Accepted)
				{
					foreach (var err in response.Errors)
					{
						if (string.IsNullOrEmpty(err.Property))
							Error(err.Message);
						else
							ModelState.AddModelError(err.Property, err.Message);
					}
					return View("UserManage", model);
				}

				return RedirectToAction("Index", "Default", new { ctr = "Campaign" });
			}
			var state = ModelState["OldPassword"];
			state.Errors.Add("Hasło podane przez użytkownika nie jest identyczne jak w bazie");

			return View("UserManage", model);
		}

		[AdServerActionExceptionAttribute]
		[Authorize]
		public ActionResult UserManage()
		{
			return View(CreateModel());
		}

		#endregion Manage

		#region Edit

		/// <summary>
		/// Modyfikacja użytkownika lub ich rejestracja
		/// </summary>
		/// <param name="id">Id modyfkiowanego użytkownika</param>
		/// <returns>Rezultat akcji</returns>
		[Authorize(Roles = "Admin")]
		[AdServerActionExceptionAttribute]
		public ActionResult Edit(int id = 0)
		{
			var model = new AdminManageViewModel();

			if (id != 0)
			{
				var mObjectStat = _statisticRepository.StatisticsStatements(null, null, StatisticsStatementType.MultimediaObject, id);
				var campStat = _statisticRepository.StatisticsStatements(null, null, StatisticsStatementType.Campaign, id);
				var user = UserManager.First(it => it.Id == id);
				model = new AdminManageViewModel
				{
					ID = user.Id,
					FirstName = user.FirstName,
					LastName = user.LastName,
					CampaignCount = campStat.Count(),
					ClickCount = mObjectStat.Sum(mm => mm.ClickedCount),
					MmObjectCount = mObjectStat.Count(),
					ViewCount = mObjectStat.Sum(mm => mm.TotalDisplayCount),
					IsBlocked = user.IsBlocked,
					Role = new SelectList(_roles.Roles.ToList(), "Id", "Name", user.Role.Id),
					RoleID = user.Role.Id,
					UserName = user.Name,
					AdPoints = user.AdPoints,
					Email = user.Email
				};
			}
			var r =
				_roles.Roles.Select(
					it => new {Text = it.Name + " -  Prowizja : " + it.Commission + "%", Value = it.Id});

			model.Role = new SelectList(r, "Value", "Text");
			var ids = User.GetUserIDInt();
			var u = _repository.Users.Single(it => it.Id == ids);
			ViewBag.AdPoints = u.AdPoints;
			return View(model);
		}

		/// <summary>
		/// Modyfikacja użytkownika lub ich rejestracja post
		/// </summary>
		/// <param name="model">Model użytkownika</param>
		/// <returns>Rezultat akcji</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		[AdServerActionExceptionAttribute]
		public ActionResult Edit(AdminManageViewModel model)
		{
			var usernameCheck =
				UserManager.FirstOrDefault(
					it => String.Equals(it.UserName, model.UserName, StringComparison.CurrentCultureIgnoreCase));

			if (model.ID == 0 || (usernameCheck != null && model.ID != usernameCheck.Id))
			{
				if (usernameCheck != null)
				{
					ModelState.AddModelError("UserName", "Nazwa użytkownika istnieje już w systemie! : " + model.UserName);
					model.Role = new SelectList(_roles.Roles.ToList(), "Id", "Name", model.RoleID);
					return View(model);
				}
			}

			var roleCheck =
				_roles.Roles.FirstOrDefault(it => it.Name.Contains(model.UserName) || model.UserName.Contains(it.Name));
			if (roleCheck != null && roleCheck.Name != "Admin")
			{
				ModelState.AddModelError("UserName",
					"Nazwa użytkownika nie może zawierać w sobie nazwy uprawnień! " + model.UserName + " : " + roleCheck.Name);
				var user = UserManager.First(it => it.Id == model.ID);

				model = new AdminManageViewModel
				{
					ID = user.Id,
					IsBlocked = user.IsBlocked,
					Role = new SelectList(_roles.Roles.ToList(), "Id", "Name", user.Role.Id),
					RoleID = user.Role.Id,
					UserName = user.Name,
					FirstName = user.FirstName,
					LastName = user.LastName,
					Email = user.Email
				};

				return View(model);
			}

			var pass = model.Password == null && model.PasswordR == null
				? null
				: (new PasswordHasher()).HashPassword(model.Password ?? model.PasswordR);
			var roleId = model.RoleID;
			var ret = new User
			{
				Id = model.ID,
				Name = model.UserName,
				FirstName = model.FirstName,
				IsBlocked = model.IsBlocked,
				LastName = model.LastName,
				Password = pass,
				RoleId = roleId,
				AdPoints = model.AdPoints,
				Email = model.Email
			};

			var response = _repository.Save(ret);
			if (!response.Accepted)
			{
				foreach (var err in response.Errors)
				{
					if (string.IsNullOrEmpty(err.Property))
						Error(err.Message);
					else
						ModelState.AddModelError(err.Property, err.Message);
				}
				model.Role = new SelectList(_roles.Roles.ToList(), "Id", "Name", model.RoleID);
				return View(model);
			}

			return RedirectToAction("Index", "Default", new { ctr = "Account", act = "Manage" });
		}

		#endregion Edit

		#region Role

		[AdServerActionExceptionAttribute]
		[Authorize(Roles = "Admin")]
		public ActionResult Role(int? page,
			string sortExpression,
			bool? ascending,
			string hiddenreturnUrl)
		{
			
			var viewModel =
				CreateModel<RoleListViewModel, RoleListViewModelFilter, Role>(
					FilterSettingsKey.RoleFilterList, PageSettingsKey.RolePageSettings, page, sortExpression,
					ascending, _roles.Roles);

			var id = User.GetUserIDInt();
			var u = _repository.Users.Single(it => it.Id == id);
			ViewBag.AdPoints = u.AdPoints;
			return View("Role", viewModel);
		}

		/// <summary>
		/// Modyfikacja uprawnień lub ich rejestracja
		/// </summary>
		/// <param name="id">Id modyfkiowanego uprawnienia</param>
		/// <returns>Rezultat akcji</returns>
		[Authorize(Roles = "Admin")]
		[AdServerActionExceptionAttribute]
		public ActionResult EditRole(int id = 0)
		{
			var tempRole = _roles.Roles.FirstOrDefault(it => it.Id == id);
			var model = tempRole != null
				? new RoleViewModel
				{
					Roles = typeof(RoleType).GetEnumAsSelectList(tempRole.RoleType),
					Type = tempRole.RoleType,
					ID = tempRole.Id,
					Name = tempRole.Name,
					Commission = tempRole.Commission
				}
				: new RoleViewModel { Roles = typeof(RoleType).GetEnumAsSelectList() };
			var ids = User.GetUserIDInt();
			var u = _repository.Users.Single(it => it.Id == ids);
			ViewBag.AdPoints = u.AdPoints;
			return View("EditRole", model);
		}

		/// <summary>
		/// Modyfikacja uprawnień lub ich rejestracja post
		/// </summary>
		/// <param name="model">Model uprawnień</param>
		/// <returns>Rezultat akcji</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		[AdServerActionExceptionAttribute]
		public ActionResult EditRole(RoleViewModel model)
		{
			var exRole = _roles.Roles.FirstOrDefault(it => it.Name == model.Name && it.Id != model.ID);

			if (exRole != null)
			{
				model.Roles = typeof(RoleType).GetEnumAsSelectList();
				Error("Dana rola znajduje się już systemie");
				return View(model);
			}
			if (model.Commission == 0)
			{
				model.Roles = typeof(RoleType).GetEnumAsSelectList();
				ModelState.AddModelError("Commission", "Nieprawidłowa wartość prowizji");
				return View(model);
			}

			var role = new Role
			{
				Id = model.ID,
				Name = model.Name,
				RoleType = model.Type,
				Commission = model.Commission
			};
			TempData["Error"] = new List<string>();
			var ret = _roles.Save(role);

			if (ret.Accepted)
				return RedirectToAction("Index", "Default", new { ctr = "Account", act = "Role" });

			model.Roles = typeof(RoleType).GetEnumAsSelectList();
			Error("Zapis roli nie powiódł się");
			return View(model);
		}

		/// <summary>
		/// Usuwanie uprawnień
		/// </summary>
		/// <param name="id">Id uprawnień</param>
		/// <returns>Rezultat akcji</returns>
		[HttpPost]
		[Authorize(Roles = "Admin")]
		[AdServerActionExceptionAttribute]
		public ActionResult DeleteRole(int id)
		{
			_roles.Delete(id);

			return RedirectToAction("Index", "Default", new { ctr = "Account", act = "Role" });
		}

		[AdServerActionExceptionAttribute]
		[HttpPost]
		public ActionResult ListRole(RoleListViewModelFilter model)
		{
			//Zapamiętanie aktualnych filtrów
			if (Session != null)
			{
				Session[FilterSettingsKey.RoleFilterList.ToString()] = model;
				PageSettings.RemoveFromSession(PageSettingsKey.RolePageSettings);
			}
			return Json(true);
		}
		#endregion Role

		#region Register

		[AllowAnonymous]
		public ActionResult Register()
		{
			if (Request.IsAuthenticated)
				return RedirectToAction("Index", "Default", new { ctr = "Campaign" });

			var model = new RegisterViewModel();
			return View(model);
		}

		[AllowAnonymous]
		[HttpPost]
		public ActionResult Register(RegisterViewModel model)
		{
			if (!UsernameChack(model.Name, null))
				return View(model);
			try
			{
				var pass = new PasswordHasher().HashPassword(model.Password);
				var role = _roles.Roles.First((it => it.Name == "User"));

				var rep = new User
				{
					AdPoints = 0,
					Name = model.Name,
					IsBlocked = false,
					FirstName = model.FirstName,
					LastName = model.LastName,
					Password = pass,
					RoleId = role.Id,
					Url = model.Url,
					Email = model.Email,
					AdditionalInfo = model.AdditionalInfo,
					CompanyAddress = model.CompanyAddress,
					CompanyName = model.CompanyName
				};
				var ret = _repository.Save(rep);
				if (!ret.Accepted)
				{
					foreach (var it in ret.Errors)
					{
						Error(it.Message);
					}
					return View(model);
				}
			}
			catch (Exception e)
			{
				Error("Rejestracja nie powiodłą się błąd : " + e.Message);
				return View(model);
			}

			var login = new LoginViewModel
			{
				Password = model.Password,
				RememberMe = false,
				UserName = model.Name
			};

			UserManager =
				(from user in _repository.Users
				 select new ApplicationUser
				 {
					 Name = user.Name,
					 Id = user.Id,
					 LastName = user.LastName,
					 FirstName = user.FirstName,
					 Password = user.Password,
					 Role = user.Role,
					 Campaigns = user.Campaigns,
					 IsBlocked = user.IsBlocked,
					 AdPoints = user.AdPoints,
					 Email = user.Email
				 }
					);

			return Login(login, "");
		}

		#endregion Register

		/// <summary>
		/// Przekierowanie
		/// </summary>
		/// <param name="returnUrl">adres przekierowania</param>
		/// <returns>Rezultat akcji</returns>
		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 2)
			{
				return Redirect(returnUrl);
			}
			return RedirectToAction("Index", "Default", new { ctr = "Campaign" });
		}

		[AdServerActionExceptionAttribute]
		[HttpPost]
		public ActionResult List(UserListViewModelFilter model)
		{
			//Zapamiętanie aktualnych filtrów
			if (Session != null)
			{
				Session[FilterSettingsKey.UserListViewModelFilter.ToString()] = model;
				PageSettings.RemoveFromSession(PageSettingsKey.UsersPageSettings);
			}
			return Json(true);
		}

		#endregion -Actions-

		#region -Private functions-

		/// <summary>
		/// Logowanie użytkownika
		/// </summary>
		/// <param name="user">model użytkownika</param>
		/// <param name="rememberMe"></param>
		public void SignIn(ApplicationUser user, bool rememberMe = false)
		{
			var claims = new List<Claim>
			{
				new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
					user.Id.ToString(CultureInfo.GetCultureInfo("en-US"))),
				new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", user.LongName),
				new Claim("AdPoints", user.AdPoints.ToString(CultureInfo.GetCultureInfo("en-US"))),
				new Claim(ClaimTypes.Role, user.Role.RoleType)
			};

			AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
			var identity = new ClaimsIdentity(user, claims);
			AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = rememberMe }, identity);
		}

		private ManageUserViewModel CreateModel()
		{
			var id = User.GetUserIDInt();
			var user = _repository.Users.First(it => it.Id == id);
			if (user != null)
			{
				var model = new ManageUserViewModel
				{
					FirstName = user.FirstName,
					LastName = user.LastName,
					AdditionalInfo = user.AdditionalInfo,
					CompanyAddress = user.CompanyAddress,
					CompanyName = user.CompanyName,
					Url = user.Url,
					Email = user.Email
				};
				return model;
			}
			return null;
		}

		private bool UsernameChack(string userName, int? id)
		{
			var ret =
				UserManager.FirstOrDefault(
					it => String.Equals(it.UserName, userName, StringComparison.CurrentCultureIgnoreCase) && it.Id != (id ?? 0));

			if (ret != null)
			{
				ModelState.AddModelError("Name", "Nazwa użytkownika istnieje już w systemie! : " + ret.UserName);
				return false;
			}

			var roleCheck = _roles.Roles.FirstOrDefault(it => it.Name.Contains(userName) || userName.Contains(it.Name));

			if (roleCheck != null)
			{
				Error("Nazwa użytkownika nie może zawierać w sobie nazwy uprawnień! " + userName + " : " + roleCheck.Name);
				ModelState.AddModelError("Name",
					"Nazwa użytkownika nie może zawierać w sobie nazwy uprawnień! " + userName + " : " + roleCheck.Name);
				return false;
			}

			return true;
		}

		#endregion -Private functions-

		/// <summary>
		/// Customowe filtrowanie
		/// </summary>
		/// <typeparam name="T">Typ filtru</typeparam>
		/// <param name="_query">Zbiór encji</param>
		/// <param name="_filter">filtr</param>
		/// <returns></returns>
		protected override T FilterSettingsVirtual<T, Q>(ref IQueryable<Q> _query, T _filter)
		{
			if (_query is IQueryable<User>)
			{

				var query = (IQueryable<User>) _query;
				dynamic filter1 = _filter;
				UserListViewModelFilter filter = filter1;

				var doFiltering = filter != null && filter.Filtering;
				if (doFiltering)
				{
					if (!string.IsNullOrEmpty(filter.FilterLogin))
					{
						query = query.Where(q => q.Name.ToLower().Contains(filter.FilterLogin.ToLower()));
					}
					if (!string.IsNullOrEmpty(filter.FilterFirstName))
					{
						query = query.Where(q => q.FirstName.ToLower().Contains(filter.FilterFirstName.ToLower()));
					}
					if (!string.IsNullOrEmpty(filter.FilterLastName))
					{
						query = query.Where(q => q.LastName.ToLower().Contains(filter.FilterLastName.ToLower()));
					}
					if (filter.FilterRolaId.HasValue)
					{
						query = query.Where(q => q.Role.Id == filter.FilterRolaId);
					}
					if (filter.FilterBlocked.HasValue)
					{
						query = query.Where(q => q.IsBlocked == filter.FilterBlocked);
					}
				}
				filter.FilterRole = new SelectList(_roles.Roles.ToList(), "Id", "Name");
				filter.YesNo = new SelectList(YesNoDictionary.GetList(), "Value", "Key");
				_query = (IQueryable<Q>) query;
				filter1 = filter;
				return (T) filter1;
			}
			var queryR = (IQueryable<Role>)_query;
			dynamic filter1R = _filter;
			RoleListViewModelFilter filterR = filter1R;

			var doFilteringR = filterR != null && filterR.Filtering;
			if (doFilteringR)
			{
				if (!string.IsNullOrEmpty(filterR.FilterName))
				{
					queryR = queryR.Where(q => q.Name.ToLower().Contains(filterR.FilterName.ToLower()));
				}
				if (filterR.Com.HasValue)
				{
					queryR = queryR.Where(q => q.Commission == filterR.Com.Value);
				}
				
			}
			_query = (IQueryable<Q>)queryR;
			filter1R = filterR;
			return (T)filter1R;
		}
	}
}