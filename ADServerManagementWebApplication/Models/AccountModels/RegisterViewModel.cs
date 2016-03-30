using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace ADServerManagementWebApplication.Models
{
	public class RegisterViewModel
	{
		[Required]
		[Display(Name = "Login używany do logowania")]
		[MaxLength(30)]
		public string Name { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "{0} musi zawierać przynajmniej {2} znaków.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Nowe hasło")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Potwierdź hasło")]
		[Compare("Password", ErrorMessage = "Hasło oraz potwierdzenie hasła nie są takie same.")]
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
}