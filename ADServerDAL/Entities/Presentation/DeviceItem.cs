using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADServerDAL.Models;

namespace ADServerDAL.Entities.Presentation
{
	public class DeviceItem : PresentationItem
	{
		public int Id { get; set; }
		[Display(Name = "Nazwa nośnika")]
		public string Name { get; set; }
		[Display(Name = "Użytkownik")]
		public int UserId { get; set; }

		public User User { get; set; }
		public Statistic Statistics { get; set; }
		public ICollection<Campaign> Campaign { get; set; }
	}
}
