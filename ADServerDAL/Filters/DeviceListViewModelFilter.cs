using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace ADServerDAL.Filters
{
	/// <summary>
	/// Klasa filtrów użytkowników
	/// </summary>
	public class DeviceListViewModelFilter : ViewModelFilterBase
	{
		#region Properties
		/// <summary>
		/// Czy wypełniono filtry
		/// </summary>
		public bool Filtering
		{
			get { return !string.IsNullOrEmpty(FilterName); }
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
			var other = obj as DeviceListViewModelFilter;
			if (other != null)
			{
				return other.FilterName == this.FilterName;
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