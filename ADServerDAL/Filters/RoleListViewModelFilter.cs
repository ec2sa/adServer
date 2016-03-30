using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADServerDAL.Filters
{
	public class RoleListViewModelFilter : ViewModelFilterBase
	{
		[Display(Name = "Prowizja za usługę")]
		public int? Com { get; set; }

		public bool Filtering
		{
			get { return !string.IsNullOrEmpty(FilterName)||
				Com.HasValue; }
		}
	}
}
