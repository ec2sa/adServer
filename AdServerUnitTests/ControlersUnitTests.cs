using ADServerDAL.Abstract;
using ADServerDAL.Filters;
using ADServerManagementWebApplication.Controllers;
using ADServerManagementWebApplication.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Web.Mvc;

namespace AdServerUnitTests
{

    /// <summary>
    /// Grupa testów do weryfikacji kontrolerów
    /// </summary>
    [TestClass]
    public class ControlersUnitTests
    {
        /// <summary>
        /// Weryfikacja kontrolera kampanii
        /// </summary>
        [TestMethod]
        public void CampaignsControllerTestMethod()
        {
            /// mock dla priorytetów
            Mock<IPriorityRepository> priorityMock = new Mock<IPriorityRepository>();
           // priorityMock.Setup(m => m.Priorities).Returns((MockData.Priorities).AsQueryable());

            /// mock dla kampanii            
            Mock<ICampaignRepository> campaignMock = new Mock<ICampaignRepository>();
           // campaignMock.Setup(m => m.Campaigns).Returns((MockData.Campaigns).AsQueryable());

			/// mock dla użytkowników            
			Mock<IUsersRepository> usersMock = new Mock<IUsersRepository>();
			//usersMock.Setup(m => m.Users).Returns((MockData.Users).AsQueryable());

            /// Utworzenie kontrolera kampanii
            CampaignController campaignController = new CampaignController(campaignMock.Object, priorityMock.Object, usersMock.Object);
            campaignController.ItemsPerPage = 6;

            /// Wywołanie akcji Index
            ViewResult view = (ViewResult)campaignController.Index(2, null, null);

            /// Weryfikacja wyników widoku
            /// Model nie może być null
            Assert.AreNotEqual(null, view.Model);
            CampaignListViewModel model = (CampaignListViewModel)view.Model;

            ///Lista kampani nie może być null
            Assert.AreNotEqual(null, model.Campaigns);

            ///Model musi być typu CampaignListViewModel
            Assert.IsInstanceOfType(model, typeof(CampaignListViewModel));

            ///Widok musi mieć nazwę Index
            Assert.IsTrue(view.ViewName == "Index");

            ///Bieżąca strona musi mieć numer 2
            Assert.AreEqual(2, model.CurrentPage);

            ///Liczba elementów na stronie musi być 6
            Assert.AreEqual(6, model.ItemsPerPage);

            ///Liczba wszystkich znalezionych kampanii musi być równa liczbie obiektów z mock-a
            Assert.AreEqual(campaignMock.Object.Campaigns.Count(), model.NumberOfResults);

            ///Liczba zwróconych użytkownikowi kampanii na bieżącej stronie musi być 6
            //Assert.AreEqual(6, model.Campaigns.Count);

            /// Wywołanie akcji listy kampanii
			//RedirectToRouteResult redirection = (RedirectToRouteResult)campaignController.List(new CampaignListViewModel
			//	{
			//		//Filters = new CampaignListViewModelFilter
			//		//{
			//		//	FilterActive = false
			//		//}
			//	});

            /// Weryfikacja wyników
            /// Przekierowanie nie może być null-em
			//Assert.IsNotNull(redirection);

			/////RouteValues nie mogą być null-em
			//Assert.IsNotNull(redirection.RouteValues);
			//Assert.IsTrue(redirection.RouteValues.Count > 0);

			/////RouteValues muszą zawierać akcję o nazwie Index
			//Assert.IsTrue(redirection.RouteValues["action"].ToString() == "Index");

            ///Wywołanie metody GET do pobierania listy kampanii
            view = (ViewResult)campaignController.Index(null, null, null);
            model = (CampaignListViewModel)view.Model;

            ///Lista kampanii musi wynosić 4 (na stronie)
          //  Assert.AreEqual(4, model.Campaigns.Count);

            ///Wywołanie akcji z filtrowaniem
			//redirection = (RedirectToRouteResult)campaignController.List(new CampaignListViewModel
			//{
			//	//Filters = new CampaignListViewModelFilter
			//	//{
			//	//	FilterActive = true,
			//	//	FilterPriorityId = 1,
			//	//	FilterStartDateFrom = DateTime.Now.AddDays(-8).Date,
			//	//	FilterStartDateTo = DateTime.Now.AddDays(-4).Date
			//	//}
			//});
			//Assert.IsNotNull(redirection);
			//Assert.IsNotNull(redirection.RouteValues);
			//Assert.IsTrue(redirection.RouteValues.Count > 0);
			//Assert.IsTrue(redirection.RouteValues["action"].ToString() == "Index");

            ///Sprawdzenie wyników
            view = (ViewResult)campaignController.Index(null, null, null);
            model = (CampaignListViewModel)view.Model;
        //    Assert.AreEqual(2, model.Campaigns.Count);

            //Sprawdzenie czy zwróciono spodziewane obiekty kampanii
			//Assert.IsTrue(model.Campaigns[0].Name == "Kampania 1");
			//Assert.IsTrue(model.Campaigns[1].Name == "Kampania 2");
        }

