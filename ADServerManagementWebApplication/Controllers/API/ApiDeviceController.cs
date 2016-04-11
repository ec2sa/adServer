using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using ADServerDAL;
using ADServerDAL.Abstract;
using ADServerDAL.Entities.Presentation;
using ADServerDAL.Models;
using ADServerDAL.Models.Base;
using ADServerManagementWebApplication.Infrastructure;
using ADServerManagementWebApplication.Models;
using ADServerManagementWebApplication.Extensions;
using Microsoft.Ajax.Utilities;
using Ninject.Infrastructure.Language;

namespace ADServerManagementWebApplication.Controllers
{
	public class ApiDeviceController : AdServerBaseApiController
	{
		private readonly IDeviceRepository _repository;
		private readonly ICampaignRepository _campaignRepository;
		private readonly ICategoryRepository _categoryRepository;
        private readonly IUsersRepository _userRepository;

		public ApiDeviceController(IDeviceRepository repository, ICampaignRepository campaignRepository, ICategoryRepository categoryRepository, IUsersRepository userRepository)
		{
			_repository = repository;
			_campaignRepository = campaignRepository;
			_categoryRepository = categoryRepository;
            _userRepository = userRepository;
		}

		public ApiResponse SaveDevice(Device device)
		{
            if (device.Id != 0)
            {
                device.UserId = _repository.GetDeviceById(device.Id).UserId;//User.GetUserIDInt();
            }else
                device.UserId = User.GetUserIDInt();

			var ret = _repository.Save(device);
			return ret;
		}

		public DevicesResponse Devices([FromBody]BaseRequest req)
		{
			if (req == null || req.Id == 0 || req.Id == 0)
			{
				return new DevicesResponse { Accepted = false };
			}
			var id = User.GetUserIDInt();
			id = User.IsInRole("Admin") ? 0 : id;
			var cmp = _campaignRepository.Campaigns.SingleOrDefault(it => it.Id == req.Id && (it.UserId == id || id == 0));

			if (cmp == null)
			{
				return new DevicesResponse { Accepted = false};
			}

			var devs = cmp.Devices.Select(it => new Devs { Description = it.Description, Id = it.Id, Name = it.Name });
			return new DevicesResponse { Accepted = true, ConnectedDevs = devs };
		}

		/// <summary>
		/// Pobiera listę kategorii z uwzględnieniem kategorii powiązanych z daną kampanią
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public CategoriesResponse Categories([FromBody]BaseRequest request)
		{
			var response = new CategoriesResponse();
			try
			{
				var allCategories = _categoryRepository.Categories.Select(it => new CatToDev { Code = it.Code, Id = it.Id, Name = it.Name });

				var connectedCategories = new List<int>();

				if (request.Id > 0)
				{
					var categoriesToDev = _repository.CatoegoriesToDev(request.Id);

					if (categoriesToDev != null && categoriesToDev.Any())
					{
						connectedCategories.AddRange(categoriesToDev);
					}
				}

				response.AvailableCategories = new List<CatToDev>();
				response.ConnectedCategories = new List<CatToDev>();

				foreach (CatToDev item in allCategories)
				{
					if (connectedCategories.Contains(item.Id))
					{
						response.ConnectedCategories.Add(item);
					}
					else
					{
						response.AvailableCategories.Add(item);
					}
				}

			}
			catch (Exception ex)
			{
				response.Errors.Add(new ApiValidationErrorItem
				{
					Message = ex.Message
				});
			}

			response.Accepted = response.Errors.Count == 0;

			return response;
		}


