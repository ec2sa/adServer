using ADServerDAL.Abstract;
using ADServerDAL.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdServerUnitTests
{
    /// <summary>
    /// Grupa testów weryfikujących poprawność algorytmu wybierania obiektu oraz zestawień
    /// </summary>
    [TestClass]
    public class MultimediaObjectChooserUnitTests
    {
        #region - Fields -
        private static bool FILESTREAM_OPTION = false;
        private List<AdFile> fetchedFiles;
        AdFile firstObject = null;
        #endregion

        #region - Test methods -
        /// <summary>
        /// Weryfikacja kontrolera obiektów multimedialnych
        /// </summary>
        [TestMethod]
        public void RequestParamsAreCompleteAndOK()
        {
            var repositories = CreateMocks();
            ///zmienna przechowująca poprzednio pobrany obiekt
            AdFile previousMultimediaObject = null;
            ///zmienna przechowująca poprzednio wygenerowaną statystykę
            StatisticsEntry previousStatisticsEntry = null;
            ///request dla metody wyszukującej obiekt multimedialny
            var request = CreateRequest();
            var itemCount = 4;

            for (var i = 0; i < itemCount; i++)
            {
                CheckMethods(repositories, ref previousMultimediaObject, ref previousStatisticsEntry, request);
            }
        }

        [TestMethod]
        public void RequestParamIsNull()
        {
            ///przypadek, kiedy obiekt requesta jest nullem
            var repositories = CreateMocks();
            var mos = new ADEngineMultimediaSelectionAlgorythm.MultimediaObjectSelection(repositories);
            List<string> validationErrors = null;
            ///wyszukanie i pobranie obiektu do wyświetlenia
            var mo1 = mos.FindMultimediaObject(null, FILESTREAM_OPTION, out validationErrors);

            ///pobranie wygenerowanej statystyki
            var se1 = mos.CreateStatisticsEntry(null, mo1);

            Assert.AreEqual(mo1, null);
            Assert.AreEqual(se1, null);
        }

        [TestMethod]
        public void RequestParamIsIncomplete()
        {
            ///przypadek, kiedy w obiekcie requesta są wypełnione tylko niezbędne pola
            var request = new ADEngineMultimediaSelectionAlgorythm.MultimediaObjectSelection.MultimediaObjectsSelectionParams();
			//request.CategoryCodes = new string[] { "MED", "MTR" };
			//request.Width = 250;
			//request.Height = 250;
			//request.SessionId = Guid.NewGuid().ToString();

            var repositories = CreateMocks();
            ///zmienna przechowująca poprzednio pobrany obiekt
            AdFile previousMultimediaObject = null;
            ///zmienna przechowująca poprzednio wygenerowaną statystykę
            StatisticsEntry previousStatisticsEntry = null;
            var mos = new ADEngineMultimediaSelectionAlgorythm.MultimediaObjectSelection(repositories);

            var itemCount = 4;

            for (var i = 0; i < itemCount; i++)
            {
                CheckMethods(repositories, ref previousMultimediaObject, ref previousStatisticsEntry, request);
            }
        }

        [TestMethod]
        public void RequestParamIsEmpty()
        {
            ///przypadek, kiedy obiekt requesta, nie ma wypełnionych żadnych pól
            var request = new ADEngineMultimediaSelectionAlgorythm.MultimediaObjectSelection.MultimediaObjectsSelectionParams();
            var repositories = CreateMocks();
            var mos = new ADEngineMultimediaSelectionAlgorythm.MultimediaObjectSelection(repositories);
            List<string> validationErrors = null;
            ///wyszukanie i pobranie obiektu do wyświetlenia
            var mo1 = mos.FindMultimediaObject(request,FILESTREAM_OPTION, out validationErrors);

            ///pobranie wygenerowanej statystyki
            var se1 = mos.CreateStatisticsEntry(request, mo1);

            Assert.AreEqual(mo1, null);
            Assert.AreEqual(se1, null);
        }

        [TestMethod]
        public void NotExistingCategoryRequested()
        {
            ///przypadek, kiedy klient pyta o nieistniejącą kategorię
            var request = new ADEngineMultimediaSelectionAlgorythm.MultimediaObjectSelection.MultimediaObjectsSelectionParams();
			//request.CategoryCodes = new string[] { "gfrued" };
			//request.Width = 250;
			//request.Height = 250;
			//request.SessionId = Guid.NewGuid().ToString();
            var repositories = CreateMocks();
            var mos = new ADEngineMultimediaSelectionAlgorythm.MultimediaObjectSelection(repositories);
            List<string> validationErrors = null;
            ///wyszukanie i pobranie obiektu do wyświetlenia
			var mo1 = mos.FindMultimediaObject(request, FILESTREAM_OPTION, out validationErrors);

            ///pobranie wygenerowanej statystyki
            var se1 = mos.CreateStatisticsEntry(request, mo1);

            Assert.AreEqual(mo1, null);
            Assert.AreEqual(se1, null);
        }

        [TestMethod]
        public void NotExistingSizeRequested()
        {
            ///przypadek, kiedy klient pyta o nieistniejący rozmiar
            var request = new ADEngineMultimediaSelectionAlgorythm.MultimediaObjectSelection.MultimediaObjectsSelectionParams();
			//request.CategoryCodes = new string[] { "MTR" };
			//request.Height = 11111111;
			//request.Width = 2;
            request.SessionId = Guid.NewGuid().ToString();
            var repositories = CreateMocks();
            var mos = new ADEngineMultimediaSelectionAlgorythm.MultimediaObjectSelection(repositories);
            List<string> validationErrors = null;
            ///wyszukanie i pobranie obiektu do wyświetlenia
            var mo1 = mos.FindMultimediaObject(request,FILESTREAM_OPTION,  out validationErrors);

            ///pobranie wygenerowanej statystyki
            var se1 = mos.CreateStatisticsEntry(request, mo1);

            Assert.AreEqual(mo1, null);
            Assert.AreEqual(se1, null);
        }
        #endregion

        #region - Private methods -
        private void CheckMethods(RepositorySet repositories,
                                  ref AdFile previousMultimediaObject,
                                  ref StatisticsEntry previousStatisticsEntry,
                                  ADEngineMultimediaSelectionAlgorythm.MultimediaObjectSelection.MultimediaObjectsSelectionParams request)
        {
            var now = DateTime.Now;
            var mos = new ADEngineMultimediaSelectionAlgorythm.MultimediaObjectSelection(repositories);
            List<string> validationErrors = null;
            ///wyszukanie i pobranie obiektu do wyświetlenia
            var mo1 = mos.FindMultimediaObject(request,FILESTREAM_OPTION, out validationErrors);
            
            ///pobranie wygenerowanej statystyki
            var se1 = mos.CreateStatisticsEntry(request, mo1);
            
            ///zapis statystyki do "bazy"
			//MockData.Statistics.Add(se1.Statistics);
            repositories = CreateMocks();

            if (fetchedFiles == null)
            {
                fetchedFiles = new List<AdFile>();
            }
            ///zapamietujemy otrzymany obiekt
            fetchedFiles.Add(mo1);

            ///czy obiekt nie jest nullem
            Assert.IsNotNull(mo1);
            Assert.IsNotNull(se1);

            if (mo1 != null && se1 != null)
            {
                ///czy zgadza się rozmiar
				//Assert.AreEqual(request.Width, mo1.Width);
				//Assert.AreEqual(request.Height, mo1.Height);
                ///czy zgadza się nośnik
				//Assert.AreEqual(request.Referrer, se1.Statistics.Referrer);
                ///czy kampanie i kategorie nie są nullem
                Assert.IsNotNull(se1.SelectedMultimediaObjectCampaigns);
                Assert.IsNotNull(se1.Categories);
                ///czy obiekt jest różny od poprzednio pobranego obiektu
                if (previousMultimediaObject != null)
                {
                    Assert.AreNotEqual(mo1.ID, previousMultimediaObject.ID);
                }

                if (se1.SelectedMultimediaObjectCampaigns != null)
                {
                    foreach (var c in se1.SelectedMultimediaObjectCampaigns)
                    {
                        Assert.IsTrue(c.IsActiveCampaign);
                        Assert.IsTrue(c.CampaignStartDate <= now);
                        Assert.IsTrue(c.CampaignEndDate >= now);
                    }
                }

                if (se1.Categories != null)
                {
					//if (request.CategoryCodes.Length <= se1.Categories.Count)
					//{
					//	for (var i = 0; i < request.CategoryCodes.Length; i++)
					//	{
					//		Assert.IsTrue(se1.Categories.ContainsKey(request.CategoryCodes[i]));
					//	}
					//}
					//else
					//{
					//	foreach (var c in se1.Categories)
					//	{
					//		Assert.IsTrue(request.CategoryCodes.ToList().Contains(c.Key));
					//	}
					//}
                }

                if (firstObject == null)
                {
                    firstObject =  mo1;
                }
                else
                {
                    var previousStatus = previousStatisticsEntry.SelectedMultimediaObjectCampaigns.Max(x=>x.PriorityCode);
                    var currentStatus = se1.SelectedMultimediaObjectCampaigns.Max(x=>x.PriorityCode);
                    if (mo1.ID != firstObject.ID)
                    {
                        Assert.IsTrue(previousStatus >= currentStatus);
                    }
                }

                previousMultimediaObject = mo1;
                previousStatisticsEntry = se1;
            }
        }

        private ADEngineMultimediaSelectionAlgorythm.MultimediaObjectSelection.MultimediaObjectsSelectionParams CreateRequest()
        {
            var request = new ADEngineMultimediaSelectionAlgorythm.MultimediaObjectSelection.MultimediaObjectsSelectionParams();
			//request.FirstName = "Jan";
			//request.LastName = "Kowalski";
			//request.CategoryCodes = new string[] { "MED", "MTR" };
			//request.Width = 250;
			//request.Height = 250;
			//request.Referrer = "UnitTestsApp";
			//request.Pesel = "1234567890";
			//request.Email = "test@test.test";
			//request.CompanyName = "Querilogic";
			//request.AdditionalInfo = "info dodatkowe";
			//request.SessionId = Guid.NewGuid().ToString();
            return request;
        }

        private RepositorySet CreateMocks()
        {
            /// mock dla statystyk            
            Mock<IStatisticRepository> statisticsMock = new Mock<IStatisticRepository>();
			//statisticsMock.Setup(m => m.Statistics).Returns((MockData.Statistics).AsQueryable());

            /// mock dla obiektów typu Statistics_Campaign           
            Mock<IStatistics_CampaignRepository> statistics_campaignMock = new Mock<IStatistics_CampaignRepository>();
            statistics_campaignMock.Setup(m => m.Statistics_Campaign).Returns((MockData.Statistics_Campaign).AsQueryable());

            /// mock dla obiektów typu Statistics_Category            
            Mock<IStatistics_CategoryRepository> statistics_CategoryMock = new Mock<IStatistics_CategoryRepository>();
            statistics_CategoryMock.Setup(m => m.Statistics_Category).Returns((MockData.Statistics_Category).AsQueryable());

            Mock<ITypeRepository> typeMock = new Mock<ITypeRepository>();
           // typeMock.Setup(m => m.Types).Returns((MockData.Types).AsQueryable());

            Mock<ICategoryRepository> categoryeMock = new Mock<ICategoryRepository>();
           // categoryeMock.Setup(m => m.Categories).Returns((MockData.Categories).AsQueryable());

            /// mock dla priorytetów
            Mock<IPriorityRepository> priorityMock = new Mock<IPriorityRepository>();
            //priorityMock.Setup(m => m.Priorities).Returns((MockData.Priorities).AsQueryable());

            /// mock dla kampanii            
            Mock<ICampaignRepository> campaignMock = new Mock<ICampaignRepository>();
           // campaignMock.Setup(m => m.Campaigns).Returns((MockData.Campaigns).AsQueryable());

            /// mock dla obiektów multimedialnych            
            Mock<IMultimediaObjectRepository> multimediaObjectMock = new Mock<IMultimediaObjectRepository>();
            //multimediaObjectMock.Setup(m => m.MultimediaObjects).Returns((MockData.Objects).AsQueryable());

            /// mock dla obiektów typu MultimediaObject_Campaign            
            Mock<IMultimediaObject_CampaignRepository> multimediaObject_CampaignMock = new Mock<IMultimediaObject_CampaignRepository>();
            multimediaObject_CampaignMock.Setup(m => m.MultimediaObject_Campaign).Returns((MockData.ObjectsCampaigns).AsQueryable());

            /// mock dla obiektów typu Campaign_Category            
            Mock<ICampaign_CategoryRepository> campaign_CategoryMock = new Mock<ICampaign_CategoryRepository>();
            campaign_CategoryMock.Setup(m => m.Campaign_Category).Returns((MockData.CampaignsCategories).AsQueryable());

            var repositories = new RepositorySet();

            repositories.CategoryRepository = categoryeMock.Object;
            repositories.CampaignRepository = campaignMock.Object;
            repositories.MultimediaObjectRepository = multimediaObjectMock.Object;
            repositories.PriorityRepository = priorityMock.Object;
            repositories.StatisticsRepository = statisticsMock.Object;
            repositories.TypeRepository = typeMock.Object;

            return repositories;
        } 
        #endregion
    }
}
