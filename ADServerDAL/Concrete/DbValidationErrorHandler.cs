using ADServerDAL.Entities.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADServerDAL.Concrete
{
    /// <summary>
    /// Klasa obsługująca wyjątki bazodanowe (Entity Framework)
    /// </summary>
    public class DbValidationErrorHandler : IDisposable
    {
        private List<ApiValidationErrorItem> validationErrors;

        /// <summary>
        /// Zapamiętana kolekcja błędów
        /// </summary>
        public List<ApiValidationErrorItem> ValidationErrors
        {
            get
            {
                return validationErrors;
            }
        }

        /// <summary>
        /// Sprawdzenie czy wykryto błędy bazodanowe
        /// </summary>
        public bool HasErrors
        {
            get { return validationErrors != null && validationErrors.Count > 0; }
        }

        /// <summary>
        /// Metoda pobierająca informacje o błędach bazodanowych
        /// </summary>
        /// <param name="entityException"></param>
        public DbValidationErrorHandler(System.Data.Entity.Validation.DbEntityValidationException entityException)
        {
            validationErrors = new List<ApiValidationErrorItem>();
            if (entityException.EntityValidationErrors.Count() > 0)
            {
                foreach (var v in entityException.EntityValidationErrors)
                {
                    if (v.ValidationErrors.Count > 0)
                    {
                        foreach (var vv in v.ValidationErrors)
                        {
                            validationErrors.Add(new ApiValidationErrorItem
                            {
                                Message = vv.ErrorMessage,
                                Property = vv.PropertyName
                            });
                        }
                    }
                }
            }
            else
            {
                validationErrors.Add(new ApiValidationErrorItem
                {
                    Message = entityException.Message
                });
            }
        }

        public void Dispose()
        {
            if (validationErrors != null)
            {
                validationErrors.Clear();
                validationErrors = null;
            }
        }

        /// <summary>
        /// Pobranie informacji o błędach bazodanowych na podstawie stanu modelu
        /// </summary>
        /// <param name="ex">Wyjątek</param>
        /// <param name="ModelState">Stan modelu</param>
        /// <param name="prefix">Prefiks określający nazwę właściwości modelu dla której sprawdzane są błędy</param>
        public static void ModelHandleException(Exception ex, System.Web.Mvc.ModelStateDictionary ModelState, string prefix)
        {
            if (ex is System.Data.Entity.Validation.DbEntityValidationException)
            {
                using (DbValidationErrorHandler dvValExp = new DbValidationErrorHandler(ex as System.Data.Entity.Validation.DbEntityValidationException))
                {
                    if (dvValExp.HasErrors)
                    {
                        foreach (var err in dvValExp.ValidationErrors)
                        {
                            ModelState.AddModelError(prefix + "." + err.Property, err.Message);
                        }
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", ex.Message);
            }
        }
    }
}