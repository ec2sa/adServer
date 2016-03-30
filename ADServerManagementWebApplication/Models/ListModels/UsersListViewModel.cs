using ADServerDAL.Entities.Presentation;
using ADServerDAL.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ADServerDAL.Models;
using ADServerDAL.Models.Base;

namespace ADServerManagementWebApplication.Models
{
	public class UsersListViewModel: ListViewModel
	{
		public IQueryable<Entity> Users { get { return Query; } }

		/// <summary>
		/// Filtry
		/// </summary>
		public UserListViewModelFilter Filters { get { return (UserListViewModelFilter) FilerBase; } }
	}
}