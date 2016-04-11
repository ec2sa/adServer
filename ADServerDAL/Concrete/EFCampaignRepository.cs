using System.Collections.ObjectModel;
using System.Data.Entity;
using ADServerDAL.Abstract;
using ADServerDAL.Entities.Presentation;
using ADServerDAL.Helpers;
using ADServerDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ADServerDAL.Concrete
{
	/// <summary>
	/// Implementacja repozytorium kampanii
	/// </summary>
	public class EFCampaignRepository : EFBaseRepository, ICampaignRepository
	{
		/// <summary>
		/// Kolekcja wszystkich kampanii
		/// </summary>
		public IQueryable<Campaign> Campaigns
		{
			get
			{
				var query = Context.Campaigns;
				return query;
			}
		}

		/// <summary>
		/// Zapisuje kampanię do bazy danych
		/// </summary>
		/// <param name="campaign">Obiekt kampanii do zapisu</param>
		/// <param name="decrement">Czy tylko zmniejszanie</param>
		public ApiResponse SaveCampaign(Campaign campaign, bool decrement = false)
		{
			var response = new ApiResponse();

			if (campaign.StartDate == DateTime.MinValue || campaign.StartDate == DateTime.MaxValue)
			{
				response.Errors = new List<ApiValidationErrorItem>
				{
					new ApiValidationErrorItem
					{
						Property = "StartDate",
						Message = "Niepoprawna data"
					}
				};
				response.Accepted = false;
				return response;
			}

			if (campaign.EndDate == DateTime.MinValue || campaign.StartDate == DateTime.MaxValue)
			{
				response.Errors = new List<ApiValidationErrorItem>
				{
					new ApiValidationErrorItem
					{
						Property = "EndDate",
						Message = "Niepoprawna data"
					}
				};
				response.Accepted = false;
				return response;
			}

			if (campaign.StartDate.Date > campaign.EndDate.Date)
			{
				response.Errors = new List<ApiValidationErrorItem>
				{
					new ApiValidationErrorItem
					{
						Property = "StartDate",
						Message = "Data rozpoczęcia musi być wcześniejsza od daty zakończenia."
					}
				};
				response.Accepted = false;
				return response;
			}
			using (var transaction = Context.Database.BeginTransaction())
			{
				try
				{
					dynamic dbEntry;

					if (campaign.Id > 0)
					{
						dbEntry = Context.Campaigns.FirstOrDefault(f => f.Id == campaign.Id);

						if (dbEntry != new Campaign())
						{
							if (dbEntry == null)
							{
								response.Errors = new List<ApiValidationErrorItem>
								{
									new ApiValidationErrorItem
									{
										Property = "Id",
										Message = "Brak kampanii"
									}
								};
								response.Accepted = false;
								return response;
							}
							dbEntry.UserId = campaign.UserId;
							dbEntry.Description = campaign.Description;
							dbEntry.EndDate = campaign.EndDate;
							dbEntry.IsActive = campaign.IsActive;
							dbEntry.Name = campaign.Name;
							dbEntry.PriorityId = campaign.PriorityId;
							dbEntry.StartDate = campaign.StartDate;
							dbEntry.ViewValue = campaign.ViewValue;
							dbEntry.ClickValue = campaign.ClickValue;
							if(!decrement)
								SetRelation(campaign, ref dbEntry);

							var duser = Context.Users.First(it => it.Id == campaign.UserId);

							if (!decrement)
							{
								if (dbEntry.AdPoints != campaign.AdPoints && ((dbEntry.AdPoints - campaign.AdPoints) > duser.AdPoints || campaign.AdPoints < 0))
								{
									response.Errors = new List<ApiValidationErrorItem>
									{
										new ApiValidationErrorItem
										{
											Property = "AdPoints",
											Message = "Nierawidłowa ilość punktów na kamapanie"
										}
									};
									response.Accepted = false;
									return response;
								}

								if (dbEntry.AdPoints != campaign.AdPoints && !(dbEntry.AdPoints - campaign.AdPoints > duser.AdPoints || campaign.AdPoints < 0))
								{
									duser.AdPoints += dbEntry.AdPoints - campaign.AdPoints;
								}
							}
							dbEntry.AdPoints = campaign.AdPoints < 0 ? 0 : campaign.AdPoints;

							Context.SaveChanges();

						}
					}
					else
					{
						dbEntry = new Campaign
						{
							Description = campaign.Description,
							EndDate = campaign.EndDate,
							IsActive = campaign.IsActive,
							Name = campaign.Name,
							PriorityId = campaign.PriorityId,
							StartDate = campaign.StartDate,
							UserId = campaign.UserId ?? 0,
							AdPoints = campaign.AdPoints,
							ViewValue = campaign.ViewValue,
							ClickValue = campaign.ClickValue
						};

						var dbUser = Context.Users.First(it => it.Id == campaign.UserId);
						if (dbEntry.AdPoints != campaign.AdPoints)
						{
							dbUser.AdPoints -= campaign.AdPoints;
						}

						if (dbEntry.AdPoints != campaign.AdPoints || (campaign.AdPoints > dbUser.AdPoints || campaign.AdPoints < 0))
						{
							response.Errors = new List<ApiValidationErrorItem>
								{
									new ApiValidationErrorItem
									{
										Property = "AdPoints",
										Message = "Nierawidłowa ilość punktów na kamapanie"
									}
								};
							response.Accepted = false;
							return response;
						}

						SetRelation(campaign, ref dbEntry);

						Context.Campaigns.Add(dbEntry);
						Context.SaveChanges();
					}

					transaction.Commit();
				}
				catch (System.Data.Entity.Validation.DbEntityValidationException ex)
				{
					// Obsługa błędów EF
					using (var handler = new DbValidationErrorHandler(ex))
					{
						if (handler.HasErrors)
						{
							response.Errors.AddRange(handler.ValidationErrors);
						}
					}
				}

				catch (Exception ex)
				{
					// Obsługa błędów pozostałych
					var hierarchy = new List<Exception>();
					ExceptionsHandlingHelper.HierarchizeError(ex, ref hierarchy);
					if (hierarchy.Count > 0)
					{
						response.Errors.AddRange(hierarchy.Select(s => new ApiValidationErrorItem { Message = s.Message + Environment.NewLine + s.StackTrace }).Distinct().AsEnumerable());
					}
				}

				if (response.Errors.Count > 0)
				{
					transaction.Rollback();
				}
			}

			response.Accepted = response.Errors.Count == 0;
			return response;
		}
		///Todo: optymalizacja!
		private void SetRelation(Campaign campaign, ref dynamic dbEntry)
		{
			ObjectRelationCategory(campaign.Categories, ref dbEntry);
			ObjectRelationMmObjects(campaign.MultimediaObjects, ref dbEntry);
			ObjectRelationDevice(campaign.Devices, ref dbEntry);
			ObjectRelationDeletedDevice(campaign.DeletedDevices, ref dbEntry);
		}

		/// <summary>
		/// Zwraca identyfikatory kampanii powiązanych z daną kategorią
		/// </summary>
		/// <param name="categoryId">Identyfikator kategorii</param>
		public IEnumerable<int> CampaignsToCategory(int categoryId)
		{
			var query = Context.Categories
						.Single(it => it.Id == categoryId)
						.Campaigns
						.Select(it => it.Id);
			return query;
		}

		/// <summary>
		/// Zwraca identyfikatory kampanii powiązanych z danym obiektem multimedialnym
		/// </summary>
		/// <param name="objectId"></param>
		public IEnumerable<int> CampaignsToObject(int objectId)
		{
			var query = Context.MultimediaObjects
				.Single(it => it.Id == objectId)
				.Campaigns
				.Select(it => it.Id);
			return query;
		}

		/// <summary>
		/// Pobiera obiekt kampanii na podstawie zadanego identyfikatora
		/// </summary>
		/// <param name="id">Identyfikator kampanii</param>
		public Campaign GetById(int id)
		{
			var campaign = Context.Campaigns.SingleOrDefault(c => c.Id == id);
			return campaign;
		}

		/// <summary>
		/// Usuwa kampanię z bazy danych na podstawie zadanego identyfikatora
		/// </summary>
		/// <param name="id">Identyfikator kampanii</param>
		public ApiResponse Delete(int id)
		{
			var response = new ApiResponse();

			Campaign deletedObject = Context.Campaigns.Find(id);
			if (deletedObject != null)
			{
				try
				{
					var removeDevs =
						(from it in deletedObject.Devices.Select(it => it.Id)
						 from c in Context.Devices
						 where c.Id == it
						 select c).ToList();

					//var removeCats =
					//	(from it in deletedObject.Campaign_Category.Select(it => it.Id)
					//	 from c in Context.Campaign_Category
					//	 where c.Id == it
					//	 select c).ToList();

					//var removeMultis =
					//	(from it in deletedObject.MultimediaObject_Campaign.Select((it => it.Id))
					//	 from m in Context.MultimediaObject_Campaign
					//	 where m.Id == it
					//	 select m).ToList();

					foreach (var it in removeDevs)
						it.Campaigns.Remove(deletedObject);
					//foreach (var it in removeCats)
					//	Context.Campaign_Category.Remove(it);
					//foreach (var it in removeMultis)
					//	Context.MultimediaObject_Campaign.Remove(it);

					//deletedObject.PriorityId = null;
					deletedObject.UserId = null;

					Context.Entry(deletedObject).State = EntityState.Deleted;

					Context.Campaigns.Remove(deletedObject);
					Context.SaveChanges();
				}
				catch (Exception)
				{
					response.Errors.Add(new ApiValidationErrorItem
						{
							Message = "Nie można usunąć kampani - upewnij się, że nie istnieją obiekty powiązane."
						});
				}
			}
			else
			{
				response.Errors.Add(new ApiValidationErrorItem
					{
						Message = "Kampania została już wcześniej usunięta"
					});
			}

			response.Accepted = response.Errors.Count == 0;
			return response;
		}

		/// <summary>
		/// Możliwość ustawienia innego kontekstu niż wbudowany
		/// </summary>
		/// <param name="context">Nowy kontekst EF</param>
		public void SetContext(System.Data.Entity.DbContext context)
		{
			SetNewContext(context);
		}

        public Campaign GetByName(string Name)
        {
            return Context.Campaigns.FirstOrDefault(p => p.Name == Name);
        }
	}
}