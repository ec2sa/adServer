using ADServerDAL.Abstract;
using ADServerDAL.Entities.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADServerDAL.Helpers;
using System.IO;
using System.Drawing;
using ADServerDAL.Models;
using System.Threading;

namespace ADServerDAL.Concrete
{
	/// <summary>
	/// Impelementacja repozytorium obiektów mulimedialnych
	/// </summary>
	public class EFMultimediaObjectRepository : EFBaseRepository, IMultimediaObjectRepository
	{
		/// <summary>
		/// Kolekcja wszystkich obiektów
		/// </summary>
		public IQueryable<MultimediaObject> MultimediaObjects
		{
			get
			{
				var query = Context.MultimediaObjects;
				return query;
			}
		}


		/// <summary>
		/// Lista identyfikatorów obiektów przypisanych do danej kampanii
		/// </summary>
		/// <param name="campaignId">Identyfikator kampanii</param>  
		public IQueryable<int> ObjectsToCampaign(int campaignId)
		{
			var query = (from cc in Context.MultimediaObjects
						 where cc.Campaigns.Any(it => it.Id == campaignId)
						 select cc.Id);
			return query;
		}

		/// <summary>
		/// Zapisuje obiekt multimedialny do bazy
		/// </summary>
		/// <param name="multimediaObject">Obiekt multimedialny</param>  
        public ApiResponse Save(MultimediaObject multimediaObject, List<Campaign> Camps)
		{
			var response = new ApiResponse();

			if (!string.IsNullOrEmpty(multimediaObject.FileContent))
			{
				var index = multimediaObject.FileContent.IndexOf("base64,");
				if (index != -1)
				{
					multimediaObject.FileContent = multimediaObject.FileContent.Substring(index + "base64,".Length);
				}
				if (multimediaObject.Url == null)
				{
					throw new Exception("Nie ustawiono adres URL dla obiektu!");
				}

				using (var transaction = Context.Database.BeginTransaction())
				{
					try
					{
						dynamic dbEntry;
						var multimediaObjectType = Context.Types.FirstOrDefault(x => x.Id == multimediaObject.TypeId);
						if (multimediaObjectType == null)
						{
							throw new Exception("Nie znaleziono wskazanego typu obiektu. Możliwe zmiany na innym stanowisku.");
						}

						// Konwersja formatu base64 na tablicę bajtów
						var imageBytes = Convert.FromBase64String(multimediaObject.FileContent);

						byte[] thumbnail = imageBytes;

						#region Zapis danych podstawowych obiektu
						if (multimediaObject.Id > 0)
						{
                            dbEntry = Context.MultimediaObjects.SingleOrDefault(f => f.Id == multimediaObject.Id);
                            if (dbEntry != null)
                            {
                                dbEntry.UserId = multimediaObject.UserId;

                                dbEntry.FileName = multimediaObject.FileName;
                                dbEntry.MimeType = multimediaObject.MimeType;
                                dbEntry.Name = multimediaObject.Name;
                                dbEntry.TypeId = multimediaObject.TypeId;
                                dbEntry.Url = multimediaObject.Url;
                                dbEntry.Contents = imageBytes;
                                dbEntry.Thumbnail = thumbnail;

                                SetRelation(multimediaObject, ref dbEntry);
                                Context.SaveChanges();
							}
						}
						else
						{
                            ICollection<Campaign> CampList = new List<Campaign>();
                            foreach (var camp in Camps)
                            {
                                Campaign c = Context.Campaigns.FirstOrDefault(p => p.Id == camp.Id);
                                Context.Campaigns.Attach(c);
                                CampList.Add(c);
                            }

							dbEntry = new MultimediaObject
							{
								FileName = multimediaObject.FileName,
								MimeType = multimediaObject.MimeType,
								Name = multimediaObject.Name,
								TypeId = multimediaObject.TypeId,
								Url = multimediaObject.Url,
								Contents = imageBytes,
								Thumbnail = thumbnail,
								UserId = multimediaObject.UserId,
                                Campaigns = CampList,
                                Content = multimediaObject.Contents,
                                
							};
                            
                            SetRelation(multimediaObject,ref dbEntry);
                            
                            Context.MultimediaObjects.Add(dbEntry);
							Context.SaveChanges();
						}
						#endregion


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
			}
			else
			{
				response.Errors.Add(new ApiValidationErrorItem { Message = "Należy dołączyć plik multimedialny." });
			}

			response.Accepted = response.Errors.Count == 0;
			return response;
		}

		private void SetRelation(MultimediaObject MMobject, ref dynamic dbEntry)
		{
			ObjectRelationCampaign(MMobject.Campaigns, ref dbEntry);
		}

		/// <summary>
		/// Zwraca obiekt multimedialny na podstawie identyfikatora
		/// </summary>
		/// <param name="id">Identyfikator obiektu multimedialnego</param>   
		public MultimediaObject GetById(int id)
		{
			var multimediaObject = Context.MultimediaObjects.FirstOrDefault(c => c.Id == id);
			return multimediaObject;
		}

		/// <summary>
		/// Usuwa obiekt multimedialny na podstawie identyfikatora
		/// </summary>
		/// <param name="id">Identyfikator obiektu multimedialnego</param>  
		public ApiResponse Delete(int id)
		{
			var response = new ApiResponse();

			var deletedObject = Context.MultimediaObjects.Find(id);
			if (deletedObject != null)
			{
				try
				{
					Context.MultimediaObjects.Remove(deletedObject);
					Context.SaveChanges();
				}
				catch (Exception)
				{
					response.Errors.Add(new ApiValidationErrorItem
					{
						Message = "Nie można usunąć obiektu multimedialnego - upewnij się, że nie istnieją obiekty powiązane."
					});
				}
			}
			else
			{
				response.Errors.Add(new ApiValidationErrorItem
				{
					Message = "Obiekt został już wcześniej usunięty"
				});
			}

			response.Accepted = response.Errors.Count == 0;
			return response;
		}


		/// <summary>
		/// Zwraca obiekt multimedialny z wypełnionym polem miniaturki zdjęcia
		/// </summary>
		/// <param name="id">Identyfikator obiektu multimedialnego</param> 
		public MultimediaObject GetThumbnail(int id)
		{
			var obj = Context.MultimediaObjects.FirstOrDefault(it => it.Id == id);
			return obj;
		}

		/// <summary>
		/// Możliwość ustawienia innego kontekstu niż wbudowany
		/// </summary>
		/// <param name="context">Nowy kontekst EF</param>
		public void SetContext(System.Data.Entity.DbContext context)
		{
			SetNewContext(context);
		}

        public int GetIntFromCamName(string name)
        {
            var C = Context.Campaigns.FirstOrDefault(p => p.Name == name);
            var id = Context.MultimediaObjects.FirstOrDefault(p => p.Campaigns.FirstOrDefault(m => m.Name == name) == C).Id;

            return id;
        }

        public MultimediaObject GetFromCamName(string name)
        {
            return GetById(GetIntFromCamName(name));
        }

        public MultimediaObject GetByCampAndType(Device device)
        {
            var u = Context.Campaigns.FirstOrDefault(it => it.Name == "LogoEC2" && it.User.Role.Name == "Admin");
            var image = Context.MultimediaObjects.FirstOrDefault(p => p.Campaigns.FirstOrDefault(m => m.Id == u.Id).MultimediaObjects.FirstOrDefault(n => n.TypeId == device.Id) != null);

            return image;
        }

        #region CropImage

        public ApiResponse CropAndSave(int id,byte[] Image)
        {
            var obj = Context.MultimediaObjects.FirstOrDefault(p => p.Id == id);
            var typ = Context.Types.FirstOrDefault(m => m.Id == obj.TypeId);

            ImageProcesorHelper.ResizeImageResult resizeResult = ImageProcesorHelper.ResizeImage(typ.Width, typ.Height, Image);
            Image = resizeResult.ResizedImage;
            byte[] thumbnail = resizeResult.Thumbnail;

            obj.Contents = Image;
            obj.Content = Image;
            obj.FileContent = Convert.ToBase64String(Image);
            obj.Thumbnail = thumbnail;

            Context.SaveChanges();

            var response = new ApiResponse();
            response.Accepted = true;

            return response;
        }

        #endregion

        #region helpers
        public MultimediaObject EditCamps(MultimediaObject image, List<int> CampsList)
        {
            //var image = Context.MultimediaObjects.FirstOrDefault(p => p.Id == imageId);

                foreach (var campItem in image.Campaigns.ToList())
                {
                    if (!CampsList.Contains(campItem.Id))
                    {
                        image.Campaigns.Remove(campItem);
                    }
                }

                foreach (var campItem in CampsList)
                {
                    if (!image.Campaigns.Any(c => c.Id == campItem))
                    {
                        var campa = Context.Campaigns.SingleOrDefault(c => c.Id == campItem);
                        image.Campaigns.Add(campa);
                    }
                }
                //Context.SaveChanges();

                return image;
        }
        #endregion
    }
}
