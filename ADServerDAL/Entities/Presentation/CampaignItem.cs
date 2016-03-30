using System;
using System.Collections.Generic;
using System.Security.Policy;
using ADServerDAL.Models;

namespace ADServerDAL.Entities.Presentation
{
	/// <summary>
	/// Reprezentacja obiektu kampanii
	/// </summary>
	public class CampaignItem : PresentationItem
	{
		/// <summary>
		/// Identyfikator
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Nazwa kampanii
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Opis kampanii
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Data rozpoczęcia
		/// </summary>
		public DateTime StartDate { get; set; }

		/// <summary>
		/// Data zakończenia
		/// </summary>
		public DateTime EndDate { get; set; }

		/// <summary>
		/// Identyfikator priorytetu
		/// </summary>
		public int? PriorityId { get; set; }

		/// <summary>
		/// Nazwa priorytetu
		/// </summary>
		public string PriorityName { get; set; }

		/// <summary>
		/// Czy kampania aktywna
		/// </summary>
		public bool IsActive { get; set; }

		public bool IsBlocked { get; set; }
		
		/// <summary>
		/// Ilość punktów przeznaczona na kampanie
		/// </summary>
		public decimal AdPoints { get; set; }

	    public decimal ViewValue { get; set; }
		public decimal ClickValue { get; set; }
		public IEnumerable<Device> Devices { get; set; }
	}
}