using ADServerDAL.Abstract;
using ADServerDAL.Helpers;
using ADServerManagementWebApplication.Infrastructure;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;

namespace ADServerManagementWebApplication.Controllers
{
    /// <summary>
    /// API kontroler do przesyłania plików
    /// </summary>
    public class UploadController : AdServerBaseApiController
    {
        #region - Fields -
        /// <summary>
        /// Repozytorium typów obiektów
        /// </summary>
        private ITypeRepository typeRepository; 
        #endregion

        #region - Constructors -
        public UploadController(ITypeRepository typeRepository)
        {
            this.typeRepository = typeRepository;
        } 
        #endregion

        #region - Overriden methods -
        /// <summary>
        /// Zwalnianie zasobów
        /// </summary>
        protected override void OnDisposeController()
        {
            if (typeRepository != null)
            {
                typeRepository.Dispose();
                typeRepository = null;
            }
        } 
        #endregion

        #region - Public methods -

        /// <summary>
        /// Metoda ładująca plik na serwer
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Upload()
        {
            var errors = "";
            object result = null;
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            try
            {
                ///Sprawdź czy w żądaniu znajduje się informacja o przesyłanym pliku
                if (HttpContext.Current.Request.Files != null && HttpContext.Current.Request.Files.Count > 0 &&
                    HttpContext.Current.Request.Params["MultimediaObjectTypeId"] != null)
                {
                    ///Pobierz pierwszy znaleziony plik
                    HttpPostedFile file = HttpContext.Current.Request.Files[0];

                    if (file.ContentType.ToUpper().StartsWith("IMAGE/"))
                    {
                        if (file.ContentLength > 0)
                        {
                            ///na formularzu, z którego przychodzi request muszą być pola o dokładnie takich nazwach:
                            var typeIdString = HttpContext.Current.Request.Params["MultimediaObjectTypeId"].ToString();
                            int typeId = 0;
                            int.TryParse(typeIdString, out typeId);
                            serializer.MaxJsonLength = GetJsonMaxLength();
                            byte[] contents = null;

                            ///Utwórz tablicę bajtów dla przesyłanego pliku
                            using (var memoryStream = new MemoryStream())
                            {
                                file.InputStream.CopyTo(memoryStream);
                                contents = memoryStream.ToArray();
                            }

                            ///Pobierz informacje o typie obiektu multimedialnego
                            ADServerDAL.Models.Type objType = typeRepository.GetById(typeId);
                            if (objType == null)
                            {
                                throw new Exception("Nie można odnaleść żadanego typu obiektu. Możliwe zmiany na innym stanowisku.");
                            }

                            ///Odczytaj docelowe wymiary obrazu
                            var width = objType.Width;
                            var height = objType.Height;

                            try
                            {
                                ///Zmień rozmiar przesłanego obrazu na podstawie wymiarów typu obiektu
                                contents = ImageProcesorHelper.ResizeImage(width, height, contents, false).ResizedImage;
                            }
                            catch (Exception)
                            {
                                errors += "Plik jest uszkodzony lub ma niewłaściwy typ. ";
                                throw;
                            }

                            HttpContext.Current.Response.ContentType = "text/plain";

                            ///Sprawdź czy plik po zmianie rozmiaru ma dopuszczalny rozmiar
                            if (contents.Length <= serializer.MaxJsonLength)
                            {
                                result = new
                                {
                                    name = file.FileName,
                                    contents = Convert.ToBase64String(contents),
                                    mimeType = file.ContentType,
                                    width = width,
                                    height = height
                                };
                            }
                            else
                            {
                                errors += string.Format("Plik jest zbyt duży. Maksymalny rozmiar pliku to {0}B. ", serializer.MaxJsonLength);
                            }
                        }
                    }
                    else
                    {
                        errors += string.Format("Plik ma nieprawidłowy format ({0})", file.ContentType);
                    }
                }
                else
                {
                    errors += "Brak pliku lub brak parametru MultimediaObjectTypeId. ";
                }
            }
            catch (Exception ex)
            {
                errors += string.Format("Komunikat błędu: {0} (Źródło: {1})", ex.Message, ex.Source); ;
            }

            if (errors != null && errors.Length > 0)
            {
                result = new { errors = errors };
            }

            ///Zapisz zmodyfikowany plik jako JSON i zwróć do klienta
            HttpContext.Current.Response.Write(serializer.Serialize(result));
            HttpContext.Current.Response.StatusCode = 200;
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// Zwraca maksymalną wielkość obiektu JSON w bajtach
        /// </summary>
        public static int GetJsonMaxLength()
        {
            var result = 2097152;
            try
            {
                ///Próba odczytania parametru z pliku konfiguracyjnego
                var conf = WebConfigurationManager.OpenWebConfiguration("~");
                ScriptingJsonSerializationSection jsonSection = conf.GetSection("system.web.extensions/scripting/webServices/jsonSerialization") as ScriptingJsonSerializationSection;
                result = jsonSection.MaxJsonLength;
            }
            catch (Exception)
            {
            }
            return result;
        }
        #endregion
    }
}
