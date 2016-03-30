using System.Collections.Generic;

namespace ADServerDAL.Entities.Presentation
{
	/// <summary>
	/// Reprezentacja obiektu statystyki
	/// </summary>
	public class StatisticItem : PresentationItem
	{
		/// <summary>
		/// Identyfikator
		/// </summary>
		public int ID { get; set; }

		/// <summary>
		/// Identyfikator obiektu
		/// </summary>
		public int MultimediaObjectId { get; set; }

		/// <summary>
		/// Nazwa obiektu
		/// </summary>
		public string MultimediaObjectName { get; set; }

		/// <summary>
		/// Nazwa typu obiektu
		/// </summary>
		public string MultimediaObjectType { get; set; }

		/// <summary>
		/// Wysokość
		/// </summary>
		public int MultimediaObjectHeight { get; set; }

		/// <summary>
		/// Szerokość
		/// </summary>
		public int MultimediaObjectWidth { get; set; }

		/// <summary>
		/// Lista nazwa kategorii
		/// </summary>
		public List<string> Categories { get; set; }

		/// <summary>
		/// Lista nazw kampanii
		/// </summary>
		public List<string> Campaigns { get; set; }

		/// <summary>
		/// Lista nazwa kategorii
		/// </summary>
		public string CategoriesString
		{
			get
			{
				return string.Join(", ", Categories.ToArray());
			}
		}

		/// <summary>
		/// Lista nazw kampanii
		/// </summary>
		public string CampaignString
		{
			get
			{
				return string.Join(", ", Campaigns.ToArray());
			}
		}

		/// <summary>
		/// Opis typu obiektu
		/// </summary>
		public string MultimediaObjectTypeDescriptor
		{
			get
			{
				return string.Format("{0} ({1}x{2})", MultimediaObjectType, MultimediaObjectWidth, MultimediaObjectHeight);
			}
		}

		/// <summary>
		/// Data żądania
		/// </summary>
		public System.DateTime RequestDate { get; set; }

		/// <summary>
		/// Data odpowiedzi
		/// </summary>
		public System.DateTime ResponseDate { get; set; }

		/// <summary>
		/// Adres IP żadania
		/// </summary>
		public string RequestIP { get; set; }

		/// <summary>
		/// Referrer
		/// </summary>
		public string Referrer { get; set; }

		/// <summary>
		/// Dodatkowe info
		/// </summary>
		public string AdditionalInfo { get; set; }

		/// <summary>
		/// Imię
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Nazwisko
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Pesel
		/// </summary>
		//public string PESEL { get; set; }

		/// <summary>
		/// Email
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Nazwa firmy
		/// </summary>
		//public string CompanyName { get; set; }

		/// <summary>
		/// Inne informacje
		/// </summary>
		public string Other { get; set; }

		/// <summary>
		/// Identyfikator sesji
		/// </summary>
		public string SessionId { get; set; }

		/// <summary>
		/// Źródło, z którego nadszedł request; typ wyliczeniowy: ADServerDAL.Statistics.RequestSourceType
		/// </summary>
		public int RequestSource { get; set; }

		/// <summary>
		/// Id użytkownika
		/// </summary>
		public int UserID { get; set; }

		/// <summary>
		/// Czy przekierowano
		/// </summary>
		public bool Clicked { get; set; }

		/// <summary>
		/// Ilość punktów wykorzystanych
		/// </summary>
		public decimal AdPoints { get; set; }

		public int? DeviceId { get; set; }
	}
}