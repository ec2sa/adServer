using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ADServerDAL.Models.Base;

namespace ADServerDAL.Models
{
	public class Device : UserBase
	{
		public Device()
		{
			Campaigns = new List<Campaign>();
			Statistics = new Collection<Statistic>();
			Categories = new Collection<Category>();
		}

		#region Description

		[Required(ErrorMessage = "Pole {0} jest wymagane")]
		[Display(Name = "Opis")]
		[StringLength(50, ErrorMessage = "Maksymalna dopuszczalna d³ugoœæ pola {0} wynosi {1}")]
		public string Description { get; set; }

		#endregion Description

		#region Foreign Key
		[Display(Name = "Typ powi¹zanych obiektów multimedialnych")]
		[ForeignKey("Type")]
		public int TypeId { get; set; }
		#endregion

		#region Foreign key values
		public virtual Type Type { get; set; }
		#endregion


		#region Collections

		public virtual ICollection<Statistic> Statistics { get; set; }

		public virtual ICollection<Campaign> Campaigns { get; set; }

		public virtual ICollection<Category> Categories { get; set; }

		#endregion Collections
	}
}