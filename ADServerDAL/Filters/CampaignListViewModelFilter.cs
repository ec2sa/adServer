using ADServerDAL.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace ADServerDAL.Filters
{
	/// <summary>
	/// Klasa filtrów kampanii
	/// </summary>
	public class CampaignListViewModelFilter : ViewModelFilterBase
	{
		#region Properties

		/// <summary>
		/// Czy aktywna
		/// </summary>
		[Display(Name = "Aktywna")]
		public bool? FilterActive { get; set; }

		/// <summary>
		/// Data rozpoczęcia od
		/// </summary>
		[Display(Name = "Data rozpoczęcia od")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true, NullDisplayText = "Wprowadź datę")]
		public DateTime? FilterStartDateFrom { get; set; }

		/// <summary>
		/// Data rozpoczęcia do
		/// </summary>
		[Display(Name = "Data rozpoczęcia do")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? FilterStartDateTo { get; set; }

		/// <summary>
		/// Data zakończenia od
		/// </summary>
		[Display(Name = "Data zakończenia od")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? FilterEndDateFrom { get; set; }

		/// <summary>
		/// Data zakończenia do
		/// </summary>
		[Display(Name = "Data zakończenia do")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? FilterEndDateTo { get; set; }

		/// <summary>
		/// Identyfikator priorytetu
		/// </summary>
		[Display(Name = "Priorytet")]
		public int? FilterPriorityId { get; set; }
		
		/// <summary>
		/// Login uzytkownika
		/// </summary>
		[Display(Name = "Użytkownik")]
		public string FilterLogin { get; set; }

		/// <summary>
		/// Lista priorytetów
		/// </summary>
		public System.Web.Mvc.SelectList Priorities { get; set; }

		/// <summary>
		/// Słownik opcji TAK/NIE
		/// </summary>
		public System.Web.Mvc.SelectList YesNo { get; set; }

		/// <summary>
		/// Czy wypełniono filtry
		/// </summary>
		public bool Filtering
		{
			get
			{
				return FilterActive.HasValue ||
					   FilterStartDateFrom.HasValue || FilterStartDateTo.HasValue ||
					   FilterEndDateFrom.HasValue || FilterEndDateTo.HasValue ||
					   (FilterPriorityId.HasValue && FilterPriorityId.Value != SelectListExt.EmptyOptionValue) ||
					   !string.IsNullOrEmpty(FilterName);
			}
		}

		#endregion Properties

		#region Overrided methods

		public override bool Equals(object obj)
		{
			var other = obj as CampaignListViewModelFilter;
			if (other != null)
			{
				return other.FilterActive == this.FilterActive &&
					other.FilterEndDateFrom == this.FilterEndDateFrom &&
					other.FilterEndDateTo == this.FilterEndDateTo &&
					other.FilterName == this.FilterName &&
					other.FilterPriorityId == this.FilterPriorityId &&
					other.FilterStartDateFrom == this.FilterStartDateFrom &&
					other.FilterStartDateTo == this.FilterStartDateTo;
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