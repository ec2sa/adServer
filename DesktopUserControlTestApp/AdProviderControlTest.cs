using DesktopUserControl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;
using System.Windows;

namespace DesktopUserControlTestApp
{
	public partial class AdProviderControlTest : Form
	{
		private string url = string.Empty;

		#region - Constructors -

		public AdProviderControlTest()
		{
			InitializeComponent();
			url = GetWebServiceURL();
			adProviderControl1.AdPrividerStarted += adProviderControl1_AdPrividerStarted;
		}

		#endregion - Constructors -

		#region - Private methods -

		/// <summary>
		/// Pobiera z pliku konfiguracyjnego adres do webserwisu WebServiceAdContentProvider
		/// </summary>
		/// <returns></returns>
		private string GetWebServiceURL()
		{
			var key = "WebServiceADContentProviderURL";
			var urlKey = ConfigurationManager.AppSettings[key];

			if (urlKey != null && !string.IsNullOrEmpty(urlKey))
			{
				return urlKey.ToString();
			}
			else
			{
				MessageBox.Show(string.Format("W pliku konfiguracyjnym nie znaleziono klucza '{0}' definiującego URL do webSerwisu WebServiceADContentProvider", key), "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return null;
		}

		/// <summary>
		/// Uruchamia sekwencyjny pokaz reklam
		/// </summary>
		private void DisplayAds1()
		{
            adProviderControl1.RequestFrequency = (int)nudAd1Freq.Value;
            adProviderControl1.Width = (int)nudWidth.Value;
            adProviderControl1.Height = (int)nudHeight.Value;
            adProviderControl1.WebServiceUrl = url;
            adProviderControl1.ID = (int)tbDevice.Value;

			if (!string.IsNullOrEmpty(url))
			{
				adProviderControl1.IsActive = true;
			}
		}

		/// <summary>
		/// Logowanie błędów i komunikatów
		/// </summary>
		private void ShowErrors1(List<string> errors)
		{
			if (errors != null && errors.Count > 0)
			{
				CrossThreadingHelper.InvokeIfRequired(tbErrors1, () =>
				{
					foreach (var e in errors)
					{
						tbErrors1.Text += DateTime.Now.ToString() + "\t" + e;
						tbErrors1.Text += Environment.NewLine;
						tbErrors1.SelectionStart = tbErrors1.Text.Length;
						tbErrors1.ScrollToCaret();
					}
				});
			}
		}

		/// <summary>
		/// Zmiana rozmiaru obrazka
		/// </summary>
		private void nudWidth_ValueChanged(object sender, EventArgs e)
		{
			int width = (int)nudWidth.Value;
			int height = (int)nudHeight.Value;
			adProviderControl1.Width = width;
			adProviderControl1.Height = height;
		}

		#endregion - Private methods -

		#region - Event methods -

		private void AdProviderControlTest_Load(object sender, EventArgs e)
		{
			adProviderControl1.ErrorsOccured += adProviderControl1_ErrorsOccured;
		}

		private void adProviderControl1_AdPrividerStarted(object sender, EventArgs e)
		{
			try
			{
				CrossThreadingHelper.InvokeIfRequired(nudAd1Freq, () =>
				{
					adProviderControl1.RequestFrequency = (int)nudAd1Freq.Value;
				}); 
				CrossThreadingHelper.InvokeIfRequired(nudAd1Freq, () =>
				{
					adProviderControl1.ID = (int)tbDevice.Value;
				});			
			}
			catch (Exception ex)
			{
				adProviderControl1_ErrorsOccured(null, new List<string> { ex.Message });
			}
		}

		/// <summary>
		/// Błądy zgłoszone przez kontrolkę
		/// </summary>
		private void adProviderControl1_ErrorsOccured(object sender, List<string> errors)
		{
			ShowErrors1(errors);
		}

		/// <summary>
		/// Uruchomienie kontroli 1
		/// </summary>
		private void btnAd1Run_Click(object sender, EventArgs e)
		{
			DisplayAds1();
		}

		/// <summary>
		/// Zatrzymanie kontroli 1
		/// </summary>
		private void btnAd1Stop_Click(object sender, EventArgs e)
		{
			adProviderControl1.IsActive = false;
		}

		/// <summary>
		/// Monitorowanie dostępnych funkcji
		/// </summary>
		private void AdMonitor_Tick(object sender, EventArgs e)
		{
			btnAd1Run.Enabled = !adProviderControl1.IsActive && !adProviderControl1.StoppingRequest;
			btnAd1Stop.Enabled = adProviderControl1.IsActive && !adProviderControl1.StoppingRequest;

			panelParameters.Enabled = !adProviderControl1.IsActive && !adProviderControl1.StoppingRequest;
		}

		/// <summary>
		/// Wyczyszczenie logów
		/// </summary>
		private void btnClearLog1_Click(object sender, EventArgs e)
		{
			tbErrors1.Clear();
		}

		#endregion - Event methods -
	}
}