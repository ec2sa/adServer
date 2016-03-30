using System.ComponentModel;
using System.Data.Entity;
using System.Dynamic;
using ADServerDAL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADServerDAL.Models;
using ADServerDAL.Models.Base;
using ADServerDAL.Models.Interface;

namespace ADServerDAL.Concrete
{
	/// <summary>
	/// Klasa bazowa dla implementacji repozytoriów
	/// </summary>
	public class EFBaseRepository : IDisposable
	{
		/// <summary>
		/// Określa czy kontekst jest domyślny, czy został przypisany ręcznie
		/// </summary>
		private bool isBuildInContext = true;

		/// <summary>
		/// Kontekst bazodanowych (EF)
		/// </summary>
		protected AdServContext Context { get; set; }

		/// <summary>
		/// Możliwość ustawienia innego kontekstu niż wbudowany
		/// </summary>
		/// <param name="context">Nowy kontekst EF</param>
		protected void SetNewContext(System.Data.Entity.DbContext context)
		{
			if (context != null)
			{
				Context = context as AdServContext;
				isBuildInContext = false;
			}
		}

		public EFBaseRepository()
		{
			Context = new AdServContext();
		}

		public void Dispose()
		{
			if (Context != null && isBuildInContext)
			{
				Context.Dispose();
				Context = null;
			}
		}

		/// <summary>
		/// Metoda określająca ile elementów należy pominąć przy stronnicowaniu
		/// </summary>
		/// <param name="page">Numer żądanej strony</param>
		/// <param name="count">Liczba elementów na stronie</param>
		/// <param name="itemsCount">Liczba wszystkich elementów</param>
		protected int GetSkip(int page, int count, int itemsCount)
		{
			if (page < 2)
			{
				return 0;
			}
			var skip = (page - 1) * count;
			if (skip >= itemsCount)
			{
				skip = itemsCount - 1;
			}
			return skip;
		}

		protected void ObjectRelationCampaign(IEnumerable<Entity> collection, ref dynamic dbEntry)
		{
			var list =
				from o in Context.Campaigns
				join c in collection.Select(it => it.Id) on o.Id equals c
				select o;

			var re = ((ICollection<Campaign>)dbEntry.Campaigns).ToList();
			foreach (var subs in re)
			{
				dbEntry.Campaigns.Remove(subs);
			}

			foreach (var adds in list)
			{
				dbEntry.Campaigns.Add(adds);
			}
		}
		protected void ObjectRelationDevice(IEnumerable<Entity> collection, ref dynamic dbEntry)
		{
			var list =
				from o in Context.Devices
				join c in collection.Select(it => it.Id) on o.Id equals c
				select o;

			var re = ((ICollection<Device>)dbEntry.Devices).ToList();
			foreach (var subs in re)
			{
				dbEntry.Devices.Remove(subs);
			}

			foreach (var adds in list)
			{
				dbEntry.Devices.Add(adds);
			}
		}
		protected void ObjectRelationDeletedDevice(IEnumerable<Entity> collection, ref dynamic dbEntry)
		{
			var list =
				from o in Context.Devices
				join c in collection.Select(it => it.Id) on o.Id equals c
				select o;

			var re = ((ICollection<DeletedDevices>)dbEntry.DeletedDevices).ToList();
			foreach (var subs in re)
			{
				dbEntry.DeletedDevices.Remove(subs);
			}

			foreach (var adds in list)
			{
				var a = new DeletedDevices { Id = adds.Id, Name = adds.Name };
				dbEntry.DeletedDevices.Add(a);
			}
		}
		protected void ObjectRelationMmObjects(IEnumerable<Entity> collection, ref dynamic dbEntry)
		{
			var list =
				from o in Context.MultimediaObjects
				join c in collection.Select(it => it.Id) on o.Id equals c
				select o;

			var re = ((ICollection<MultimediaObject>)dbEntry.MultimediaObjects).ToList();
			foreach (var subs in re)
			{
				dbEntry.MultimediaObjects.Remove(subs);
			}

			foreach (var adds in list)
			{
				dbEntry.MultimediaObjects.Add(adds);
			}
		}
		protected void ObjectRelationCategory(IEnumerable<Entity> collection, ref dynamic dbEntry)
		{
			var list =
				from o in Context.Categories
				join c in collection.Select(it => it.Id) on o.Id equals c
				select o;

			var re = ((ICollection<Category>)dbEntry.Categories).ToList();
			foreach (var subs in re)
			{
				dbEntry.Categories.Remove(subs);
			}
			Context.SaveChanges();
			foreach (var adds in list)
			{
				dbEntry.Categories.Add(adds);
			}
		}
	}
}
