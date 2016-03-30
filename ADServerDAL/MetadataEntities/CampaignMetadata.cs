using ADServerDAL.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ADServerDAL
{
	/// <summary>
	/// Metadane kampanii
	/// </summary>
	public partial class CampaignMetadata
	{
		/// <summary>
		/// Identyfikator
		/// </summary>
		[HiddenInput(DisplayValue = false)]
		public int ID { get; set; }

		/// <summary>
		/// Walidacja nazwy kampanii
		/// </summary>
		[Required(ErrorMessage = "Pole {0} wymagane")]
		[Display(Name = "Nazwa")]
		[StringLength(150, ErrorMessage = "Maksymalna dopuszczalna długość pola {0} wynosi {1}")]
		[CampaignValidationAttribute()]
		public string Name { get; set; }

		/// <summary>
		/// Opis
		/// </summary>
		[Display(Name = "Opis")]
		[StringLength(500, ErrorMessage = "Maksymalna dopuszczalna długość pola {0} wynosi {1}")]
		public string Description { get; set; }

		/// <summary>
		/// Walidacja datay rozpoczęcia
		/// </summary>
		[Display(Name = "Data rozpoczęcia")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		[CampaignValidationAttribute()]
		public DateTime StartDate { get; set; }

		/// <summary>
		/// Walidacja daty zakończenia
		/// </summary>
		[Display(Name = "Data zakończenia")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime EndDate { get; set; }

		/// <summary>
		/// Czy kampania aktywna
		/// </summary>
		[Display(Name = "Aktywna")]
		public bool IsActive { get; set; }

		/// <summary>
		/// Walidacja identyfikatora priorytetu
		/// </summary>
		[Required(ErrorMessage = "Pole {0} wymagane")]
		[Display(Name = "Priorytet")]
		public int PriorityId { get; set; }

		/// <summary>
		/// Użytkownik
		/// </summary>
		[Display(Name = "Użytkownik")]
		public int UserID { get; set; }

		[Display(Name = "AdPoints")]
		[Range(0, 999999.999999)]
		public decimal Points { get; set; }
		[Display(Name = "Punkty za wyświetlenie")]
		[Range(0, 9999.9999)]
		public decimal ViewValue { get; set; }

		[Display(Name = "Punkty za przekierowanie")]
		[Range(0, 9999.9999)]
		public decimal ClickValue { get; set; }
	}
}