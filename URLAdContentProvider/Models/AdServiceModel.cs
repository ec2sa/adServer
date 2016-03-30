using URLAdContentProvider.WebServiceADContentProvider;

namespace URLAdContentProvider.Models
{
    public class AdServiceModel
    {
        private string _categoryCoodesAsString;

        #region - Properties -
        /// <summary>
        /// Adres URL webserwisu, z którego mają być pobierane reklamy/obiekty multimedialne
        /// </summary>
        public string ServiceUrl { get; set; }

        /// <summary>
        /// Nazwa webserwisowej metody, która zwraca obiekty multimedialne
        /// </summary>
        public string ServiceMethod { get; set; }

        /// <summary>
        /// Zwraca pełny adres webserwisu (włącznie z nazwą metody), z którego pobierane mają być reklamy/obiekty multimedialne
        /// </summary>
        public string ServiceFullAddress
        {
            get
            {
                return ServiceUrl + "/" + ServiceMethod;
            }
        }

        /// <summary>
        /// Request, który ma być parametrem webserwisowej metody, któa zwraca obiekty multimedialne
        /// </summary>
        public GetMultimediaObject_Request Request { get; set; }

        /// <summary>
        /// Lista oddzielonych przecinkami kodów kategorii
        /// </summary>
        public string CategoryCodesAsString
        {
            get
            {
	            if (_categoryCoodesAsString != null) 
					return _categoryCoodesAsString;
				//if (Request != null && Request.CategoryCodes != null)
				//{
				//	_categoryCoodesAsString = string.Join(",", Request.CategoryCodes);
				//}

	            return _categoryCoodesAsString;
            }

            set
            {
                _categoryCoodesAsString = value;
            }
        }

        /// <summary>
        /// Jeśli pole != null, oznacza to, że wystąpiły jakieś błędy i nie należy uruchamiać sekwencyjnego pokazu reklamS
        /// </summary>
        public string Errors { get; set; }
        #endregion
    }
}