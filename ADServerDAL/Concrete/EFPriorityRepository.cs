using ADServerDAL.Abstract;
using ADServerDAL.Entities.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using ADServerDAL.Helpers;
using ADServerDAL.Models;

namespace ADServerDAL.Concrete
{
    /// <summary>
    /// Implementacja repozytorium priorytetów kampanii
    /// </summary>
    public class EFPriorityRepository : EFBaseRepository, IPriorityRepository
    {

        /// <summary>
        /// Kolekcja wszystkich priorytetów
        /// </summary>
        public IQueryable<Priority> Priorities
        {
            get
            {
	            var query = Context.Priorities;
                return query;
            }
        }

        /// <summary>
        /// Pobiera encję priorytetu wg podanego identyfikatora
        /// </summary>
        /// <param name="id">Identyfikator priorytetu</param>   
        public Priority GetById(int id)
        {
            Priority priority = Context.Priorities.FirstOrDefault(c => c.Id == id);
            return priority;
        }

        /// <summary>
        /// Usuwa wskazany priorytet z bazy danych
        /// </summary>
        /// <param name="id">Identyfikator priorytetu</param>
        public ApiResponse Delete(int id)
        {
            var response = new ApiResponse();

            var deletedObject = Context.Priorities.Find(id);
            if (deletedObject != null)
            {
                try
                {
                    Context.Priorities.Remove(deletedObject);
                    Context.SaveChanges();
                }
                catch (Exception)
                {
                    response.Errors.Add(new ApiValidationErrorItem
                    {
                        Message = "Nie można usunąć priorytetu - upewnij się, że nie istnieją obiekty powiązane."
                    });
                }
            }
            else
            {
                response.Errors.Add(new ApiValidationErrorItem
                {
                    Message = "Priorytet został już wcześniej usunięty"
                });
            }

            response.Accepted = response.Errors.Count == 0;
            return response;
        }

        /// <summary>
        /// Zapisuje dany priorytet do bazy danych
        /// </summary>
        /// <param name="priority">Obiekt priorytetu</param>
        public ApiResponse Save(Priority priority)
        {
            var response = new ApiResponse();

            try
            {
                if (priority.Id == 0)
                {
                    Context.Priorities.Add(priority);
                }
                else
                {
                    var dbEntry = Context.Priorities.Find(priority.Id);
                    if (dbEntry != null)
                    {
                        dbEntry.Code = priority.Code;
                        dbEntry.Name = priority.Name;
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
        /// Możliwość ustawienia innego kontekstu niż wbudowany
        /// </summary>
        /// <param name="context">Nowy kontekst EF</param>
        public void SetContext(System.Data.Entity.DbContext context)
        {
            base.SetNewContext(context);
        }
    }
}
