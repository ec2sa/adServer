using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ADServerDAL.Entities.Presentation;

namespace ADServerManagementWebApplication.Models
{
	public class RoleViewModel
	{
		public int ID { get; set; }

		/// <summary>
		/// Nazwa roli
		/// </summary>
		[Display(Name = "Nazwa roli")]
		public string Name { get; set; }

		/// <summary>
		/// Pobierana prowizja za usługę
		/// </summary>
		[Display(Name = "Prowizja za usługę")]
		[Range(0, 100)]
		public short Commission { get; set; }

		/// <summary>
		/// Uprawnienia
		/// </summary>
		public IEnumerable<SelectListItem> Roles { get; set; }

		/// <summary>
		/// Uprawnienia roli
		/// </summary>
		[Display(Name = "Poziom uprawnień")]
		public string Type { get; set; }
	}
}