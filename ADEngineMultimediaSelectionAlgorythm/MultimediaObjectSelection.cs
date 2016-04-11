using System.CodeDom;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Contexts;
using ADServerDAL;
using ADServerDAL.Abstract;
using ADServerDAL.Entities;
using ADServerDAL.Entities.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using ADServerDAL.Models;

namespace ADEngineMultimediaSelectionAlgorythm
{
	public class MultimediaObjectSelection
	{
		#region - Classess -

		/// <summary>
		/// Obiekt będący kolekcją parametrów niezbędnych do wyszukania obiektu multimedialnego oraz do zapisania w statystykach
		/// informacji o requeście i wyniku requesta
		/// </summary>
		public class MultimediaObjectsSelectionParams
		{
			public int ID { get; set; }

			public string Data0 { get; set; }

			public string Data1 { get; set; }

			public string Data2 { get; set; }

			public string Data3 { get; set; }
			
			public string SessionId { get; set; }

			public int RequestSource { get; set; }

			public DateTime	RequestDate { get; set; }

			public string RequestIP { get; set; }
		}

		#endregion - Classess -

		#region - Fields -

		private readonly RepositorySet _repositories;

		#endregion - Fields -

		#region - Constructors -

		public MultimediaObjectSelection(RepositorySet repositories)
		{
			_repositories = repositories;
		}

		#endregion - Constructors -

		#region - Public methods -

		/// <summary>
		/// Sprawdza, czy parametry są właściwie wypełnione i czy na ich podstawie można dokonać wyszykuwania obiektu i zapisu requesta do statystyk
		/// </summary>
		/// <param name="selectionParams">Parametr do sprawdzenia</param>
		/// <param name="errors">List błędów</param>
		/// <returns>TRUE - jeśli parametr jest OK</returns>
		public bool RequestParamsAreValid(MultimediaObjectsSelectionParams selectionParams, out List<string> errors)
		{
			errors = new List<string>();
			var result = true;

			if (selectionParams == null)
			{
				errors.Add("Parametry requesta nie mogą być nullem.");
				result = false;
			}
			else
			{
				if (string.IsNullOrEmpty(selectionParams.SessionId))
				{
					errors.Add("Pole SessionId nie może być puste ani nie może być nullem.");
					result = false;
				}
				if (selectionParams.ID == 0)
				{
					errors.Add("Identyfikator nośnika nie może być 0");
					result = false;
				}
			}

			return result;
		}

		/// <summary>
		/// Wyszukuje obiekt multimedialny, który powinien zostać wysłany do klienta w odpowiedzi na jego request, a
		/// następnie zapisuje w statystykach informacje o requeście klienta i o wysłanym do niego w odpowiedzi obiekcie multimedialnym.
		/// </summary>
		/// <param name="request">Parametry opisujące request, na podstawie których zostanie wybrany odpowiedni obiekt multimedialny</param>
		/// <param name="filestream">Określa czy pliki mają być pobierane z FILESTREAM</param>
		/// <param name="validationErrors">Lista błędów</param>
		/// <returns>Obiekt multimedialny do wyświetlenia w kliencie</returns>
        public AdFile GetMultimediaObject(MultimediaObjectsSelectionParams request, bool filestream, bool add, out List<string> validationErrors)
		{
			var result = FindMultimediaObject(request, filestream, out validationErrors);

			if (result != null)
			{
				result.Contents = _repositories.MultimediaObjectRepository.GetById(result.ID).Contents;
                if (add)
                {
                    SaveStatisticsEntry(request, result);
                }
			}

			return result;
		}

		/// <summary>
		/// Metoda wyszukuje URL obiektu multimedialnego oraz aktualizuje statystyki
		/// </summary>
		/// <param name="id">Id obiektu klikniętego</param>
		/// <param name="statusCode">Priorytet</param>
		/// <param name="request">Parametry opisujące request, na podstawie których zostanie wybrany odpowiedni obiekt multimedialny</param>
		/// <returns>URL obiektu multimedialnego</returns>
		public string GetMmObjectUrl(int id, int statusCode,int cmp, MultimediaObjectsSelectionParams request, bool Add)
		{
			var obj = _repositories.MultimediaObjectRepository.GetById(id);
            if (Add)
            {
                SaveStatisticsEntry(request, new AdFile { ID = obj.Id, StatusCode = statusCode, CmpId = cmp }, true);
            }

			return obj.Url;
		}

