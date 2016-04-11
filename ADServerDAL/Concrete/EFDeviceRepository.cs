using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity;
using ADServerDAL.Abstract;
using ADServerDAL.Entities.Presentation;
using System;
using System.Linq;
using ADServerDAL.Helpers;
using ADServerDAL.Models;
using ADServerDAL.Models.Base;
using ADServerDAL.Models.Interface;

namespace ADServerDAL.Concrete
{
	public class EFDeviceRepository : EFBaseRepository, IDeviceRepository
	{
		public IQueryable<Device> Devices
		{
			get
			{
				return Context.Devices;
			}
		}

		/// <summary>
		/// Zapisywanie uprawnień do bazy danych
		/// </summary>
		/// <param name="device">Nośnik</param>
		/// <returns>Odpowiedź o stanie zapisu</returns>
		public ApiResponse Save(Device device)
		{
			var response = new ApiResponse();

			try
			{
				if (device.Id == 0)
				{

					dynamic dbEntry = new Device();

					dbEntry.Name = device.Name;
					dbEntry.UserId = device.UserId;
					dbEntry.TypeId = device.TypeId;
					SetRelation(device,ref dbEntry);
					dbEntry.Description = device.Description;
					Context.Devices.Add(dbEntry);
				}
				else
				{
					dynamic dbEntry = Context.Devices.Find(device.Id);
					if (dbEntry != null)
					{
						SetRelation(device,ref dbEntry);

						dbEntry.Name = device.Name;
						dbEntry.TypeId = device.TypeId;
						dbEntry.Statistics = device.Statistics;
						dbEntry.UserId = device.UserId;
						dbEntry.Description = device.Description;
					}
				}

				Context.SaveChanges();
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
				var hierarchy = new List<Exception>();
				ExceptionsHandlingHelper.HierarchizeError(ex, ref hierarchy);
				if (hierarchy.Count > 0)
				{
					response.Errors.AddRange(hierarchy.Select(s => new ApiValidationErrorItem { Message = s.Message }).Distinct().AsEnumerable());
				}
			}

			response.Accepted = response.Errors.Count == 0;
			return response;
		}

		/// <summary>
		/// Usuwanie uprawnień
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Odpowiedź o stanie zapisu</returns>
		public ApiResponse Delete(int id)
		{
			var response = new ApiResponse();

			var deletedObject = Context.Devices.Find(id);
			if (deletedObject != null)
			{
				try
				{
					var removeCamp =
						(from it in deletedObject.Campaigns.Select(it => it.Id)
						 from c in Context.Campaigns
						 where c.Id == it
						 select c).ToList();
					var removeStats =
						(from it in deletedObject.Statistics.Select(it => it.Id)
						 from s in Context.Statistics
						 where s.Id == it
						 select s).ToList();

					var removeCats =
						(from it in deletedObject.Categories.Select(it => it.Id)
						 from s in Context.Categories
						 where s.Id == it
						 select s).ToList();

					foreach (var it in removeCamp)
						deletedObject.Campaigns.Remove(it);
					foreach (var it in removeStats)
						it.DeviceId = null;
					foreach (var it in removeCats)
					{
						deletedObject.Categories.Remove(it);
					}
					Context.Devices.Remove(deletedObject);

					Context.SaveChanges();
				}
				catch (Exception e)
				{
					var a = e;
					response.Errors.Add(new ApiValidationErrorItem
					{
						Message = "Nie można usunąć uprawnień - upewnij się, że nie istnieją obiekty powiązane."
					});
				}
			}
			else
			{
				response.Errors.Add(new ApiValidationErrorItem
				{
					Message = "Uprawnienia zostały już wcześniej usunięty"
				});
			}

			response.Accepted = response.Errors.Count == 0;
			return response;
		}

		public IEnumerable<int> CatoegoriesToDev(int Id)
		{
			var asd = Context.Devices
				.First(it => it.Id == Id);
			var query = Context.Devices
						.First(it => it.Id == Id)
						.Categories
						.Select(it => it.Id);
			return query;
		}

		public void SetContext(System.Data.Entity.DbContext context)
		{
			SetNewContext(context);
		}


		///Todo: optymalizacja!
		private void SetRelation(Device device, ref dynamic dbEntry)
		{
			ObjectRelationCategory(device.Categories, ref dbEntry);
			ObjectRelationCampaign(device.Campaigns, ref dbEntry);
		}

        public Device GetDeviceById(int id)
        {
            return Context.Devices.FirstOrDefault(p => p.Id == id);
        }
	}
}

