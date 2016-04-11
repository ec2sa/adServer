using System.Collections.ObjectModel;
using ADServerDAL.Abstract;
using ADServerDAL.Entities.Presentation;
using ADServerDAL.Helpers;
using ADServerDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using ADServerDAL.Other;

namespace ADServerDAL.Concrete
{
	public class EFUsersRepository : EFBaseRepository, IUsersRepository
	{
		public IQueryable<User> Users
		{
			get
			{
				var query = Context.Users;
				return query;
			}
		}

		/// <summary>
		/// Zapisywanie/Modyfikacja użytkownika
		/// </summary>
		/// <param name="user">Użytkownik</param>
		/// <returns>Odpowiedź funkcji</returns>
		public ApiResponse Save(User user)
		{
			var response = new ApiResponse { Errors = new List<ApiValidationErrorItem>() };

			#region Errors

			if (user.RoleId == 0)
			{
				response.Errors.Add(new ApiValidationErrorItem { Property = "Role", Message = "Nieprawidłowe uprawnienia użytkownika" });
				response.Accepted = false;
				return response;
			}

			#endregion Errors

			using (var transaction = Context.Database.BeginTransaction())
			{
				try
				{
					User dbEntry;

					#region Zapis danych podstawowych kampanii

					if (user.Id > 0)
					{
						dbEntry = Context.Users.FirstOrDefault(f => f.Id == user.Id);
						if (dbEntry != null)
						{
							if (!string.IsNullOrEmpty(user.Name))
							{
								dbEntry.Name = user.Name;
							}
							if (!string.IsNullOrEmpty(user.LastName))
							{
								dbEntry.LastName = user.LastName;
							}
							if (!string.IsNullOrEmpty(user.FirstName))
							{
								dbEntry.FirstName = user.FirstName;
							}
							if (!string.IsNullOrEmpty(user.Password))
							{
								dbEntry.Password = user.Password;
							}
							if (user.CompanyAddress != null)
							{
								dbEntry.CompanyAddress = user.CompanyAddress;
							}
							if (user.CompanyName != null)
							{
								dbEntry.CompanyName = user.CompanyName;
							}
							if (user.AdditionalInfo != null)
							{
								dbEntry.AdditionalInfo = user.AdditionalInfo;
							}
							if (!string.IsNullOrEmpty(user.Email))
							{
								dbEntry.Email = user.Email;
							}
							if (!string.IsNullOrEmpty(user.Url))
							{
								dbEntry.Url = user.Url;
							}
							dbEntry.RoleId = user.RoleId;
							dbEntry.AdPoints = user.AdPoints;
							dbEntry.IsBlocked = user.IsBlocked;
							Context.SaveChanges();
						}
					}
					else
					{
						if (string.IsNullOrWhiteSpace(user.Name))
						{
							response.Errors.Add(new ApiValidationErrorItem { Property = "Name", Message = "Nieprawidłowe dane" });
							response.Accepted = false;
							return response;
						}
						dbEntry = new User
						{
							Name = user.Name,
							LastName = user.LastName,
							FirstName = user.FirstName,
							Password = user.Password,
							Email = user.Email,
							AdditionalInfo = user.AdditionalInfo,
							CompanyName = user.CompanyName,
							CompanyAddress = user.CompanyAddress,
							Url = user.Url,
							IsBlocked = user.IsBlocked,
							RoleId = user.RoleId,
							AdPoints = user.AdPoints
						};
						Context.Users.Add(dbEntry);
						Context.SaveChanges();

						var device = new Device();
						var devRepo = new EFDeviceRepository();
                        var imageRepo = new EFMultimediaObjectRepository();
						var u = Context.Campaigns.FirstOrDefault(it => it.Name == "LogoEC2" && it.User.Role.Name == "Admin");

						device.Name = "LogoEC2";
						device.Description = "Przykładowa kampania AdServera";
                        device.TypeId = (int)u.MultimediaObjects.First().TypeId;
						device.UserId = dbEntry.Id;

						var camps = new List<Campaign>();
						var cats = new List<Category>();
						camps.Add(u);
						cats.AddRange(u.Categories);

						device.Campaigns.Add(u);
						//device.Categories = u.Categories;
						devRepo.Save(device);
						dbEntry.Devices = new Collection<Device> { device };
						Context.SaveChanges();

					}

					#endregion Zapis danych podstawowych kampanii

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

		public ApiResponse Delete(int id)
		{
			throw new NotImplementedException();
		}
	}
}