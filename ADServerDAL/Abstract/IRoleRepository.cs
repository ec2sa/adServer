using ADServerDAL.Entities.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADServerDAL.Models;

namespace ADServerDAL.Abstract
{
	public interface IRoleRepository : IDisposable
	{
		/// <summary>
		/// Kolekcja wszystkich ról
		/// </summary>
		IQueryable<Role> Roles { get; }

		/// <summary>
		/// Zapisanie użytkownika
		/// </summary>
		/// <param name="Role"></param>
		/// <returns></returns>
		ApiResponse Save(Role Role);

		/// <summary>
		/// Usunięcie roli
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		ApiResponse Delete(int ID);
	}
}
