using System;
using System.ComponentModel.DataAnnotations;

namespace ADServerDAL.Filters
{
	/// <summary>
	/// Klasa filtrów typów obiektów
	/// </summary>
	public class MultimediaTypesListViewModelFilter : ViewModelFilterBase
	{
		#region Properties

		/// <summary>
		/// Szerokość
		/// </summary>
		[Display(Name = "Szerokość")]
		[Range(1, Int32.MaxValue)]
		public int? FilterWidth { get; set; }

		/// <summary>
		/// Wysokość
		/// </summary>
		[Display(Name = "Wysokość")]
		[Range(1, Int32.MaxValue)]
		public int? FilterHeight { get; set; }

		/// <summary>
		/// Czy wypełniono filtry
		/// </summary>
		public bool Filtering
		{
			get
			{
				return !string.IsNullOrEmpty(FilterName) ||
					  FilterWidth.HasValue ||
					  FilterHeight.HasValue;
			}
		}

		#endregion Properties

		#region Overrided methods

		public override bool Equals(object obj)
		{
			var other = obj as MultimediaTypesListViewModelFilter;
			if (other != null)
			{
				return other.FilterHeight == FilterHeight &&
				  other.FilterName == FilterName &&
				  other.FilterWidth == FilterWidth;
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