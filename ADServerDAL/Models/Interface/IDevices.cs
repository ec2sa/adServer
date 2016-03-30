using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADServerDAL.Models.Interface
{
	public interface IDevices
	{
		ICollection<Device> Devices { get; set; }
	}
}
