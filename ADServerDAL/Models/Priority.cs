using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ADServerDAL.Models.Base;
using ADServerDAL.Validation;

namespace ADServerDAL.Models
{
    public partial class Priority : Entity
    {
		#region Description
		[Required(ErrorMessage = "Pole {0} wymagane")]
		[Display(Name = "Kod")]
		[PriorityValidation]
		[Range(0, int.MaxValue, ErrorMessage = "Niepoprawna wartoœæ pola {0}")]
		public int Code { get; set; }
		#endregion

		#region Collections
		public virtual ICollection<Campaign> Campaigns { get; set; }
		#endregion
	}
}
