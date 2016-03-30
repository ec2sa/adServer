using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ADServerDAL;
using ADServerDAL.Models;

namespace ADServerManagementWebApplication.Models
{
	public class DeviceViewModel : Device
	{
		public IEnumerable<int> Campaign { get; set; }
	}
}