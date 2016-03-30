using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ADServerDAL.Models.Base;

namespace ADServerDAL.Models
{
    public partial class Type : Entity
	{
		#region Description

		[Required(ErrorMessage = "Pole {0} wymagane")]
		[Display(Name = "Szerokoœæ")]
		[Range(1, int.MaxValue, ErrorMessage = "Niepoprawna wartoœæ pola {0}")]
		public int Width { get; set; }

		[Required(ErrorMessage = "Pole {0} wymagane")]
		[Display(Name = "Wysokoœæ")]
		[Range(1, int.MaxValue, ErrorMessage = "Niepoprawna wartoœæ pola {0}")]
        public int Height { get; set; }
		#endregion

		#region Collections
		public virtual ICollection<MultimediaObject> MultimediaObjects { get; set; }
		public virtual ICollection<Device> Devices { get; set; } 
		#endregion
	}
}
