using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using ADServerDAL.Models.Base;
using ADServerDAL.Models.Interface;
using ADServerDAL.Validation;

namespace ADServerDAL.Models
{
	public class Category : Entity, ICampaigns, IStatistics
	{
		public Category()
		{
			Campaigns = new Collection<Campaign>();
			Statistics = new Collection<Statistic>();
		}
		#region Description

		[Required]
		[Display(Name = "Kod")]
		[CategoryValidation]
		[StringLength(50, ErrorMessage = "Maksymalna dopuszczalna d³ugoœæ pola {0} wynosi {1}")]
		public string Code { get; set; }

		#endregion Description

		#region Collections

		public virtual ICollection<Campaign> Campaigns { get; set; }

		public virtual ICollection<Statistic> Statistics { get; set; }

		public virtual ICollection<Device> Devices { get; set; }

		#endregion Collections
	}
}