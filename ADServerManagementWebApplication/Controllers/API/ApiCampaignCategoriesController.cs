using System.Linq;
using ADServerDAL.Abstract;
using ADServerDAL.Entities.Presentation;
using ADServerDAL.Models;
using ADServerManagementWebApplication.Infrastructure;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ADServerManagementWebApplication.Controllers
{
    /// <summary>
    /// API kontroler do zarządzania kategoriami
    /// </summary>
    public class ApiCampaignCategoriesController : AdServerBaseApiController
    {
        #region - Fields -
        /// <summary>
        /// Repozytorium kategorii
        /// </summary>
        private ICategoryRepository categoryRepository;

        /// <summary>
        /// Repozytorium kampanii
        /// </summary>
        private ICampaignRepository campaignRepository;
        #endregion

        #region - Constructors -
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="categoryRepository">Repozytorium kategorii</param>
        /// <param name="campaignRepository">Repozytorium kampanii</param>
        public ApiCampaignCategoriesController(ICategoryRepository categoryRepository, ICampaignRepository campaignRepository)
        {
            this.campaignRepository = campaignRepository;
            this.categoryRepository = categoryRepository;
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
        } 
        #endregion

        #region - Public methods -
        /// <summary>
        /// Zapisywanie obiektu kategorii do bazy danych
        /// </summary>
        /// <param name="category">Obiekt kategorii</param>
        public ApiResponse SaveCategory(Category category)
        {
            return categoryRepository.Save(category);
        }

        /// <summary>
        /// Pobranie listy kampanii dla zadanej kategorii
        /// </summary>
        /// <param name="request"></param>        
        public CampaignsResponse Campaigns([FromBody] CampaignsRequest request)
        {
            var response = new CampaignsResponse();

            try
            {
				var allCampaigns = campaignRepository.Campaigns.Select(it => new CampToCat{Id = it.Id, ClickValue = it.ClickValue, EndDate = it.EndDate, IsActive = it.IsActive, Name = it.Name, ViewValue = it.ViewValue, StartDate = it.StartDate});

                var connectedCampaigns = new List<int>();

                if (request.ObjectId > 0)
                {
	                var campaignsToCategory = categoryRepository.Categories
						.Single(it => it.Id == request.ObjectId)
						.Campaigns.Select(it=>it.Id);

                    if (campaignsToCategory.Any())
                    {
                        connectedCampaigns.AddRange(campaignsToCategory);
                    }
                }

				response.AvailableCampaigns = new List<CampToCat>();
				response.ConnectedCampaigns = new List<CampToCat>();

				foreach (var item in allCampaigns)
                {
                    if (connectedCampaigns.Contains(item.Id))
                    {
                        response.ConnectedCampaigns.Add(item);
                    }
                    else
                    {
                        response.AvailableCampaigns.Add(item);
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

        #region - Classes (Requests & Responses) -

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
			public List<CampToCat> AvailableCampaigns { get; set; }

            /// <summary>
            /// Lista powiązanych kampanii (z obiektem kategorii)
            /// </summary>
			public List<CampToCat> ConnectedCampaigns { get; set; }
        }

	    public class CampToCat
	    {
		    public int Id;
		    public string Name;
		    public bool IsActive;
		    public decimal ViewValue;
		    public decimal ClickValue;
		    public DateTime StartDate;
		    public DateTime EndDate;
	    }
        #endregion
    }
}