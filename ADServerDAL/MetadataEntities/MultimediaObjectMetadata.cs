using ADServerDAL.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ADServerDAL
{
	/// <summary>
	/// Metadane obiektu multimedialnego
	/// </summary>
	public partial class MultimediaObjectMetadata
	{
		/// <summary>
		/// Identyfikator
		/// </summary>
		[HiddenInput(DisplayValue = false)]
		public int ID { get; set; }

		/// <summary>
		/// Walidacja nazwy obiektu
		/// </summary>
		[Required(ErrorMessage = "Pole {0} wymagane")]
		[Display(Name = "Nazwa")]
		[MultimediaObjectValidation]
		[StringLength(150, ErrorMessage = "Maksymalna dopuszczalna długość pola {0} wynosi {1}")]
		public string Name { get; set; }

		/// <summary>
		/// Identyfikator typu obiektu
		/// </summary>
		[Required(ErrorMessage = "Pole {0} wymagane")]
		[Display(Name = "Typ obiektu")]
		public int TypeId { get; set; }

		/// <summary>
		/// Nazwa pliku
		/// </summary>
		[Display(Name = "Nazwa pliku")]
		[StringLength(250, ErrorMessage = "Maksymalna dopuszczalna długość pola {0} wynosi {1}")]
		[ReadOnly(true)]
		public string FileName { get; set; }

		/// <summary>
		/// Typ mime pliku
		/// </summary>
		[Display(Name = "Mime")]
		[StringLength(100, ErrorMessage = "Maksymalna dopuszczalna długość pola {0} wynosi {1}")]
		[ReadOnly(true)]
		public string MimeType { get; set; }

		/// <summary>
		/// Użytkownik
		/// </summary>
		[Display(Name = "Użytkownik")]
		public int UserID { get; set; }

		/// <summary>
		/// Adres odnośnika obiektu
		/// </summary>
		[Display(Name = "Adres URL")]
		[Url]
		[StringLength(200, ErrorMessage = "Maksymalna dopuszczalna długość pola {0} wynosi {1}")]
		public string URL { get; set; }
	}
}