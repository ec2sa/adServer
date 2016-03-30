using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace ADServerDAL.Models.Base
{
	public abstract class UserBase : Entity
	{
		[HiddenInput(DisplayValue = false)]
		[Display(Name = "Użytkownik")]
		[ForeignKey("User")]
		public int? UserId { get; set; }

		public virtual User User { get; set; }
	}
}