        /// <summary>
        /// Weryfikacja kontrolera kategorii
        /// </summary>
        [TestMethod]
        public void CategoriesControllerTestMethod()
        {
            /// mock dla kategorii            
            Mock<ICategoryRepository> categoryMock = new Mock<ICategoryRepository>();
          //  categoryMock.Setup(m => m.Categories).Returns((MockData.Categories).AsQueryable());

            /// Utworzenie kontrolera kampanii
            //Camp//aignCategoriesController categoryController = new CampaignCategoriesController(categoryMock.Object);
           // categoryController.ItemsPerPage = 1;

            int mocCategoriesCount = categoryMock.Object.Categories.Count();

            /// Wywołanie akcji Index
            //ViewResult view = (ViewResult)categoryController.Index(mocCategoriesCount - 1, null, null);

            /// Weryfikacja wyników
           // Assert.AreNotEqual(null, view.Model);
           // CampaignCategoriesListViewModel model = view.Model as CampaignCategoriesListViewModel;
			//Assert.IsInstanceOfType(model, typeof(CampaignCategoriesListViewModel));
			//Assert.IsTrue(view.ViewName == "Index");
			//Assert.AreEqual(mocCategoriesCount - 1, model.CurrentPage);
			//Assert.IsTrue(model.ItemsPerPage == 1);
			//Assert.IsTrue(model.TotalPages == model.NumberOfResults);
			////Assert.AreEqual(model.Categories[0].Name, "Motoryzacja");

			//categoryController.ItemsPerPage = 10;

            /// Wywołanie akcji listy kategorii
			//RedirectToRouteResult redirection = (RedirectToRouteResult)categoryController.List(new CampaignCategoriesListViewModel
			//{
			//	//Filters = new CampaignCategoriesListViewModelFilter
			//	//{
			//	//	FilterCode = "KAT3"
			//	//}
			//});
            /// Weryfikacja wyników
			//Assert.IsNotNull(redirection);
			//Assert.IsNotNull(redirection.RouteValues);
			//Assert.IsTrue(redirection.RouteValues.Count > 0);
			//Assert.IsTrue(redirection.RouteValues["action"].ToString() == "Index");

			//view = (ViewResult)categoryController.Index(null, null, null);
			//model = (CampaignCategoriesListViewModel)view.Model;
			//Assert.AreEqual(1, model.Categories.Count());

            ///Wywołanie akcji z filtrowaniem
			//redirection = (RedirectToRouteResult)categoryController.List(new CampaignCategoriesListViewModel
			//{
			//	Filters = new CampaignCategoriesListViewModelFilter
			//	{
			//		FilterName = "Kateg"
			//	}
			//});
			//Assert.IsNotNull(redirection);
			//Assert.IsNotNull(redirection.RouteValues);
			//Assert.IsTrue(redirection.RouteValues.Count > 0);
			//Assert.IsTrue(redirection.RouteValues["action"].ToString() == "Index");

            ///Sprawdzenie wyników
			//view = (ViewResult)categoryController.Index(null, null, null);
			//model = (CampaignCategoriesListViewModel)view.Model;
            //Assert.AreEqual(4, model.Categories.Count);
        }

