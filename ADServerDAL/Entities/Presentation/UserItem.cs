using System.Collections.Generic;
using System.Security.Policy;
using ADServerDAL.Models;

namespace ADServerDAL.Entities.Presentation
{
	/// <summary>
	/// Reprezentacja obiektu użytkownika
	/// </summary>
	public class UserItem : PresentationItem
	{
		/// <summary>
		/// Identyfikator
		/// </summary>
		public int ID { get; set; }

		/// <summary>
		/// Login użytkownika
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Hasło użytkownika
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// Imię użytkownika
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Nazwisko użytkownika
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Uprawnienia użytkownika
		/// </summary>
		public Role Role { get; set; }

		/// <summary>
		/// Pracownik zablokowany
		/// </summary>
		public bool IsBlocked { get; set; }

		/// <summary>
		/// Punkty użytkownika
		/// </summary>
		public decimal AdPoints { get; set; }

		/// <summary>
		/// Lista powiązanych kampanii
		/// </summary>
		public List<Campaign> Campaigns { get; set; }


		/// <summary>
		/// Lista obiektów użytkownika
		/// </summary>
        public List<MultimediaObject> MultimediaObjects { get; set; }

        public string Email { get; set; }
        public string AdditionalInfo { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string Url { get; set; }
		public ICollection<Device> Devices { get; set; } 
	}
}