		/// <summary>
		/// Pobranie listy kampanii dla zadanej kategorii
		/// </summary>
		/// <param name="request"></param>        
		public CampaignsResponse Campaigns([FromBody] BaseRequest request)
		{
			var response = new CampaignsResponse();

			try
			{
				var allCampaigns = _campaignRepository.Campaigns.ToList();
                var UserId = User.GetUserIDInt();
                if(User.GetRole() != "Admin")
                {
                    allCampaigns = _campaignRepository.Campaigns.Where(m => m.UserId == UserId).ToList();
                }


				var device = _repository.Devices.FirstOrDefault(it => it.Id == request.Id);


				response.ConnectedCampaigns = device == null ?
					new List<CampaignItem>() :
					from dc in device.Campaigns
					select new CampaignItem { Id = dc.Id, Name = dc.Name, IsActive = dc.IsActive, StartDate = dc.StartDate, EndDate = dc.EndDate, ViewValue = dc.ViewValue, ClickValue = dc.ClickValue };

				response.AvailableCampaigns = device == null
					? from c in allCampaigns
					  select
						  new CampaignItem
						  {
							  Id = c.Id,
							  Name = c.Name,
							  IsActive = c.IsActive,
							  StartDate = c.StartDate,
							  EndDate = c.EndDate,
							  ViewValue = c.ViewValue,
							  ClickValue = c.ClickValue,
							  IsBlocked = (request.Id != 0 && c.DeletedDevices.Any(it => it.Id == request.Id))
						  } :
					allCampaigns
					.Where(it => response.ConnectedCampaigns.All(c => c.Id != it.Id))
					.Select(c => new CampaignItem
							{
								Id = c.Id,
								Name = c.Name,
								IsActive = c.IsActive,
								StartDate = c.StartDate,
								EndDate = c.EndDate,
								ViewValue = c.ViewValue,
								ClickValue = c.ClickValue,
								IsBlocked = (request.Id != 0 && c.DeletedDevices.Any(it => it.Id == request.Id))
							});
			}
			catch (Exception ex)
			{
				response.Errors.Add(new ApiValidationErrorItem
				{
					Message = ex.Message
				});
			}

			response.Accepted = response.Errors.Count == 0;

			return response;
		}
		#region - Classes (Requests & Responses) -
		/// <summary>
		/// Request dla metody pobierającej listę kategorii
		/// </summary>
		public class CategoriesRequest
		{
			/// <summary>
			/// Identyfikator
			/// </summary>
			public int Id { get; set; }
		}

		public class BaseRequest
		{
			/// <summary>
			/// Identyfikator
			/// </summary>
			public int Id { get; set; }
		}

		/// <summary>
		/// Response dla metody pobierającej listę kategorii
		/// </summary>
		public class CategoriesResponse : ApiResponse
		{
			/// <summary>
			/// Lista wszystkich kategorii
			/// </summary>
			public List<CatToDev> AvailableCategories { get; set; }

			/// <summary>
			/// Lista powiązanych kategorii z obiektem kampanii
			/// </summary>
			public List<CatToDev> ConnectedCategories { get; set; }
		}

		public class DevicesResponse : ApiResponse
		{
			/// <summary>
			/// Lista wszystkich kategorii
			/// </summary>
			public IEnumerable<Devs> ConnectedDevs { get; set; }
		}

		public class CatToDev : Entity
		{
			public string Code { get; set; }
		}

		public class Devs
		{
			public int Id;
			public string Description;
			public string Name;
		}

		/// <summary>
		/// Klasa pomocniczna
		/// </summary>
		public class CampaignsRequest
		{
			/// <summary>
			/// Identyfikator kategorii
			/// </summary>
			public int ObjectId { get; set; }
		}

		/// <summary>
		/// Klasa pomocnicza
		/// </summary>
		public class CampaignsResponse : ApiResponse
		{
			/// <summary>
			/// Lista wszystkich kampanii
			/// </summary>
			public IEnumerable<CampaignItem> AvailableCampaigns { get; set; }

			/// <summary>
			/// Lista powiązanych kampanii (z obiektem kategorii)
			/// </summary>
			public IEnumerable<CampaignItem> ConnectedCampaigns { get; set; }
		}

		#endregion
	}
}