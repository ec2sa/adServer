using ADServerDAL.Concrete;
using ADServerDAL.Entities.Presentation;
using ADServerDAL.Models;
using Microsoft.AspNet.Identity;
using System.Security.Principal;

namespace ADServerManagementWebApplication.Models
{
	/// <summary>
	/// Klasa obiektu użytkownika aplikacji
	/// </summary>
	public class ApplicationUser : User,  IPrincipal, IIdentity
	{
		#region -Fields-

		/// <summary>
		/// Nazwa użytkownika
		/// </summary>
		public string UserName
		{
			get { return Name; }
			set { Name = value; }
		}

		/// <summary>
		/// Identity użytkonika
		/// </summary>
		public IIdentity Identity { get; set; }

		/// <summary>
		/// Długa nazwa użytkownika
		/// </summary>
		public string LongName
		{
			get { return FirstName + " " + LastName; }
		}

		/// <summary>
		/// Sprawdzenie czy rola zapytana jest taka sama jak użytkownika
		/// </summary>
		/// <param name="role">Sprawdzana rola</param>
		/// <returns>Prawda gdy taka sama, fałsz gdy niezgodna</returns>
		public bool IsInRole(string role)
		{
			return role == Role.Name;
		}
		
		/// <summary>
		/// Typ autentyfikacji Cookie
		/// </summary>
		public string AuthenticationType
		{
			get { return DefaultAuthenticationTypes.ApplicationCookie; }
		}

		/// <summary>
		/// Czy jest zalogowany
		/// </summary>
		public bool IsAuthenticated
		{
			get { return true; }
		}

		#endregion -Fields-

		#region -Constructors-

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="name">Nazwa użytkownika</param>
		/// <param name="password">Hasło użytkownika</param>
		/// <param name="firstName">Imie użytkownika</param>
		/// <param name="lastName">Nazwisko użytkownika</param>
		/// <param name="id">Id użytkownika</param>
		/// <param name="role">Rola użytkownika</param>
		public ApplicationUser(string name, string password, string firstName, string lastName, int id, Role role)
		{
			//Identity = new GenericIdentity(name, role.Name);
			UserName = name;
			Password = password;
			FirstName = firstName;
			LastName = lastName;
			Role = role;
			Id = id;
		}

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="user">Klasa bazowa użytkownika</param>
		public ApplicationUser(User user)
		{
			//Identity = new GenericIdentity(user.Name, user.Role.Name);
			Name = user.Name;
			Id = user.Id;
			LastName = user.LastName;
			FirstName = user.FirstName;
			Password = user.Password;
			Role = user.Role;
			Campaigns = user.Campaigns;
			IsBlocked = user.IsBlocked;
			AdPoints = user.AdPoints;
			Email = user.Email;
		}

		/// <summary>
		/// Konstruktor domyślny
		/// </summary>
		public ApplicationUser()
		{
		}

		#endregion -Constructors-
	}
}