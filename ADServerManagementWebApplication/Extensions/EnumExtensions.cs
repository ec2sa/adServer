using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using ADServerDAL.Entities.Presentation;
using Microsoft.Ajax.Utilities;

namespace ADServerManagementWebApplication.Extensions
{
	public static class EnumExtensions
	{
		/// <summary>
		/// Metoda zwraca listę dla dropdowna
		/// </summary>
		/// <param name="value">Typ</param>
		/// <param name="selected">Wybrany element</param>
		/// <returns>Lista selectedlistitem</returns>
		public static IEnumerable<SelectListItem> GetEnumAsSelectList(this Type value, Object selected = null)
		{
			var stringItems = Enum.GetNames(value);
			var selectedStringItems =
				selected == null ? new string[0] : selected.ToString().Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

			return (from item in stringItems let fi = value.GetField(item) let attributes = (DisplayAttribute[])fi.GetCustomAttributes(typeof(DisplayAttribute), true) select attributes.Length > 0 ? new SelectListItem { Text = attributes[0].Name, Value = item } : new SelectListItem { Text = item, Value = (item), Selected = selectedStringItems.Contains(item) }).ToList();
		}

		/// <summary>
		/// Łatwa funkcja zamieniająca string na enum
		/// </summary>
		/// <typeparam name="T">Typ enuma</typeparam>
		/// <param name="Type">Nazwa enuma</param>
		/// <returns>Enum</returns>
		public static T GetEnumFromString<T>(string Type)
		{
			return (T)Enum.Parse(typeof(T), Type);
		}
	}
}