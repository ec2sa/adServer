using ADServerDAL.Abstract;
using ADServerDAL.Entities.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADServerDAL.Helpers;
using ADServerDAL.Models;

namespace ADServerDAL.Concrete
{
    /// <summary>
    /// Implementacja repozytorium kategorii
    /// </summary>
    public class EFCategoryRepository : EFBaseRepository, ICategoryRepository
    {

        /// <summary>
        /// Kolekcja wszystkich kategorii
        /// </summary>
        public IQueryable<Category> Categories
        {
            get
            {
	            var query = Context.Categories;
                return query;
            }
        }

        /// <summary>
        /// Zwraca identyfikatory kategorii powiązanych z daną kampanią
        /// </summary>
        /// <param name="campaignId">Identyfikator kampanii</param>   
        public List<int> CategoriesToCampaign(int campaignId)
        {
	        var cmp = Context.Campaigns.FirstOrDefault(it => it.Id == campaignId);
	        var query = cmp.Categories.Select(it=> it.Id);
            return query.ToList();
        }

        /// <summary>
        /// Zapisuje kategorię do bazy danych
        /// </summary>
        /// <param name="category">Obiekt kategorii do zapisu</param>    
        public ApiResponse Save(Category category)
        {
            var response = new ApiResponse();

            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    dynamic dbEntry;

                    if (category.Id > 0)
                    {
                        dbEntry = Context.Categories.FirstOrDefault(f => f.Id == category.Id);
                        if (dbEntry != null)
                        {
                            dbEntry.Name = category.Name;
							dbEntry.Code = category.Code;
							SetRelation(category, ref dbEntry);
                            Context.SaveChanges();
                        }
                    }
                    else
                    {
                        dbEntry = new Category
                        {
                            Name = category.Name,
                            Code = category.Code
                        };
						SetRelation(category, ref dbEntry);
                        Context.Categories.Add(dbEntry);
                        Context.SaveChanges();
                    }
					
                    transaction.Commit();
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
                    //  Obsługa błędów pozostałych
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

	    void SetRelation(Category category, ref dynamic dbEntry)
	    {
			ObjectRelationCampaign(category.Campaigns, ref dbEntry);
	    }

        /// <summary>
        /// Zwraca obiekt kategorii na podstawie zadanego identyfikatora
        /// </summary>
        /// <param name="id">Identyfikator kategorii</param> 
        public Category GetById(int id)
        {
            Category category = Context.Categories.FirstOrDefault(c => c.Id == id);
            return category;
        }

        /// <summary>
        /// Usuwa kategorię z bazy danych na podstawie zadanego identyfikatora
        /// </summary>
        /// <param name="id">Identyfikator kategorii</param> 
        public ApiResponse Delete(int id)
        {
            ApiResponse response = new ApiResponse();

            Category deletedObject = Context.Categories.Find(id);
            if (deletedObject != null)
            {
                try
                {
                    Context.Categories.Remove(deletedObject);
                    Context.SaveChanges();
                }
                catch (Exception)
                {
                    response.Errors.Add(new ApiValidationErrorItem
                    {
                        Message = "Nie można usunąć kategorii - upewnij się, że nie istnieją obiekty powiązane."
                    });
                }
            }
            else
            {
                response.Errors.Add(new ApiValidationErrorItem
                {
                    Message = "Kategoria została już wcześniej usunięta"
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