        /// <summary>
        /// Weryfikacja kontrolera priorytetów
        /// </summary>
        [TestMethod]
        public void PrioritiesControllerTestMethod()
        {
			///// mock dla piorytetów            
			//Mock<IPriorityRepository> priorityMock = new Mock<IPriorityRepository>();
			//priorityMock.Setup(m => m.Priorities).Returns((MockData.Priorities).AsQueryable());

			///// Utworzenie kontrolera priorytetu
			//CampaignPrioritiesController priorityController = new CampaignPrioritiesController(priorityMock.Object);
			//priorityController.ItemsPerPage = 20;

			//int mocPrioritiesCount = priorityMock.Object.Priorities.Count();

			///// Wywołanie akcji Index
			//ViewResult view = (ViewResult)priorityController.Index(null, null, null);

			///// Weryfikacja wyników
			//Assert.AreNotEqual(null, view.Model);
			//CampaignPrioritiesListViewModel model = view.Model as CampaignPrioritiesListViewModel;
			//Assert.IsInstanceOfType(model, typeof(CampaignPrioritiesListViewModel));
			//Assert.IsTrue(view.ViewName == "Index");

			//Assert.AreEqual(1, model.CurrentPage);
			//Assert.IsTrue(model.TotalPages == 1);
			//Assert.AreEqual(model.Priorities[0].Name, "Normalny");
			//Assert.AreEqual(model.Priorities[1].Name, "Średni");
			//Assert.AreEqual(model.Priorities[2].Name, "Wysoki");

			//Assert.IsTrue(model.NumberOfResults == mocPrioritiesCount);


			///// Wywołanie akcji listy priorytetów
			//RedirectToRouteResult redirection = (RedirectToRouteResult)priorityController.List(new CampaignPrioritiesListViewModel
			//{
			//	Filters = new CampaignPrioritiesListViewModelFilter
			//	{
			//		FilterCode = 3
			//	}
			//});
			///// Weryfikacja wyników
			//Assert.IsNotNull(redirection);
			//Assert.IsNotNull(redirection.RouteValues);
			//Assert.IsTrue(redirection.RouteValues.Count > 0);
			//Assert.IsTrue(redirection.RouteValues["action"].ToString() == "Index");

			//view = (ViewResult)priorityController.Index(null, null, null);
			//model = (CampaignPrioritiesListViewModel)view.Model;
			//Assert.AreEqual(1, model.Priorities.Count);

			/////Wywołanie akcji z filtrowaniem
			//redirection = (RedirectToRouteResult)priorityController.List(new CampaignPrioritiesListViewModel
			//{
			//	Filters = new CampaignPrioritiesListViewModelFilter
			//	{
			//		FilterName = "Śr"
			//	}
			//});
			//Assert.IsNotNull(redirection);
			//Assert.IsNotNull(redirection.RouteValues);
			//Assert.IsTrue(redirection.RouteValues.Count > 0);
			//Assert.IsTrue(redirection.RouteValues["action"].ToString() == "Index");

			/////Sprawdzenie wyników
			//view = (ViewResult)priorityController.Index(null, null, null);
			//model = (CampaignPrioritiesListViewModel)view.Model;
			//Assert.AreEqual(1, model.Priorities.Count);
			//Assert.IsTrue(model.Priorities[0].Name == "Średni" && model.Priorities[0].Code == 2);
        }

        /// <summary>
        /// Weryfikacja kontrolera typów obiektów multimedialnych
        /// </summary>
        [TestMethod]
        public void TypesControllerTestMethod()
        {
            /// mock dla typów            
		   // Mock<ITypeRepository> typesMock = new Mock<ITypeRepository>();
		   // //typesMock.Setup(m => m.Types).Returns((MockData.Types).AsQueryable());

		   // /// Utworzenie kontrolera typów
		   // MultimediaTypesController typeController = new MultimediaTypesController(typesMock.Object);
		   // typeController.ItemsPerPage = 2;

		   // int mocTypesCount = typesMock.Object.Types.Count();

		   // /// Wywołanie akcji Index
		   // ViewResult view = (ViewResult)typeController.Index(3, null, null);

		   // /// Weryfikacja wyników
		   // Assert.AreNotEqual(null, view.Model);
		   // MultimediaTypesListViewModel model = view.Model as MultimediaTypesListViewModel;
		   // Assert.IsInstanceOfType(model, typeof(MultimediaTypesListViewModel));
		   // Assert.IsTrue(view.ViewName == "Index");
		   // Assert.AreEqual(3, model.CurrentPage);
		   // //ssert.IsTrue(model.MultimediaTypes.Count == 2);
		   //// Assert.AreEqual(model.MultimediaTypes[0].Name, "Prostokąt x3");


            ///Wywołanie akcji z filtrowaniem
			//RedirectToRouteResult redirection = (RedirectToRouteResult)typeController.List(new MultimediaTypesListViewModel
			//{
			//	Filters = new MultimediaTypesListViewModelFilter
			//	{
			//		FilterWidth = 80,
			//		FilterHeight = 120
			//	}
			//});
			//Assert.IsNotNull(redirection);
			//Assert.IsNotNull(redirection.RouteValues);
			//Assert.IsTrue(redirection.RouteValues.Count > 0);
			//Assert.IsTrue(redirection.RouteValues["action"].ToString() == "Index");

            ///Sprawdzenie wyników
			//view = (ViewResult)typeController.Index(null, null, null);
			//model = (MultimediaTypesListViewModel)view.Model;
          //  Assert.AreEqual(1, model.MultimediaTypes.Count);
           // Assert.IsTrue(model.MultimediaTypes[0].DescriptorWithName == string.Format("{0} ({1}x{2})", "Prostokąt x1", 80, 120));
        }

