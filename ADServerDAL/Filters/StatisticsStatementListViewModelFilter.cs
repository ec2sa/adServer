using System;
using System.ComponentModel.DataAnnotations;

namespace ADServerDAL.Filters
{
	/// <summary>
	/// Klasa filtrów zestawień
	/// </summary>
	public class StatisticsStatementListViewModelFilter : ViewModelFilterBase
	{
		#region Properties

		/// <summary>
		/// Data od zestawienia
		/// </summary>
		[Display(Name = "Data żądania od")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? FilterDateFrom { get; set; }

		/// <summary>
		/// Data do zestawienia
		/// </summary>
		[Display(Name = "Data żądania do")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? FilterDateTo { get; set; }

		/// <summary>
		/// Czy wypełniono filtry
		/// </summary>
		public bool Filtering
		{
			get
			{
				return FilterDateFrom.HasValue ||
					   FilterDateTo.HasValue;
			}
		}

		#endregion Properties

		#region Constructors

		public StatisticsStatementListViewModelFilter()
		{
			var now = DateTime.Now;
			FilterDateFrom = new DateTime(now.Year, now.Month, 1);
			FilterDateTo = now;
		}

		#endregion Constructors

		#region Overrided methods

		public override bool Equals(object obj)
		{
			StatisticsStatementListViewModelFilter other = obj as StatisticsStatementListViewModelFilter;
			if (other != null)
			{
				return other.FilterDateFrom == this.FilterDateFrom &&
					other.FilterDateTo == this.FilterDateTo;
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