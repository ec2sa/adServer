using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;

namespace DesktopUserControlWPFTest
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly string _url = "";

		public MainWindow()
		{
			InitializeComponent();
			AdControl.AdPrividerStarted += Provider_Started;
			AdControl.ErrorsOccured += ErrorsOccured;
			_url = GetWebServiceURL();
		}

		/// <summary>
		/// Pobiera z pliku konfiguracyjnego adres do webserwisu WebServiceAdContentProvider
		/// </summary>
		/// <returns></returns>
		private string GetWebServiceURL()
		{
			const string key = "WebServiceADContentProviderURL";
			var urlKey = ConfigurationManager.AppSettings[key];

			if (urlKey != null && !string.IsNullOrEmpty(urlKey))
			{
				return urlKey;
			}
			MessageBox.Show(string.Format("W pliku konfiguracyjnym nie znaleziono klucza '{0}' definiującego URL do webSerwisu WebServiceADContentProvider", key), "Błąd");

			return null;
		}

		private void Provider_Started(object sender, EventArgs e)
		{
			try
			{
				// Dostęp do odanych z poziomu wpf odbywa się poprzez dispatcher
				czesto.Dispatcher.BeginInvoke(new Action(delegate { AdControl.RequestFrequency = int.Parse(czesto.Text); }));

				// Jeśli mamy zahardkodowane dane wrzucamy je bezpośrednio
				AdControl.ID = 1;
			}
			catch (Exception ex)
			{
				ErrorsOccured(null, new List<string> { ex.Message });
			}
		}

		/// <summary>
		/// Błądy zgłoszone przez kontrolkę
		/// </summary>
		private void ErrorsOccured(object sender, List<string> errors)
		{
			foreach (string it in errors)
				Log.Dispatcher.BeginInvoke(new Action(delegate { Log.Text += it + Environment.NewLine; }));
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			AdControl.IsActive = !AdControl.IsActive;
			Run.Content = AdControl.IsActive ? "Zatrzymaj" : "Uruchom";
		}
	}
}