        /// <summary>
        /// Weryfikacja kontrolera obiektów multimedialnych
        /// </summary>
        [TestMethod]
        public void ObjectsControllerTestMethod()
        {
            /// mock dla obiektów            
            Mock<IMultimediaObjectRepository> objectMock = new Mock<IMultimediaObjectRepository>();
           // objectMock.Setup(m => m.MultimediaObjects).Returns((MockData.Objects).AsQueryable());

			/// mock dla typów
            Mock<ITypeRepository> typeMock = new Mock<ITypeRepository>();
           // typeMock.Setup(m => m.Types).Returns((MockData.Types).AsQueryable());

			/// mock dla użytkowników
			Mock<IUsersRepository> usersMock = new Mock<IUsersRepository>();
			//usersMock.Setup(m => m.Users).Returns((MockData.Users).AsQueryable());

            /// Utworzenie kontrolera obiektów
            MultimediaObjectsController objectRepository = new MultimediaObjectsController(objectMock.Object, typeMock.Object, usersMock.Object);
            objectRepository.ItemsPerPage = 10;

            int mocObjectsCount = objectMock.Object.MultimediaObjects.Count();

            /// Wywołanie akcji Index
            ViewResult view = (ViewResult)objectRepository.Index(mocObjectsCount - 1, null, null, null);

            /// Weryfikacja wyników
            Assert.AreNotEqual(null, view.Model);
            MultimediaObjectsListViewModel model = view.Model as MultimediaObjectsListViewModel;
            Assert.IsInstanceOfType(model, typeof(MultimediaObjectsListViewModel));
            Assert.IsTrue(view.ViewName == "Index");
            Assert.AreEqual(mocObjectsCount - 1, 15);
            Assert.IsTrue(model.ItemsPerPage == 10);
            Assert.IsTrue(model.TotalPages == 2);
           // Assert.AreEqual(model.MultimediaObjects[0].Name, "autobus");

			///// Wywołanie akcji listy priorytetów
			//RedirectToRouteResult redirection = (RedirectToRouteResult)objectRepository.List(new MultimediaObjectsListViewModel
			//{
			//	///Wszystkie obiekty przypisane do typów zawierających w nazwie 'Kwadrat'
			//	Filters = new MultimediaObjectListViewModelFilter
			//	{
			//		FilterType = "Kwadrat"
			//	}
			//});
			///// Weryfikacja wyników
			//Assert.IsNotNull(redirection);
			//Assert.IsNotNull(redirection.RouteValues);
			//Assert.IsTrue(redirection.RouteValues.Count > 0);
			//Assert.IsTrue(redirection.RouteValues["action"].ToString() == "Index");

            view = (ViewResult)objectRepository.Index(null, null, null, null);
            model = (MultimediaObjectsListViewModel)view.Model;

            ///W mock-u występują 4 elementy pasujące do warunku
      //      Assert.AreEqual(10, model.MultimediaObjects.Count);
         
            ///Powiązanie obiektów z kampaniami
			//objectMock.Setup(m => m.ObjectsToCampaign(7)).Returns(
			//	(from o in MockData.Objects
			//	 join cc in MockData.ObjectsCampaigns on o.ID equals cc.MultimediaObjectId
			//	 where cc.CampaignId == 7
			//	 select o.ID).ToList());
			//Assert.AreEqual(objectMock.Object.ObjectsToCampaign(7).Count, 3);

        }
    }
}
