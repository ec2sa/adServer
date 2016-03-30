using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.ComTypes;
using System.Web.Mvc;

namespace ADServerManagementWebApplication.Models
{
	public class ManageUserViewModel
	{
		[HiddenInput]
		public int Id { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Aktualne hasło")]
		public string OldPassword { get; set; }

		[StringLength(100, ErrorMessage = "{0} musi zawierać przynajmniej {2} znaków.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Nowe Hasło")]
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Potwierdź hasło")]
		[System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "Hasło oraz potwierdzenie hasła nie są takie same.")]
		public string ConfirmPassword { get; set; }

		[Required]
		[MaxLength(30)]
		[Display(Name = "Imię")]
		public string FirstName { get; set; }

		[Required]
		[MaxLength(30)]
		[Display(Name = "Nazwisko")]
		public string LastName { get; set; }

		[MaxLength(30)]
		[Display(Name = "Nazwa firmy")]
		public string CompanyName { get; set; }

		[MaxLength(30)]
		[Display(Name = "Adres")]
		public string CompanyAddress { get; set; }

		[MaxLength(250)]
		[Display(Name = "Dane dodatkowe")]
		[DataType(DataType.MultilineText)]
		public string AdditionalInfo { get; set; }

		[Required]
		[DataType(DataType.Url)]
		[MaxLength(70)]
		[Display(Name = "Adres URL strony firmy")]
		[Url]
		public string Url { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		[MaxLength(70)]
		[Display(Name = "Email")]
		[EmailAddress]
		public string Email { get; set; }
	}

	public class LoginViewModel
	{
		[Required]
		[Display(Name = "Login użytkownika")]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Hasło")]
		public string Password { get; set; }

		[Display(Name = "Zapamiętaj mnie")]
		public bool RememberMe { get; set; }
	}

	public class AdminManageViewModel
	{
		[Required]
		[MaxLength(30)]
		[Display(Name = "Login użytkownika")]
		public string UserName { get; set; }

		[StringLength(100, ErrorMessage = "{0} musi zawierać przynajmniej {2} znaków.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Hasło")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Potwierdź hasło")]
		[System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Hasło oraz potwierdzenie hasła nie są takie same.")]
		public string ConfirmPassword { get; set; }

		[Display(Name = "Role")]
		public SelectList Role { get; set; }

		[Display(Name = "Rola")]
		public int RoleID { get; set; }

		public int ID { get; set; }

		[Display(Name = "Użytkownik zablokowany")]
		public bool IsBlocked { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "{0} musi zawierać przynajmniej {2} znaków.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Hasło")]
		public string PasswordR { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Potwierdź hasło")]
		[System.ComponentModel.DataAnnotations.Compare("PasswordR", ErrorMessage = "Hasło oraz potwierdzenie hasła nie są takie same.")]
		public string ConfirmPasswordR { get; set; }

		[Required]
		[Display(Name = "AdPoints")]
		public decimal AdPoints { get; set; }

		[MaxLength(30)]
		[Display(Name = "Imię")]
		[Required]
		public string FirstName { get; set; }

		[MaxLength(30)]
		[Required]
		[Display(Name = "Nazwisko")]
		public string LastName { get; set; }

		[Display(Name = "Liczba obiektów")]
		public int MmObjectCount { get; set; }

		[Display(Name = "Liczba kampanii")]
		public int CampaignCount { get; set; }

		[Display(Name = "Ilość wyświetleń")]
		public int ViewCount { get; set; }

		[Display(Name = "Ilość przekierowań")]
		public int ClickCount { get; set; }

		[Display(Name = "Punkty za hosting reklamy - wyświetlenie")]
		public Decimal ViewAdd { get; set; }

		[Display(Name = "Punkty za wyświetlanie reklamy")]
		public Decimal ViewSub { get; set; }

		[Display(Name = "Punkty za hosting reklamy - przekierowanie")]
		public Decimal ClickAdd { get; set; }

		[Display(Name = "Punkty za wyświetlanie reklamy")]
		public Decimal ClickSub { get; set; }

		[DataType(DataType.EmailAddress)]
		[MaxLength(70)]
		[Display(Name = "Email")]
		[EmailAddress]
		[Required]
		public string Email { get; set; }
	}
}