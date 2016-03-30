using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ADServerDAL;
using ADServerDAL.Entities.Presentation;
using ADServerDAL.Filters;
using ADServerDAL.Models;
using ADServerDAL.Models.Base;

namespace ADServerManagementWebApplication.Models
{
	public class DeviceListViewModel : ListViewModel
	{
		public IQueryable<Entity> Devices { get { return Query; } }

		/// <summary>
		/// Filtry
		/// </summary>
		public DeviceListViewModelFilter Filters { get; set; }
	}
}