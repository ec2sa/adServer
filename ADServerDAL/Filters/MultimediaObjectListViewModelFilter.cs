using System.ComponentModel.DataAnnotations;

namespace ADServerDAL.Filters
{
	/// <summary>
	/// Klasa filtrów obiektów multimedialnych
	/// </summary>
	public class MultimediaObjectListViewModelFilter : ViewModelFilterBase
	{
		#region Properties
		
		/// <summary>
		/// Nazwa pliku
		/// </summary>
		[Display(Name = "Nazwa pliku")]
		public string FilterFileName { get; set; }

		/// <summary>
		/// Identyfikator obiektu
		/// </summary>
		[Display(Name = "Id")]
		public int? FilterId { get; set; }

		/// <summary>
		/// Nazwa typu obiektu
		/// </summary>
		[Display(Name = "Typ")]
		public string FilterType { get; set; }

		/// <summary>
		/// Typ mime obiektu
		/// </summary>
		[Display(Name = "Mime")]
		public string FilterMime { get; set; }

		/// <summary>
		/// Login uzytkownika
		/// </summary>
		[Display(Name = "Użytkownik")]
		public string FilterLogin { get; set; }

		/// <summary>
		/// Czy wypełniono filtry
		/// </summary>
		public bool Filtering
		{
			get
			{
				return !string.IsNullOrEmpty(FilterName) ||
					   !string.IsNullOrEmpty(FilterFileName) ||
					   !string.IsNullOrEmpty(FilterType) ||
					   !string.IsNullOrEmpty(FilterMime);
			}
		}

		#endregion Properties

		#region Overrided methods

		public override bool Equals(object obj)
		{
			var other = obj as MultimediaObjectListViewModelFilter;
			if (other != null)
			{
				return other.FilterFileName == FilterFileName &&
					other.FilterId == FilterId &&
					other.FilterMime == FilterMime &&
					other.FilterName == FilterName &&
					other.FilterType == FilterType;
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