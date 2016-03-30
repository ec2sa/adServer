using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Runtime.Serialization;
using ADServerDAL.Models.Base;
using ADServerDAL.Models.Interface;
using ADServerDAL.Validation;

namespace ADServerDAL.Models
{
	public class Campaign : UserBase, ICategories, IMMObjects, IStatistics, IDevices
	{
		public Campaign()
		{
			Devices = new Collection<Device>();
			Categories = new Collection<Category>();
			MultimediaObjects = new Collection<MultimediaObject>();
			Statistics = new Collection<Statistic>();
			DeletedDevices = new Collection<DeletedDevices>();
		}

		#region Descriptions
		/// <summary>
		/// Opis
		/// </summary>
		[Display(Name = "Opis")]
		[StringLength(500, ErrorMessage = "Maksymalna dopuszczalna d³ugoœæ pola {0} wynosi {1}")]
		public string Description { get; set; }

		/// <summary>
		/// Data rozpoczêcia kampanii
		/// </summary>
		[Display(Name = "Data rozpoczêcia")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		[CampaignValidationAttribute]
		public System.DateTime StartDate { get; set; }

		/// <summary>
		/// Data zakoñczenia kampanii
		/// </summary>
		[Display(Name = "Data zakoñczenia")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public System.DateTime EndDate { get; set; }

		/// <summary>
		/// Czy kampania aktywna
		/// </summary>
		[Display(Name = "Aktywna")]
		public bool IsActive { get; set; }

		[Display(Name = "AdPoints")]
		[Range(0, 999999.999999)]
		public decimal AdPoints { get; set; }

		[Display(Name = "Punkty za wyœwietlenie")]
		[Range(0, 9999.9999)]
		public decimal ViewValue { get; set; }

		[Display(Name = "Punkty za przekierowanie")]
		[Range(0, 9999.9999)]
		public decimal ClickValue { get; set; }

		#endregion Descriptions

		#region Foreign Key

		/// <summary>
		/// Priorytet
		/// </summary>
		[DataMember(IsRequired=true)] 
		//[Required(ErrorMessage = "Pole {0} wymagane")]
		[Display(Name = "Priorytet")]
		[ForeignKey("Priority")]
		public int PriorityId { get; set; }

		#endregion ForeignKey

		#region Virtual Key Value

		public virtual Priority Priority { get; set; }

		#endregion Virtual Key Value

		#region Collections

		public virtual ICollection<Category> Categories { get; set; }

		public virtual ICollection<MultimediaObject> MultimediaObjects { get; set; }

		public virtual ICollection<Statistic> Statistics { get; set; }

		public virtual ICollection<Device> Devices { get; set; }

		public virtual ICollection<DeletedDevices> DeletedDevices { get; set; }

		#endregion Collections
	}
}