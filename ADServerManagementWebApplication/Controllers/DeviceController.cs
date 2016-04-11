using System.Linq;
using System.Web.Mvc;
using ADServerDAL.Abstract;
using ADServerDAL.Filters;
using ADServerDAL.Models;
using ADServerManagementWebApplication.Extensions;
using ADServerManagementWebApplication.Infrastructure;
using ADServerManagementWebApplication.Infrastructure.ErrorHandling;
using ADServerManagementWebApplication.Models;
using Microsoft.Ajax.Utilities;
using ADServerDAL.Concrete;

namespace ADServerManagementWebApplication.Controllers
{
	public class DeviceController : AdServerBaseController
	{
		private readonly IDeviceRepository _repository;
		private readonly ICampaignRepository _campaign;
		private readonly ITypeRepository _typeRepository;
		private readonly IUsersRepository _usersRepository;
		public DeviceController(IDeviceRepository repository, ICampaignRepository campaign, ITypeRepository type, IUsersRepository usersRepository)
		{
			_repository = repository;
			_campaign = campaign;
			_typeRepository = type;
			_usersRepository = usersRepository;
		}

		/// <summary>
		/// Lista nośników
		/// </summary>
		/// <param name="page">Numer żądanej strony</param>
		/// <param name="sortExpression">Pole do sortowania</param>
		/// <param name="accending">Czy sortować rosnąco</param>
		public ActionResult Index(int? page,
								  string sortExpression,
								  bool? ascending,
								  string hiddenreturnUrl)
		{
			if (Request.HttpMethod == "GET")
				return RedirectToAction("Index", "Default", new { act = "Index", ctr = "Device" });
			var viewModel = CreateModel<DeviceListViewModel, DeviceListViewModelFilter, Device>(FilterSettingsKey.DeviceFilterList, PageSettingsKey.DevicePageSettings, page, sortExpression, ascending, _repository.Devices);
			var id = User.GetUserIDInt();
			var u = _usersRepository.Users.Single(it => it.Id == id);
			ViewBag.AdPoints = u.AdPoints;
			
			User.UpdateAdPoints(u.AdPoints);
			return View("Index", viewModel);
		}

		/// <summary>
		/// Edycja nośnika
		/// </summary>
		/// <param name="id">Id nośnika</param>
		/// <returns>Widok edycji</returns>
		public ActionResult Edit(int? id, string returnUrl)
		{
			var ids = User.GetUserIDInt();
			var us = _usersRepository.Users.Single(it => it.Id == ids);
			ViewBag.AdPoints = us.AdPoints;

            var u = ids;// User.GetUserIDInt();
			var r = User.GetRole();
			if (id != null)
			{
				id = r == "Admin" || _repository.Devices.FirstOrDefault(it => it.Id == id && it.UserId == u) != null ? id : 0;
			}

            var device = (id == null || id == 0) ? new Device() : _repository.Devices.First(it => it.Id == id);
            if(device != null)
            {
                if (device.UserId != ids && us.Role.Name != "Admin" && device.UserId != null) { return RedirectToAction("Index"); };
            }
			ViewBag.SelectList = new SelectList(_campaign.Campaigns, "Id", "Name");

			var model = new DeviceViewModel
			{
				Id = device.Id,
				Name = device.Name,
				UserId = device.UserId,
				Campaign = device.Campaigns.Select(it => it.Id),
				Description = device.Description,
				TypeId = device.TypeId,
			};
			ViewBag.Return = string.IsNullOrEmpty(returnUrl) ? Url.Content("~") + "?ctr=Device&act=index" : returnUrl;
			
			var t =
				_typeRepository.Types.Select(it => new { Value = it.Id, Text = it.Name + " szer: " + it.Width + "px wys: " + it.Height + "px" });

			ViewBag.Types = new SelectList(t, "Value", "Text");
            
            return View(model);
		}
		/// <summary>
		/// Edycja nośnika - post
		/// </summary>
		/// <param name="device">model edycji nośnika</param>
		/// <returns>Widok powrotu</returns>
		[System.Web.Mvc.HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(DeviceViewModel device)
		{
			var ids = User.GetUserIDInt();
			var u = _usersRepository.Users.Single(it => it.Id == ids);
			ViewBag.AdPoints = u.AdPoints;

			if (ModelState.IsValid)
			{
				device.UserId = device.UserId == 0 ? User.GetUserIDInt() : device.UserId;
				_repository.Save(device);
				return RedirectToAction("Index", "Default", new { ctr = "Device" });
			}

            return View(device);
		}

		[AdServerActionException]
		[HttpPost]
		public ActionResult List(DeviceListViewModelFilter model)
		{
			// Zapamiętanie aktualnych filtrów
			if (Session != null)
			{
				Session[FilterSettingsKey.DeviceFilterList.ToString()] = model;
				PageSettings.RemoveFromSession(PageSettingsKey.DevicePageSettings);
			}
			return Json(true);
		}

		// GET: /Device/Delete/5
		public ActionResult Delete(int id)
		{
			if (id != 0)
			{
				_repository.Delete(id);
			}
			return RedirectToAction("Index", "Default", new { ctr = "Device" });
		}

		public ActionResult Preview(int id)
		{
			var ids = User.GetUserIDInt();
			var us = _usersRepository.Users.Single(it => it.Id == ids);
            
			ViewBag.AdPoints = us.AdPoints;

			var dev = _repository.Devices.SingleOrDefault(it => it.Id == id);
            
            if (dev.UserId == ids || us.Role.Name == "Admin")
            {
                return View(dev);
            }
            else
                return RedirectToAction("Index");
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
			var query = (IQueryable<Device>)_query;
			var UserRole = User.GetRole();
			var UserId = User.GetUserIDInt();

			query = query.Where(q => UserRole == "Admin" || UserId == q.UserId);
			dynamic filter1 = _filter;
			DeviceListViewModelFilter filter = filter1;
			var doFiltering = filter != null && filter.Filtering;
			if (doFiltering)
			{
				if (!string.IsNullOrEmpty(filter.FilterName))
				{
					query = query.Where(q => q.Name.ToLower().Contains(filter.FilterName.ToLower()));
				}
			}
			_query = (IQueryable<Q>)query;
			filter1 = filter;
			return (T)filter1;
		}
	}
}
