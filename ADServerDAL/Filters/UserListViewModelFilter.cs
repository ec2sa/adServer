using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace ADServerDAL.Filters
{
	/// <summary>
	/// Klasa filtrów użytkowników
	/// </summary>
	public class UserListViewModelFilter : ViewModelFilterBase
	{
		#region Properties

		/// <summary>
		/// Nazwa
		/// </summary>
		[Display(Name = "Użytkownik")]
		public string FilterLogin { get; set; }

		/// <summary>
		/// Nazwa
		/// </summary>
		[Display(Name = "Imie")]
		public string FilterFirstName { get; set; }

		/// <summary>
		/// Nazwa
		/// </summary>
		[Display(Name = "Nazwisko")]
		public string FilterLastName { get; set; }

		/// <summary>
		/// Nazwa
		/// </summary>
		[Display(Name = "Zablokowany")]
		public bool? FilterBlocked { get; set; }

		/// <summary>
		/// Identyfikator roli
		/// </summary>
		[Display(Name = "Rola")]
		public int? FilterRolaId { get; set; }

		/// <summary>
		/// Punkty reklamowe
		/// </summary>
		[Display(Name = "AdPoints")]
		public decimal AdPoints { get; set; }

		/// <summary>
		/// List ról
		/// </summary>
		public SelectList FilterRole { get; set; }

		/// <summary>
		/// Słownik opcji TAK/NIE
		/// </summary>
		public SelectList YesNo { get; set; }
		
		/// <summary>
		/// Czy wypełniono filtry
		/// </summary>
		public bool Filtering
		{
			get
			{
				return !string.IsNullOrEmpty(FilterLogin) ||
					   !string.IsNullOrEmpty(FilterFirstName) ||
					   !string.IsNullOrEmpty(FilterLastName) ||
					   FilterBlocked.HasValue || FilterRolaId.HasValue;
			}
		}

		#endregion Properties

		#region Overrided methods

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// true if the specified object  is equal to the current object; otherwise, false.
		/// </returns>
		/// <param name="obj">The object to compare with the current object. </param><filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			var other = obj as UserListViewModelFilter;
			if (other != null)
			{
				return other.FilterLogin == this.FilterLogin &&
					other.FilterFirstName == this.FilterFirstName;
			}
			return false;
		}

		/// <summary>
		/// Serves as a hash function for a particular type. 
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#endregion Overrided methods
	}
}