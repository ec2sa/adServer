using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADServerDAL.Entities.Presentation;
using ADServerDAL.Models;

namespace ADServerDAL.Abstract
{
	public interface IDeviceRepository : IDisposable
	{
		/// <summary>
		/// Kolekcja wszystkich nośników
		/// </summary>
		IQueryable<Device> Devices { get; }

		/// <summary>
		/// Zapisanie użytkownika
		/// </summary>
		/// <param name="device">Obiekt nośnika</param>
		/// <returns></returns>
		ApiResponse Save(Device device);

		/// <summary>
		/// Usunięcie użytkownika
		/// </summary>
		/// <param name="id">Id nośnika</param>
		/// <returns></returns>
		ApiResponse Delete(int id);

		/// <summary>
		/// Możliwość ustawienia innego kontekstu niż wbudowany
		/// </summary>
		/// <param name="context">Nowy kontekst EF</param>
		void SetContext(System.Data.Entity.DbContext context);

		/// <summary>
		/// Zwraca identyfikatory kategorii powiązanych z danym nośnikiem
		/// </summary>
		/// <param name="Id">Identyfikator kategorii</param>
		IEnumerable<int> CatoegoriesToDev(int Id);

        Device GetDeviceById(int id);
	}
}
