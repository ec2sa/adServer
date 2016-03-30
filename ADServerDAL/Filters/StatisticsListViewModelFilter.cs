using System;
using System.ComponentModel.DataAnnotations;

namespace ADServerDAL.Filters
{
	/// <summary>
	/// Klasa filtrów statystyk
	/// </summary>
	public class StatisticsListViewModelFilter : ViewModelFilterBase
	{
		#region Properties

		/// <summary>
		/// Data żądania/odpowiedzi od
		/// </summary>
		[Display(Name = "Data od")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? FilterDateFrom { get; set; }

		/// <summary>
		/// Data żądania/odpowiedzi do
		/// </summary>
		[Display(Name = "Data do")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? FilterDateTo { get; set; }

		/// <summary>
		/// Identyfiktor obiektu
		/// </summary>
		[Display(Name = "Id obiektu")]
		[Range(1, Int32.MaxValue)]
		public int? FilterMultimediaObjectId { get; set; }

		/// <summary>
		/// Nazwa obiektu
		/// </summary>
		[Display(Name = "Nazwa obiektu")]
		public string FilterMultimediaObjectName { get; set; }

		/// <summary>
		/// Nazwa kampanii
		/// </summary>
		[Display(Name = "Kampania")]
		public string FilterCampaignName { get; set; }

		/// <summary>
		/// Adres IP użytkownika
		/// </summary>
		[Display(Name = "IP")]
		public string FilterRequestIP { get; set; }

		/// <summary>
		/// Nazwa kategorii
		/// </summary>
		[Display(Name = "Kategoria")]
		public string FilterCategoryName { get; set; }

		/// <summary>
		/// Nazwa nośnika aplikacji
		/// </summary>
		[Display(Name = "Nośnik")]
		public string FilterReferrerName { get; set; }

		/// <summary>
		/// Informacje dodatkowe
		/// </summary>
		[Display(Name = "Informacje dodatkowe")]
		public string FilterAdditionalInfoName { get; set; }

		/// <summary>
		/// Imię/nazwisko użytkownika
		/// </summary>
		[Display(Name = "Imię / nazwisko")]
		public string FilterClientName { get; set; }

		/// <summary>
		/// Pesel
		/// </summary>
		[Display(Name = "PESEL")]
		public string FilterPESEL { get; set; }

		/// <summary>
		/// E-mail
		/// </summary>
		[Display(Name = "E-mail")]
		public string FilterEmail { get; set; }

		/// <summary>
		/// Nazwa firmy
		/// </summary>
		[Display(Name = "Firma")]
		public string FilterCompanyName { get; set; }

		/// <summary>
		/// Inne informacje
		/// </summary>
		[Display(Name = "Inne")]
		public string FilterOther { get; set; }

		/// <summary>
		/// Czy wypełniono filtry
		/// </summary>
		public bool Filtering
		{
			get
			{
				return FilterDateFrom.HasValue ||
					   FilterDateTo.HasValue ||
					   FilterMultimediaObjectId.HasValue ||
					   (FilterMultimediaObjectName != null && FilterMultimediaObjectName.Length > 0) ||
					   (FilterCampaignName != null && FilterCampaignName.Length > 0) ||
					   (FilterRequestIP != null && FilterRequestIP.Length > 0) ||
					   (FilterCategoryName != null && FilterCategoryName.Length > 0) ||
					   (FilterReferrerName != null && FilterReferrerName.Length > 0) ||
					   (FilterAdditionalInfoName != null && FilterAdditionalInfoName.Length > 0) ||
					   (FilterClientName != null && FilterClientName.Length > 0) ||
					   (FilterPESEL != null && FilterPESEL.Length > 0) ||
					   (FilterEmail != null && FilterEmail.Length > 0) ||
					   (FilterCompanyName != null && FilterCompanyName.Length > 0) ||
					   (FilterOther != null && FilterOther.Length > 0);
			}
		}

		#endregion Properties

		#region Overrided methods

		public override bool Equals(object obj)
		{
			StatisticsListViewModelFilter other = obj as StatisticsListViewModelFilter;
			if (other != null)
			{
				return other.FilterAdditionalInfoName == this.FilterAdditionalInfoName &&
					other.FilterCampaignName == this.FilterCampaignName &&
					other.FilterCategoryName == this.FilterCategoryName &&
					other.FilterClientName == this.FilterClientName &&
					other.FilterCompanyName == this.FilterCompanyName &&
					other.FilterDateFrom == this.FilterDateFrom &&
					other.FilterDateTo == this.FilterDateTo &&
					other.FilterEmail == this.FilterEmail &&
					other.FilterMultimediaObjectId == this.FilterMultimediaObjectId &&
					other.FilterMultimediaObjectName == this.FilterMultimediaObjectName &&
					other.FilterOther == this.FilterOther &&
					other.FilterPESEL == this.FilterPESEL &&
					other.FilterReferrerName == this.FilterReferrerName;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#endregion Overrided methods
	}
}