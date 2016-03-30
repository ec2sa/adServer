using DesktopUserControl.WebServiceADContentProvider;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.ServiceModel;
using System.Windows.Forms;
namespace DesktopUserControl
{
	public partial class AdProviderControl : UserControl
	{
		#region - Fields -
		/// <summary>
		/// Identyfikactor sesji, w ramach której jest uruchomiony bieżący pokaz reklam
		/// </summary>
		private readonly string _sessionId;

		/// <summary>
		/// TRUE - jeśli trwa sekwencyjny pokaz reklam (obiektów multimedialnych), FALSE - w przeciwnym wypadku.
		/// Ustawienie tej właściwości na TRUE - uruchamia pokaz reklam.
		/// Ustawienie tej właściwości na FALSE - wyłącza pokaz reklam.
		/// </summary>
		private bool _isActive;

		#endregion

		#region - Properties -

		private string _weburl;

		/// <summary>
		/// Adres URL do webserwisu, z którego mają być pobierane obiekty multimedialne
		/// </summary>
		public string WebServiceUrl
		{
			get
			{
				if (string.IsNullOrEmpty(_weburl))
					return "http://adapp.ec2.pl/AdServerWS/WebServiceADContentProvider.asmx";
				return _weburl;
			}
			set { _weburl = value; }
		}

		/// <summary>
		/// Częstotliwość pobierania z webserwisu kolejnych obiektów multimedialnych (w milisekundach)
		/// </summary>
		public int RequestFrequency { get; set; }

		/// <summary>
		/// Maksymalny dozwolony rozmiar przychodzącej odpowiedzi w bajtach
		/// </summary>
		public int MaxReceivedMessageSize { get; set; }

		/// <summary>
		/// Maksymalny dozwolony rozmiar bufora przychodzącej odpowiedzi w bajtach
		/// </summary>
		public int MaxBufferSize { get; set; }

		/// <summary>
		/// Maksymalny dozwolony czas oczekiwania na otworzenie połączenia z web service w milisekundach
		/// </summary>
		public int OpenTimeOut { get; set; }

		/// <summary>
		/// Maksymalny dozwolony czas oczekiwania na odpowiedź z web service w milisekundach
		/// </summary>
		public int ReciveTimeOut { get; set; }

		/// <summary>
		/// Maksymalny dozwolony czas wysyłania żądania do web service w milisekundach
		/// </summary>
		public int SendTimeOut { get; set; }

		/// <summary>
		/// Flaga informująca, że zgłoszono żadanie zatrzymania pokazu
		/// </summary>
		public bool StoppingRequest { get; private set; }

		/// <summary>
		/// ID nośnika
		/// </summary>
		public int ID { get; set; }

		/// <summary>
		/// TRUE - jeśli trwa sekwencyjny pokaz reklam (obiektów multimedialnych), FALSE - w przeciwnym wypadku.
		/// Ustawienie tej właściwości na TRUE - uruchamia pokaz reklam.
		/// Ustawienie tej właściwości na FALSE - wyłącza pokaz reklam.
		/// </summary>
		public bool IsActive
		{
			get
			{
				return _isActive;
			}

			set
			{
				if (_isActive == value)
					return;
				// Jeżeli poprzednia operacja jeszcze nie dobiegła końca - anuluj żadanie
				if (StoppingRequest)
				{
					return;
				}

				_isActive = value;

				if (_isActive)
				{
					StartDisplayingAds();
				}
				else
				{
					// jeżeli worker jest wciąż uruchomiony (System.Threading.Sleep) odnotuj informację o żądaniu przerwania
					if (bgWorker.IsBusy)
					{
						StoppingRequest = true;
					}
				}
			}
		}

		public string _data0 { get; set; }
		public string _data1 { get; set; }
		public string _data2 { get; set; }
		public string _data3 { get; set; }


		private string Url { get; set; }
		private int ObjStatusCode { get; set; }
		private int ObjId { get; set; }
		#endregion

		#region - Constructors -
		public AdProviderControl()
		{
			_sessionId = Guid.NewGuid().ToString();
			InitializeComponent();
			pbAd.SizeMode = PictureBoxSizeMode.StretchImage;
			MaxReceivedMessageSize = int.MaxValue;
			MaxBufferSize = int.MaxValue;
			OpenTimeOut = (int)TimeSpan.FromMinutes(2).TotalMilliseconds;
			ReciveTimeOut = (int)TimeSpan.FromMinutes(2).TotalMilliseconds;
			SendTimeOut = (int)TimeSpan.FromMinutes(2).TotalMilliseconds;
			StoppingRequest = false;
		}
		#endregion

		#region - Delegates and events -
		public delegate void ErrorsOccuredHandler(object sender, List<string> errors);
		/// <summary>
		/// Zdarzenie informujące o tym, że wystąpiły jakieś błędy
		/// </summary>
		public event ErrorsOccuredHandler ErrorsOccured;

		/// <summary>
		/// Zdarzenie informujące o tym, że pokaz został zatrzymany
		/// </summary>
		public event EventHandler AdProviderStopped;

		/// <summary>
		/// Zdarzenie informujące o tym, że pokaz został rozpoczęty
		/// </summary>
		public event EventHandler AdPrividerStarted;

		#endregion