		/// <summary>
		/// Zapisuje w statystykach informacje o requeście klienta
		/// </summary>
		/// <param name="selectionParams">Parametry opisujące request, na podstawie których został wybrany odpowiedni obiekt multimedialny</param>
		/// <param name="multimediaObject">Wybrany obiekt multimedialny (który zostanie zwrócony klientowi jako odpowiedź na jego request)</param>
		/// <param name="cliecked">Przekierowanie</param>
		public void SaveStatisticsEntry(MultimediaObjectsSelectionParams selectionParams, AdFile multimediaObject, bool cliecked = false)
		{
			var s = CreateStatisticsEntry(selectionParams, multimediaObject, cliecked);
			SaveStatisticsEntry(selectionParams, multimediaObject, s);
		}

		/// <summary>
		/// Wyszukuje obiekt multimedialny, który zostanie wysłany do klienta w odpowiedzi na jego request
		/// </summary>
		/// <param name="selectionParams">Parametry obiektu</param>
		/// <param name="filestream">Określa czy pliki mają być pobierane z FILESTREAM</param>
		/// <param name="validationErrors">lista błędów</param>
		/// <returns>Obiekt multimedialny do wyświetlenia w kliencie</returns>
		public AdFile FindMultimediaObject(MultimediaObjectsSelectionParams selectionParams, bool filestream, out List<string> validationErrors)
		{
			if (!RequestParamsAreValid(selectionParams, out validationErrors))
			{
				return null;
			}

			AdFile resultFile = null;

			var requestFromNewSession =
				!_repositories.StatisticsRepository.Statistics.Any(s => s.SessionId == selectionParams.SessionId);// == 0;

			// wyszukujemy obiekty, które zostały ostatnio pobrane przez ten sam nośnik w ramach jednej sesji
			IQueryable<int> previousObjectId = null;
			var alreadyFetchedObjects =
								from s in _repositories.StatisticsRepository.Statistics
								join moc in _repositories.CampaignRepository.Campaigns.SelectMany(it => it.MultimediaObjects) on s.MultimediaObjectId equals moc.Id
								where s.DeviceId == selectionParams.ID 
								orderby s.ResponseDate descending
								select new
								{
									sessionID = s.SessionId,
									multimediaObjectID = s.MultimediaObjectId
								};

			if (!requestFromNewSession)
			{
				// pobieramy idyntyfikatory wszystkich obiektów multimedialnych pobranych w ramach tej samej sesji
				alreadyFetchedObjects = from s in alreadyFetchedObjects
										where s.sessionID == selectionParams.SessionId
										select s;

				previousObjectId = from s in alreadyFetchedObjects
									select s.multimediaObjectID;
			}

			var now = DateTime.Now.Date;
			// wyszukujemy obiekty multimedialne spełniające kryteria określone w parametrach metody
            var dev = _repositories.DeviceRepository.Devices
                            .SingleOrDefault(it => it.Id == selectionParams.ID);

            //var dev = _repositories.DeviceRepository.Devices.FirstOrDefault(m => m.Id == selectionParams.ID);

			if (dev == null)
				return null;
			
			var objPCmp = dev.Campaigns.Select(it=> new MMCmp{cmp = it, muliMultimediaObjects = it.MultimediaObjects});

			ICollection<ExtMM> obj = new Collection<ExtMM>();
			
			foreach (var a in objPCmp.SelectMany(it => it.muliMultimediaObjects.Select(o => new ExtMM
			{
				Campaigns = o.Campaigns,
				cmp = it.cmp,
				Contents = o.Contents,
				FileName = o.FileName,
				Id = o.Id,
				Url = o.Url,
				Type = o.Type,
				Name = o.Name,
				Thumbnail = o.Thumbnail,
				Statistics = o.Statistics,
				TypeId = o.TypeId,
				User = o.User,
				MimeType = o.MimeType,
				UserId = o.UserId
			}))
			.Where(a => (dev.Categories.Count() != dev.Categories.Except(a.cmp.Categories).Count()|| dev.Categories.Count == 0) &&
			a.cmp.IsActive && a.cmp.StartDate <= now && a.cmp.EndDate >= now && a.cmp.AdPoints > 0 &&
			a.TypeId == dev.TypeId)) 
			
			{
				obj.Add(a);
			}

			if (!obj.Any())
			{
				foreach (var a in _repositories.CampaignRepository.Campaigns.SelectMany(it => it.MultimediaObjects.Select(o => new ExtMM
				{
					Campaigns = o.Campaigns,
					cmp = it,
					Contents = o.Contents,
					FileName = o.FileName,
					Id = o.Id,
					Url = o.Url,
					Type = o.Type,
					Name = o.Name,
					Thumbnail = o.Thumbnail,
					Statistics = o.Statistics,
					TypeId = o.TypeId,
					User = o.User,
					MimeType = o.MimeType,
					UserId = o.UserId
				})).Where(a => (dev.Categories.Count() != dev.Categories.Except(a.cmp.Categories).Count() || dev.Categories.Count == 0) && dev.TypeId == a.TypeId))
				{
					obj.Add(a);
				}
			}

			var availableObjects = (
				from m in obj
				select new
				{
					multimediObject = m,
					priorityCode = m.cmp.Priority.Code,
					width = m.Type.Width,
					height = m.Type.Height,
					cmpId = m.cmp.Id
				});
			
			var availableObjectsId = (from mo in availableObjects
									  select mo.multimediObject.Id).Distinct();
			
			if (requestFromNewSession)
			{
				availableObjects = (from s in availableObjects
									select s).Distinct().OrderByDescending(x => x.priorityCode);
			}
			else
			{
				if (NotFetchedObjectsExist(previousObjectId, availableObjectsId))
				{
					// jeszcze nie wszystkie obiekty zostaly pobrane, wybieramy tego, ktory nie byl jeszcze pobrany
					availableObjects = (from s in availableObjects
										where !previousObjectId.Contains(s.multimediObject.Id)
										select s).Distinct().OrderByDescending(x => x.priorityCode);
				}
				else if (previousObjectId.Any())
				{
					// wszystkie mozliwe do pobrania obiekty zostały już pobrane, więc rozpoczynamy pobieranie od nowa

					// sprawdzamy, który z dotychczas pobranych obiektów nie są już dostępne do ponownego pobrania
					var excluded = ExcludedObjects(previousObjectId.ToList(), availableObjectsId.ToList());

					// pogrupuj wszystkie pobrane dotychczas (i możliwe do ponownego pobrania) obiekty wg ilości pobrań
					var previousIdFiltered = previousObjectId.ToList().Where(x => !excluded.Contains(x));
					var groupedByCount = previousIdFiltered.GroupBy(x => x)
														 .Select(group => new
														 {
															 Id = group.Key,
															 Count = group.Count()
														 })
														 .OrderBy(x => x.Count);

					// bierzemy ten, który był pobrany najwięcej razy
					var max = groupedByCount.LastOrDefault().Count;
					// bierzyemy te, które były pobrane mniej razy
					var a = groupedByCount.Where(x => x.Count < max);
					if (!a.Any() || (a.Count() == 1 && a.FirstOrDefault().Id == previousIdFiltered.FirstOrDefault()))
					{
						// jeśli wszystkie byly pobrane tyle samo razy, to znaczy, że rozpoczynamy nową
						// rundę pobierania obiektów od początku
						a = groupedByCount;
					}

					var previousObject = (from n in a
										  join m in availableObjects on n.Id equals m.multimediObject.Id
										  where n.Id == previousIdFiltered.FirstOrDefault()
										  orderby n.Count
										  orderby m.priorityCode descending
										  orderby m.multimediObject.Id
										  select m).Distinct();

					// jesli w międzyczasie doszły nowe obiekty to najpierw je wyświetlamy, a kiedy ich ilość wyświetleń będzie
					// taka sama jak dotychczas wyświetlanych obiektów, to w sortujemy od najwyższego do najniższego priorytetu
					availableObjects = (from n in a
										join m in availableObjects on n.Id equals m.multimediObject.Id
										where n.Id != previousIdFiltered.FirstOrDefault()
										orderby n.Count
										orderby m.priorityCode descending
										orderby m.multimediObject.Id
										select m).Distinct();

					var fetchedCount = alreadyFetchedObjects.Count();
					if (!availableObjects.Any() && fetchedCount > 0)
					{
						availableObjects = previousObject;
					}
				}
			}

			if (availableObjects.Any())
			{
				var result = availableObjects.FirstOrDefault();

				if (result != null)
				{
					resultFile = new AdFile
					{
						Name = result.multimediObject.Name,
						MimeType = result.multimediObject.MimeType,
						ID = result.multimediObject.Id,
						Width = result.width,
						Height = result.height,
						StatusCode = result.priorityCode,
						URL = result.multimediObject.Url,
						CmpId = result.multimediObject.cmp.Id
					};
				}
			}

			return resultFile;
		}
		
