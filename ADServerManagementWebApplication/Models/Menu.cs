using System.Collections.Generic;

namespace ADServerManagementWebApplication.Models
{
    /// <summary>
    /// Klasa obsługująca menu
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// Nazwa opcji
        /// </summary>
        public string OptionName { get; set; }

        /// <summary>
        /// Nazwa akcji
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Nazwa kontrolera
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// Czy aktywne dla wszystkich akcji
        /// </summary>
        public bool ActiveForAllAction { get; set; }

        /// <summary>
        /// Javascriptova metoda, któa ma być wywołana na onClicku
        /// </summary>
        public string OnClick { get; set; }

        /// <summary>
        /// Podmenu
        /// </summary>
        public List<Menu> Submenu { get; set; }

		/// <summary>
		/// Uprawnienia umożliwiające dostęp
		/// </summary>
		public string[] Role { get; set; }

		/// <summary>
		/// Czy Single Page
		/// </summary>
	    public bool ListView { get; set; }
    }
}