		#region - Private methods -
		/// <summary>
		/// Sprawdza, czy zostały wypełnione wszystkie propertasy, które są niezbędne do pobierania i wyświetlania reklam
		/// </summary>
		/// <returns>TRUE - wszystkie propertasy zostały wypełnione i pokaz reklam jest możliwy do uruchomienia; FALSE - w pozostałych przypadkach</returns>
		private bool IsValid()
		{
			var valid = !string.IsNullOrEmpty(WebServiceUrl);
			valid = valid && Height > 0;
			valid = valid && Width > 0;

			return valid;
		}

		/// <summary>
		/// Wyświetla obiekt multimedialny
		/// </summary>
		/// <param name="file"></param>
		private void ShowImage(AdFile file)
		{
			if (file == null || file.Contents == null || file.Contents.Length <= 0 || !_isActive)
				return;
			var image = Image.FromStream(new MemoryStream(file.Contents));

			CrossThreadingHelper.InvokeIfRequired(pbAd, () =>
			{
				pbAd.Image = image;
			});
		}

		/// <summary>
		/// Łączy się z webserwisem w celu pobrania obiektu multimedialnego, a następnie wyświetla pobrany obiekt
		/// </summary>
		private void DisplayAd()
		{
			try
			{
				if (!IsValid())
				{
					return;
				}

				try
				{
					var binding = new BasicHttpBinding
					{
						MaxReceivedMessageSize = MaxReceivedMessageSize,
						MaxBufferSize = MaxBufferSize,
						OpenTimeout = TimeSpan.FromMilliseconds(OpenTimeOut),
						SendTimeout = TimeSpan.FromMilliseconds(SendTimeOut),
						ReceiveTimeout = TimeSpan.FromMilliseconds(ReciveTimeOut)
					};

					var client = new WebServiceADContentProviderSoapClient(binding, new EndpointAddress(WebServiceUrl));
					var response = client.GetMultimediaObject(new GetMultimediaObject_Request
					{
						SessionId = _sessionId,
						ID = ID,
						Data0 = _data0,
						Data1 = _data1,
						Data2 = _data2,
						Data3 = _data3,
						RequestDate = DateTime.Now,
						RequestSource = 1
					});

					if (response != null)
					{
						var result = JsonConvert.DeserializeObject<GetMultimediaObject_Response>(response);
						if (!result.ErrorsOccured && (result.ErrorMessage == null || result.ErrorMessage.Count == 0))
						{
							if (result.File != null)
							{
								ShowImage(result.File);
								Url = result.File.URL;
								ObjId = result.File.ID;
								ObjStatusCode = result.File.StatusCode;
							}
							else
							{
								ErrorsOccured(this, new List<string> { "Serwer nie zwrócił pliku." });
							}
						}
						else
						{
							if (ErrorsOccured != null)
							{
								var errors = result.ErrorMessage;
								ErrorsOccured(this, errors);
							}
						}
					}

				}
				catch (Exception ex)
				{
					if (ErrorsOccured != null)
					{
						var errors = new List<string> { ex.Message };
						ErrorsOccured(this, errors);
					}
				}

			}
			catch (Exception ex)
			{
				if (ErrorsOccured != null)
				{
					var errors = new List<string> { ex.Message };
					ErrorsOccured(this, errors);
				}
			}
		}

		/// <summary>
		/// Uruchamia sekwencyjny pokaza reklam, włączając timer, na którym reklamy są pobierane/wyświetlane
		/// </summary>
		private void StartDisplayingAds()
		{
			if (IsActive)
			{
				bgWorker.RunWorkerAsync();
			}
		}

		#endregion

		#region - Event methods -

		private void bgWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			_isActive = true;

			while (IsActive)
			{
				if (AdPrividerStarted != null)
				{
					AdPrividerStarted(this, EventArgs.Empty);
				}

				DisplayAd();

				if (!IsActive)
				{
					return;
				}

				System.Threading.Thread.Sleep(RequestFrequency);
			}
		}

		private void bgWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
		{
			_isActive = false;
			StoppingRequest = false;

			if (AdProviderStopped != null)
			{
				AdProviderStopped(this, EventArgs.Empty);
			}
		}

		private void pbAd_Click(object sender, EventArgs e)
		{
			if (Url != null)
			{
				var binding = new BasicHttpBinding
				{
					MaxReceivedMessageSize = MaxReceivedMessageSize,
					MaxBufferSize = MaxBufferSize,
					OpenTimeout = TimeSpan.FromMilliseconds(OpenTimeOut),
					SendTimeout = TimeSpan.FromMilliseconds(SendTimeOut),
					ReceiveTimeout = TimeSpan.FromMilliseconds(ReciveTimeOut)
				};

				var client = new WebServiceADContentProviderSoapClient(binding, new EndpointAddress(WebServiceUrl));

				client.MultimediaObjectUrlClicked(new GetMultimediaObject_Request
				{
					SessionId = _sessionId,
					ID = ID,
					Data0 = _data0,
					Data1 = _data1,
					Data2 = _data2,
					Data3 = _data3,
					RequestDate = DateTime.Now,
					RequestSource = 1
				}, ObjId, ObjStatusCode);

				System.Diagnostics.Process.Start(Url);
			}
		}
		#endregion

	}
}
