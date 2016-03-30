using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace ADServerDAL.Models.Base
{
	public abstract class Entity
	{
		[HiddenInput(DisplayValue = false)]
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		[Display(Name = "Nazwa")]
		[StringLength(150, ErrorMessage = "Maksymalna dopuszczalna długość pola {0} wynosi {1}")]
		[MaxLength(150)]
		public string Name { get; set; }

		#region Overrided
		public override string ToString()
		{
			return Name;
		}

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// true if the specified object  is equal to the current object; otherwise, false.
		/// </returns>
		/// <param name="obj">The object to compare with the current object. </param><filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			var cmp = obj as Entity;

			if (cmp == null) 
				return false;

			if (cmp.Id != 0 && Id != 0)
			{
				return cmp.Id == Id;
			}
			if (obj is Priority)
				return ((Priority) obj).Code == ((Priority) this).Code;
			return cmp.Name == Name;
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
		#endregion
	}
}