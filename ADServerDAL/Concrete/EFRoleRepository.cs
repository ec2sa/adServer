using System.Collections.Generic;
using ADServerDAL.Abstract;
using ADServerDAL.Entities.Presentation;
using System;
using System.Linq;
using ADServerDAL.Helpers;
using ADServerDAL.Models;

namespace ADServerDAL.Concrete
{
	public class EFRoleRepository : EFBaseRepository, IRoleRepository
	{
		public IQueryable<Role> Roles
		{
			get
			{
				var query = Context.Roles;
				return query;
			}
		}

		/// <summary>
		/// Zapisywanie uprawnień do bazy danych
		/// </summary>
		/// <param name="role">Uprawnienia</param>
		/// <returns>Odpowiedź o stanie zapisu</returns>
		public ApiResponse Save(Role role)
		{
			var response = new ApiResponse();

			try
			{
				if (role.Id == 0)
				{
					Context.Roles.Add(role);
				}
				else
				{
					var dbEntry = Context.Roles.Find(role.Id);
					if (dbEntry != null)
					{
						dbEntry.Name = role.Name;
						dbEntry.Commission = role.Commission;
						dbEntry.RoleType = role.RoleType;
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

			var deletedObject = Context.Roles.Find(id);
			if (deletedObject != null)
			{
				try
				{
					Context.Roles.Remove(deletedObject);
					Context.SaveChanges();
				}
				catch (Exception)
				{
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
	}
}