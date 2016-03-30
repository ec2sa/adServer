using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ADServerDAL.Models.Base;

namespace ADServerDAL.Models
{
    public class User : Entity
	{
		#region Description
		public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AdditionalInfo { get; set; }
		public bool IsBlocked { get; set; }
		[Range(0, 99999.99999)]
		[DataType("DECIMAL(5,4)")]
		public Decimal AdPoints { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string Url { get; set; }
		#endregion

		#region Foreign Key
		[ForeignKey("Role")]
		public int RoleId { get; set; }
		#endregion

		#region Foreign Key Value
		public virtual Role Role { get; set; }
		#endregion

		#region Collections
		public virtual ICollection<Campaign> Campaigns { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
        public virtual ICollection<MultimediaObject> MultimediaObjects { get; set; }
        public virtual ICollection<Statistic> Statistics { get; set; }
		#endregion
	}
}
