using System.ComponentModel.DataAnnotations;

namespace ADServerDAL.Filters
{
	/// <summary>
	/// Klasa filtrów priorytetów
	/// </summary>
	public class CampaignPrioritiesListViewModelFilter : ViewModelFilterBase
	{
		#region Properties

		/// <summary>
		/// Kod
		/// </summary>
		[Display(Name = "Kod")]
		public int? FilterCode { get; set; }

		/// <summary>
		/// Czy wypełniono filtry
		/// </summary>
		public bool Filtering
		{
			get
			{
				return !string.IsNullOrEmpty(FilterName) ||
					   (FilterCode.HasValue);
			}
		}

		#endregion Properties

		#region Overrided methods

		public override bool Equals(object obj)
		{
			var other = obj as CampaignPrioritiesListViewModelFilter;
			if (other != null)
			{
				return other.FilterCode == this.FilterCode &&
					other.FilterName == this.FilterName;
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