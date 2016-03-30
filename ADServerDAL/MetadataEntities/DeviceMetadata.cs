using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ADServerDAL.Validation;

namespace ADServerDAL.MetadataEntities
{
	class DeviceMetadata
	{

		/// <summary>
		/// Identyfikator
		/// </summary>
		[HiddenInput(DisplayValue = false)]
		public int Id { get; set; }

		[Required(ErrorMessage = "Pole {0} jest wymagane")]
		[Display(Name = "Nazwa")]
		[StringLength(50, ErrorMessage = "Maksymalna dopuszczalna długość pola {0} wynosi {1}")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Pole {0} jest wymagane")]
		[Display(Name = "Opis")]
		[StringLength(560, ErrorMessage = "Maksymalna dopuszczalna długość pola {0} wynosi {1}")]
		public string Description { get; set; }
		[HiddenInput(DisplayValue = false)]
		public int UserID { get; set; }
	}
}
