using ADServerDAL.Entities.Presentation;
using System;
using System.Linq;
using ADServerDAL.Models;

namespace ADServerDAL.Abstract
{
	public interface IUsersRepository : IDisposable
	{
		/// <summary>
		/// Kolekcja wszystkich użytkowników
		/// </summary>
		IQueryable<User> Users { get; }

		/// <summary>
		/// Zapisanie użytkownika
		/// </summary>
		/// <param name="user">Obiekt użytkownika</param>
		/// <returns></returns>
		ApiResponse Save(User user);

		/// <summary>
		/// Usunięcie użytkownika
		/// </summary>
		/// <param name="ID">Id użytkownika</param>
		/// <returns></returns>
		ApiResponse Delete(int ID);


	}
}