		public class MMCmp
		{
			public ICollection<MultimediaObject> muliMultimediaObjects;
			public Campaign cmp;
		}

		public class ExtMM : MultimediaObject
		{
			public Campaign cmp;
		}

		/// <summary>
		/// Tworzy zestaw informacji niezbędnych do odnotowania w statystykach informacji o requeście klienta i o wysłanym do niego w odpowiedzi obiekcie multimedialnym.
		/// </summary>
		/// <param name="selectionParams">Parametry opisujące request, na podstawie których został wybrany odpowiedni obiekt multimedialny</param>
		/// <param name="multimediaObject">Wybrany obiekt multimedialny (który zostanie zwrócony klientowi jako odpowiedź na jego request)</param>
		/// <param name="clicked">Stat wpisu - przekierowanie / wyswietlenie</param>
		public StatisticsEntry CreateStatisticsEntry(MultimediaObjectsSelectionParams selectionParams, AdFile multimediaObject, bool clicked = false)
		{
			if (selectionParams == null || multimediaObject == null)
				return null;

			var result = new StatisticsEntry();
			var userId = _repositories.DeviceRepository.Devices.Single(it => it.Id == selectionParams.ID).UserId;
			var statistics = new Statistic
			{
				DeviceId = selectionParams.ID,
				UserId = userId,
				RequestDate = selectionParams.RequestDate,
				ResponseDate = DateTime.Now,
				SessionId = selectionParams.SessionId,
				Data1 = selectionParams.Data0,
				Data2 = selectionParams.Data1,
				Data3 = selectionParams.Data2,
				Data4 = selectionParams.Data3,
				MultimediaObjectId = multimediaObject.ID,
				RequestSource = selectionParams.RequestSource,
				CampaignId = multimediaObject.CmpId,
				RequestIP = selectionParams.RequestIP
			};
			result.Statistics = statistics;

			var now = DateTime.Now;
			// szukamy kampanii, do których należy obiekt multimedialny
			var mmObject = _repositories.MultimediaObjectRepository.MultimediaObjects.Single(it => it.Id == multimediaObject.ID);

			var cmps = mmObject.Campaigns.Select(it => new SelectedMultimediaObjectCampaign
			{
				CampaignId = it.Id,
				IsActiveCampaign = it.IsActive,
				PriorityCode = it.Priority.Code,
				CampaignStartDate = it.StartDate,
				CampaignEndDate = it.EndDate
			});

			var campaignsId = cmps.Select(it => it.CampaignId);

			// bierzemy tylko te kategorie, do których należą wybrane kampanie
			var categories = mmObject.Campaigns
				.SelectMany(it => it.Categories)
				.Distinct();

			result.Categories = categories.Select(it => new {it.Code, it.Id}).ToDictionary(it => it.Code, it => it.Id);

			var cmp = mmObject.Campaigns.FirstOrDefault();
			if (cmp != null)
			{
				result.Statistics.AdPoints = clicked ? cmp.ClickValue : cmp.ViewValue;
			}
			result.Statistics.Clicked = clicked;
			return result;
		}

