using System;
using System.ComponentModel.DataAnnotations.Schema;
using ADServerDAL.Models.Base;

namespace ADServerDAL.Models
{
    public class Statistic : UserBase
	{
		#region Description
		public DateTime RequestDate { get; set; }
		public DateTime ResponseDate { get; set; }
		public string RequestIP { get; set; }
		public string Data1 { get; set; }
		public string Data2 { get; set; }
		public string Data3 { get; set; }
	    public string Data4 { get; set; }
		public string SessionId { get; set; }
		public int RequestSource { get; set; }
		public bool Clicked { get; set; }
		public decimal AdPoints { get; set; }
		#endregion

		#region Foreign Key
		[ForeignKey("MultimediaObject")]
		public int MultimediaObjectId { get; set; }
		[ForeignKey("Device")]
		public int? DeviceId { get; set; }
		[ForeignKey("Campaign")]
		public int? CampaignId { get; set; }
		#endregion

		#region Foreign Key Value
		public virtual Device Device { get; set; }
        public virtual MultimediaObject MultimediaObject { get; set; }
		public virtual Campaign Campaign { get; set; }
		#endregion

		/// <summary>
		/// Definicja rodzaju Ÿród³a ¿¹dania
		/// </summary>
		public enum RequestSourceType
		{
			/// <summary>
			/// Aplikacje WWW
			/// </summary>            
			WWW = 0,

			/// <summary>
			/// Aplikacje desktop
			/// </summary>
			Desktop = 1
		}
	}
}
