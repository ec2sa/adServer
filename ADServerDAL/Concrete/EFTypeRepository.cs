using ADServerDAL.Abstract;
using ADServerDAL.Entities;
using ADServerDAL.Entities.Presentation;
using ADServerDAL.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADServerDAL.Helpers;

namespace ADServerDAL.Concrete
{
    /// <summary>
    /// Implementacja repozytorium typów multimedialnych
    /// </summary>
    public class EFTypeRepository : EFBaseRepository, ITypeRepository
    {
        /// <summary>
        /// Kolekcja dostępnych typów
        /// </summary>
        public IQueryable<Models.Type> Types
        {
            get
            {
	            var query = Context.Types;
                return query;
            }
        }

        /// <summary>
        /// Pobiera prezenter typ na podstawie zadanego identyfikatora
        /// </summary>
        /// <param name="id">Identyfikator typu</param>
        public MultimediaTypeItem GetPresenterById(int id)
        {
            var type = Context.Types.FirstOrDefault(t => t.Id == id);
            if (type != null)
            {
                return new MultimediaTypeItem
                {
                    ID = type.Id,
                    Name = type.Name,
                    Width = type.Width,
                    Height = type.Height
                };
            }
            return null;
        }

        /// <summary>
        /// Pobiera obiekt typu na podstawie zadanego identyfikatora
        /// </summary>
        /// <param name="id">Identyfikator typu</param>   
        public Models.Type GetById(int id)
        {
			var type = Context.Types.FirstOrDefault(c => c.Id == id);
            return type;
        }

        /// <summary>
        /// Zapisywanie typu do bazy danych
        /// </summary>
        /// <param name="type">Obiekt typu</param>  
        public ApiResponse Save(Models.Type type)
        {
            var response = new ApiResponse();

            try
            {
                if (type.Id == 0)
                {
                    Context.Types.Add(type);
                }
                else
                {
                    var dbEntry = Context.Types.Find(type.Id);
                    if (dbEntry != null)
                    {
                        dbEntry.Name = type.Name;
                        dbEntry.Height = type.Height;
                        dbEntry.Width = type.Width;
                    }
                }

                Context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                //  Obsługa błędów EF
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
        /// Usuwanie typu z bazy danych na podstawie zadanego identyfikatora
        /// </summary>
        /// <param name="id">Identyfikator typu</param>
        public ApiResponse Delete(int id)
        {
            var response = new ApiResponse();
            var deletedObject = Context.Types.Find(id);

            if (deletedObject != null)
            {
                try
                {
                    Context.Types.Remove(deletedObject);
                    Context.SaveChanges();
                }
                catch (Exception)
                {
                    response.Errors.Add(new ApiValidationErrorItem
                    {
                        Message = "Nie można usunąć typu - upewnij się, że nie istnieją obiekty powiązane."
                    });
                }
            }
            else
            {
                response.Errors.Add(new ApiValidationErrorItem
                {
                    Message = "Typ został już wcześniej usunięty"
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
            base.SetNewContext(context);
        }
    }
}