		/// <summary>
		/// Zapisuje w statystykach informacje o requeście klienta i o wysłanym do niego w odpowiedzi obiekcie multimedialnym.
		/// </summary>
		/// <param name="selectionParams">Parametry opisujące request, na podstawie których został wybrany odpowiedni obiekt multimedialny</param>
		/// <param name="multimediaObject">Wybrany obiekt multimedialny (który zostanie zwrócony klientowi jako odpowiedź na jego request)</param>
		/// <param name="statisticsEntry">Wpis statystyk</param>
		public void SaveStatisticsEntry(MultimediaObjectsSelectionParams selectionParams, AdFile multimediaObject, StatisticsEntry statisticsEntry)
		{
			if (selectionParams == null || multimediaObject == null)
				return;

			if (statisticsEntry == null)
			{
				throw new Exception("Nie udało się zapisać do statystyk informacji o requeście, ponieważ nie powiodło się generowanie takiego obiektu.");
			}

			try
			{
				var host = _repositories.UserRepository.Users.First(it => it.Devices.Any(dev=> dev.Id == selectionParams.ID));
				var cmpid = multimediaObject.CmpId;
				var campUp = _repositories.CampaignRepository.Campaigns.First(it => it.Id == cmpid);
				var cmpUserId = campUp.UserId;
				var user = _repositories.UserRepository.Users.First(it => it.Id == cmpUserId);
				if (user.Role.RoleType != "Admin")
				{
					campUp.AdPoints -= statisticsEntry.Statistics.Clicked ? campUp.ClickValue : campUp.ViewValue;
					_repositories.CampaignRepository.SaveCampaign(campUp, true);
				}
				host.AdPoints += (statisticsEntry.Statistics.Clicked ? campUp.ClickValue : campUp.ViewValue) * host.Role.Commission * (decimal)0.01;
				_repositories.UserRepository.Save(host);
			}
			catch (Exception)
			{
				throw new Exception("Nie udało się wykonć zmian AdPoints'ów");
			}


			var response = _repositories.StatisticsRepository.Save(statisticsEntry);

			if (response.Accepted)
				return;

			var fullMessage = new List<string>();

			if (response.Errors != null && response.Errors.Count > 0)
			{
				fullMessage.AddRange(response.Errors.Select(e => e.Message));
			}

			var thrownError = "Nie udało się zapisać do statystyk informacji o requeście. Nie można przesłać obiektu multimedialnego.";
			if (fullMessage.Count <= 0)
				throw new Exception(thrownError);
			thrownError += (Environment.NewLine + Environment.NewLine) + string.Join(Environment.NewLine, fullMessage);

			fullMessage.Clear();

			throw new Exception(thrownError);
		}

