using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADServerDAL.Models.Interface
{
	public interface ICampaigns
	{
		ICollection<Campaign> Campaigns { get; set; }
	}
}
