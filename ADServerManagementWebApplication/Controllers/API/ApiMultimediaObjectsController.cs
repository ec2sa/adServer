using System.Collections.Generic;
using System.Web.Http;
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
using System;
using System.Linq;
using System.Web.Mvc;


namespace ADServerManagementWebApplication.Controllers
{

    /// <summary>
    /// API kontroler do obsługi obiektów multimedialnych
    /// </summary>
    public class ApiMultimediaObjectsController : AdServerBaseApiController
    {
        #region - Fields -
        /// <summary>
        /// Repozytorium obiektów
        /// </summary>
        private IMultimediaObjectRepository objectRepository;

        /// <summary>
        /// Repozytorium kampanii
        /// </summary>
        private ICampaignRepository campaignRepository;

        /// <summary>
        /// Repozytorium typów obiektów
        /// </summary>
        private ITypeRepository typeRepository; 
        #endregion

        #region - Constructors -
        public ApiMultimediaObjectsController(IMultimediaObjectRepository objectRepository, ICampaignRepository campaignRepository, ITypeRepository typeRepository)
        {
            this.objectRepository = objectRepository;
            this.campaignRepository = campaignRepository;
            this.typeRepository = typeRepository;
        } 
        #endregion

        #region - Overriden methods -
        /// <summary>
        /// Zwalnianie zasobów
        /// </summary>
        protected override void OnDisposeController()
        {
            if (objectRepository != null)
            {
                objectRepository.Dispose();
                objectRepository = null;
            }

            if (campaignRepository != null)
            {
                campaignRepository.Dispose();
                campaignRepository = null;
            }

            if (typeRepository != null)
            {
                typeRepository.Dispose();
                typeRepository = null;
            }
        } 
        #endregion

        #region - Public methods -
        /// <summary>
        /// Zapisuje obiekt multimedialny do bazy danych
        /// </summary>
        /// <param name="multimediaObject">Obiekt multimedialny</param>        
        public ApiResponse SaveObject(MultimediaObject multimediaObject)
        {
			if(multimediaObject.UserId == 0 || (!User.IsInRole("Admin") && User.GetUserIDInt() != multimediaObject.UserId))
				multimediaObject.UserId = User.GetUserIDInt();
            List<Campaign> CampList = multimediaObject.Campaigns.ToList();
            return objectRepository.Save(multimediaObject, CampList);
        }

        /// <summary>
        /// Zwraca listę kampanii
        /// </summary>
        /// <param name="request"></param>        
        public CampaignsResponse Campaigns([FromBody] CampaignsRequest request)
        {
            var response = new CampaignsResponse();

            try
            {
				var adminRole =  User.IsInRole("Admin") ;
				var id = adminRole && request.Id != 0 ? (int)objectRepository.MultimediaObjects.FirstOrDefault(it => it.Id == request.Id).UserId : User.GetUserIDInt();
                
				var allCampaigns = campaignRepository.Campaigns
					.Where(it => it.UserId == id || (adminRole && request.Id == 0))
					.Select(it => new CampToMM { Id = it.Id, ClickValue = it.ClickValue, EndDate = it.EndDate, IsActive = it.IsActive, Name = it.Name, ViewValue = it.ViewValue, StartDate = it.StartDate });
				
                if(adminRole)
                {
                    allCampaigns = campaignRepository.Campaigns.Select(it => new CampToMM { Id = it.Id, ClickValue = it.ClickValue, EndDate = it.EndDate, IsActive = it.IsActive, Name = it.Name, ViewValue = it.ViewValue, StartDate = it.StartDate });
                }

                var connectedCampaigns = new List<int>();

                if (request.Id > 0)
                {
                    var campaignsToObject = campaignRepository.CampaignsToObject(request.Id);
                    if (campaignsToObject.Any())
                    {
                        connectedCampaigns.AddRange(campaignsToObject);
                    }
                }

				response.AvailableCampaigns = new List<CampToMM>();
				response.ConnectedCampaigns = new List<CampToMM>();

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

        /// <summary>
        /// Zwraca informacje o typie obiektu multiedialnego
        /// </summary>
        /// <param name="request">Parametr żądania</param>        
        public TypeInformationResponse TypeInformation([FromBody] TypeInformationRequest request)
        {
            TypeInformationResponse response = new TypeInformationResponse();

            try
            {
                response.TypeItem = new MultimediaTypeItem();
                response.TypeItem.ID = request.TypeId;

                if (request.TypeId > 0)
                {
                    response.TypeItem = typeRepository.GetPresenterById(request.TypeId);
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

        #region  - Classess (Requests & Responses) -

        /// <summary>
        /// Klasa pomocnicza
        /// </summary>
        public class CampaignsRequest
        {
            public int Id { get; set; }
        }

        /// <summary>
        /// Klasa pomocnicza
        /// </summary>
        public class CampaignsResponse : ApiResponse
        {
            /// <summary>
            /// Lista wszystkich kampanii
            /// </summary>
			public List<CampToMM> AvailableCampaigns { get; set; }

            /// <summary>
            /// Lista kampanii powiązanych z obiektem multimedialnym
            /// </summary>
			public List<CampToMM> ConnectedCampaigns { get; set; }
        }

        /// <summary>
        /// Klasa pomocnicza
        /// </summary>
        public class TypeInformationRequest
        {
            /// <summary>
            /// Identyfikator typu obiektu
            /// </summary>
            public int TypeId { get; set; }
        }

        /// <summary>
        /// Klasa pomocnicza
        /// </summary>
        public class TypeInformationResponse : ApiResponse
        {
            /// <summary>
            /// Obiekt typu
            /// </summary>
            public MultimediaTypeItem TypeItem { get; set; }
        }
		public class CampToMM
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