		#endregion - Public methods -

		#region - Private methods -

		/// <summary>
		/// Sprawdza, czy zostały już pobrane i wyświetlone wszystkie obiekty multimedialne, czy może wciąż istnieją takie,
		/// które nie były jeszcze pobrane
		/// </summary>
		/// <param name="fetchedObjects">Lista identyfikatorów dotychczas pobranych/wyświetlonych obiektów multimedialnych</param>
		/// <param name="allObjects">Lista identyfikatorów wszystkich dostępnych dla danej sesji obiektów multimedialnych</param>
		/// <returns>TRUE - jeśli istnieją jeszcze obiekty, które nie były jeszcze pobrane, FALSE - w przeciwnym razie</returns>
		private static bool NotFetchedObjectsExist(IEnumerable<int> fetchedObjects, IEnumerable<int> allObjects)
		{
			return allObjects.Any(i => !fetchedObjects.Contains(i));
		}

		/// <summary>
		/// Sprawdza, które obiekty z listy dotychczas pobranych plików,
		/// nie są już dostępne do pobrania (bo np. kampania przestała już być aktywna w związku z czym nie można pobierać obiektów, które były do niej przypisane)
		/// </summary>
		/// <param name="fetchedObjects">Lista dotychczas pobranych obiektów</param>
		/// <param name="allObjects">Lista wszystkich dostępnych - na chwilę obecną - obiektów</param>
		/// <returns>Lista obiektów, które nie są już dostępne do pobrania</returns>
		private static List<int> ExcludedObjects(IEnumerable<int> fetchedObjects, IEnumerable<int> allObjects)
		{
			var result = new List<int>();

			if (allObjects != null && fetchedObjects != null)
			{
				result.AddRange(fetchedObjects.Where(i => !allObjects.Contains(i)));
			}

			return result;
		}

		/// <summary>
		/// Usuwa z listy wszystkie obiekty przekazane w parametrze
		/// </summary>
		/// <param name="previousObjectID">Oryginalna lista, z której mają zostać usunięte określone obiekty</param>
		/// <param name="excluded">Lista obiektów do usunięcia z oryginalnej listy</param>
		/// <returns>Zredukowana oryginalna lista obiektów</returns>
		private static List<int> RemoveExcludedObjects(List<int> previousObjectID, IEnumerable<int> excluded)
		{
			foreach (var i in excluded)
			{
				if (previousObjectID.Contains(i))
					previousObjectID.Remove(i);
			}

			return previousObjectID;
		}

		#endregion - Private methods -
	}
}