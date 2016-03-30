using ADServerDAL;
using ADServerDAL.Abstract;
using ADServerDAL.Entities.Presentation;
using ADServerDAL.Models;
using ADServerDAL.Models.Base;
using ADServerManagementWebApplication.Infrastructure;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using ADServerManagementWebApplication.Extensions;

namespace ADServerManagementWebApplication.Controllers
{
    /// <summary>
    /// API kontroler do zarządzania kampaniami
    /// </summary>
    public class ApiCampaignController : AdServerBaseApiController
    {
        #region - Fields -
        /// <summary>
        /// repozytorium kampanii
        /// </summary>
        private ICampaignRepository campaignRepository;

        /// <summary>
        /// repozytorium kategorii
        /// </summary>
        private ICategoryRepository categoryRepository;

        /// <summary>
        /// repozytorium obiektów multimedialnych
        /// </summary>
        private IMultimediaObjectRepository objectRepository; 
        #endregion

        #region - Constructors -
        public ApiCampaignController(ICampaignRepository campaignRepository, ICategoryRepository categoryRepository, IMultimediaObjectRepository objectRepository)
        {
            this.campaignRepository = campaignRepository;
            this.categoryRepository = categoryRepository;
            this.objectRepository = objectRepository;
        } 
        #endregion

        #region - Overriden methods -
        /// <summary>
        /// Zwalnianie zasobów
        /// </summary>
        protected override void OnDisposeController()
        {
            if (campaignRepository != null)
            {
                campaignRepository.Dispose();
                campaignRepository = null;
            }

            if (categoryRepository != null)
            {
                categoryRepository.Dispose();
                categoryRepository = null;
            }

            if (objectRepository != null)
            {
                objectRepository.Dispose();
                objectRepository = null;
            }
        } 
        #endregion

        #region - Public methods -
        /// <summary>
        /// Zapisuje obiekt kampanii do bazy danych
        /// </summary>
        /// <param name="campaign"></param>
        /// <returns></returns>
        public ApiResponse SaveCampaign(Campaign campaign)
        {
			if (campaign.UserId == null || campaign.UserId == 0)
				campaign.UserId = User.GetUserIDInt();

            return campaignRepository.SaveCampaign(campaign);
        }

        /// <summary>
        /// Pobiera listę kategorii z uwzględnieniem kategorii powiązanych z daną kampanią
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CategoriesResponse Categories([FromBody]CategoriesRequest request)
        {
            var response = new CategoriesResponse();
            try
            {
                var allCategories = categoryRepository.Categories.Select(it=> new CatToCamp(){Code = it.Code, Id = it.Id, Name = it.Name});

                var connectedCategories = new List<int>();

                if (request.Id > 0)
                {
                    var categoriesToCampaign = categoryRepository.CategoriesToCampaign(request.Id);
                    if (categoriesToCampaign != null && categoriesToCampaign.Count > 0)
                    {
                        connectedCategories.AddRange(categoriesToCampaign);
                        categoriesToCampaign.Clear();
                    }
                }

				response.AvailableCategories = new List<CatToCamp>();
				response.ConnectedCategories = new List<CatToCamp>();

				foreach (CatToCamp item in allCategories)
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
        /// Pobiera listę obiektów multimedialnych z uwzględnieniem powiązań z daną kampanią
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ObjectsResponse Objects([FromBody] ObjectsRequest request)
        {
            var response = new ObjectsResponse();
            try
            {
				var adminRole =  User.IsInRole("Admin");
				var id = adminRole  && request.CampaignID != 0 ? (int)campaignRepository.Campaigns.FirstOrDefault(it => it.Id == request.CampaignID).UserId : User.GetUserIDInt();

                var allObjects = objectRepository.MultimediaObjects.Select(it=> new MMToCamp{Id = it.Id, Name = it.Name, TypeName = it.Type.Name, Mime = it.MimeType, UserId = it.UserId, Height =  it.Type.Height, Width = it.Type.Width});
				
                var connectedObjects = new List<int>();

                if (request.CampaignID > 0)
                {
                    var objectsToCampaign = objectRepository.ObjectsToCampaign(request.CampaignID).ToList();
                    if (objectsToCampaign != null && objectsToCampaign.Any())
                    {
                        connectedObjects.AddRange(objectsToCampaign);
                    }
                }

                response.AvailableObjects = new List<MMToCamp>();
				response.ConnectedObjects = new List<MMToCamp>();

                foreach (var item in allObjects)
                {
                    if (connectedObjects.Contains(item.Id))
                    {
                        response.ConnectedObjects.Add(item);
                    }
					else if (item.UserId == id || (adminRole && request.CampaignID == 0))
                    {
                        response.AvailableObjects.Add(item);
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
        #endregion
    }

    #region - Classess (Requests & Responses) -

    /// <summary>
    /// Request dla metody pobierającej listę kategorii
    /// </summary>
    public class CategoriesRequest
    {
        /// <summary>
        /// Identyfikator kampanii
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
		public List<CatToCamp> AvailableCategories { get; set; }

        /// <summary>
        /// Lista powiązanych kategorii z obiektem kampanii
        /// </summary>
		public List<CatToCamp> ConnectedCategories { get; set; }
    }

	public class CatToCamp : Entity
	{
		public string Code { get; set; }
	}
	public class MMToCamp : Entity
	{
		public string TypeName { get; set; }
		public string Mime { get; set; }
		public int? UserId { get; set; }
		public int Width { get; set; }
		public int Height; 
	}

    /// <summary>
    /// Request dla metody pobierającej listę obiektów multimedialnych
    /// </summary>
    public class ObjectsRequest
    {
        /// <summary>
        /// Identyfikator kampanii
        /// </summary>
        public int CampaignID { get; set; }
    }

    /// <summary>
    /// Response dla metody pobierającej listę obiektów multimedialnych
    /// </summary>
    public class ObjectsResponse : ApiResponse
    {
        /// <summary>
        /// Lista powiązanych obiektów multimedialnych z obiektem kampanii
        /// </summary>
		public List<MMToCamp> ConnectedObjects { get; set; }

        /// <summary>
        /// Lista wszystkich obiektów multimedialnych
        /// </summary>
		public List<MMToCamp> AvailableObjects { get; set; }
    }

    #endregion
}