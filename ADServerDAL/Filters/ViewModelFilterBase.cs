using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADServerDAL.Filters
{
	public abstract class ViewModelFilterBase
	{
		/// <summary>
		/// Nazwa
		/// </summary>
		[Display(Name = "Nazwa")]
		public string FilterName { get; set; }
	}
}
