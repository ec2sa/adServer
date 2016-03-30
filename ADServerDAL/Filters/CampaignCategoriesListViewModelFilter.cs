using System.ComponentModel.DataAnnotations;

namespace ADServerDAL.Filters
{
	/// <summary>
	/// Filtry listy kategorii
	/// </summary>
	public class CampaignCategoriesListViewModelFilter : ViewModelFilterBase
	{
		/// <summary>
		/// Kod kategorii
		/// </summary>
		[Display(Name = "Kod")]
		public string FilterCode { get; set; }

		/// <summary>
		/// Czy wypełniono filtry
		/// </summary>
		public bool Filtering
		{
			get
			{
				return !string.IsNullOrEmpty(FilterName) ||
					   !string.IsNullOrEmpty(FilterCode);
			}
		}

		#region Overrided methods

		public override bool Equals(object obj)
		{
			var other = obj as CampaignCategoriesListViewModelFilter;
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