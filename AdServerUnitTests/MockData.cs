using ADServerDAL.Entities.Presentation;
using ADServerDAL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdServerUnitTests
{
    /// <summary>
    /// Klasa dostarczająca sztycznych danych na potrzeby testów
    /// </summary>
    public static class MockData
    {
		#region Użytkownicy
		private static List<UserItem> users = null;

		/// <summary>
		/// Użytkownicy
		/// </summary>
		public static List<UserItem> Users
		{
			get
			{
				if (users == null)
				{
					users = new List<UserItem>();
					users.Add(new UserItem
					{
						ID = 1,
						Name = "User1",
						//Role = new RoleItem() { ID = 1, Name = "Admin" }
					});
					users.Add(new UserItem
					{
						ID = 2,
						Name = "User2",
						//Role = new RoleItem() { ID = 2, Name = "User" }
					});
					users.Add(new UserItem
					{
						ID = 3,
						Name = "User3",
						//Role = new RoleItem() { ID = 2, Name = "User" }
					});
				}
				return users;
			}
		}
		#endregion

        #region Kampanie
        private static List<CampaignItem> campaigns = null;

        /// <summary>
        /// Kampanie
        /// </summary>
        public static List<CampaignItem> Campaigns
        {
            get
            {
                if (campaigns == null)
                {
                    campaigns = new List<CampaignItem>();
                    campaigns.Add(new CampaignItem
                        {
                            Id = 1,
                            Name = "Kampania 1",
                            IsActive = true,
                            StartDate = DateTime.Now.AddDays(-5).Date,
                            EndDate = DateTime.Now.AddDays(25).Date,
                            PriorityId = 1
                        });
                    campaigns.Add(new CampaignItem
                    {
                        Id = 2,
                        Name = "Kampania 2",
                        IsActive = true,
                        StartDate = DateTime.Now.AddDays(-7).Date,
                        EndDate = DateTime.Now.AddDays(3).Date,
                        PriorityId = 1
                    });
                    campaigns.Add(new CampaignItem
                    {
                        Id = 3,
                        Name = "Kampania 3",
                        IsActive = true,
                        StartDate = DateTime.Now.AddDays(1).Date,
                        EndDate = DateTime.Now.AddDays(13).Date,
                        PriorityId = 2
                    });
                    campaigns.Add(new CampaignItem
                    {
                        Id = 4,
                        Name = "Kampania 4",
                        IsActive = false,
                        PriorityId = 3
                    });
                    campaigns.Add(new CampaignItem
                    {
                        Id = 5,
                        Name = "Kampania 5",
                        IsActive = true,
                        StartDate = DateTime.Now.AddDays(-30).Date,
                        EndDate = DateTime.Now.AddDays(-11).Date,
                        PriorityId = 2
                    });
                    campaigns.Add(new CampaignItem
                    {
                        Id = 6,
                        Name = "Kampania 6",
                        IsActive = true,
                        PriorityId = 2
                    });
                    campaigns.Add(new CampaignItem
                    {
                        Id = 7,
                        Name = "Kampania 7",
                        IsActive = false,
                        StartDate = DateTime.Now.AddDays(-1).Date,
                        EndDate = DateTime.Now.AddDays(-1).Date,
                        PriorityId = 1
                    });
                    campaigns.Add(new CampaignItem
                    {
                        Id = 8,
                        Name = "Kampania 8",
                        IsActive = true,
                        PriorityId = 3
                    });
                    ///-----
                    campaigns.Add(new CampaignItem
                    {
                        Id = 9,
                        Name = "Autobusy",
                        StartDate = DateTime.Now.AddMonths(-1),
                        EndDate = DateTime.Now.AddMonths(1),
                        IsActive = false,
                        PriorityId = 3
                    });
                    campaigns.Add(new CampaignItem
                    {
                        Id = 10,
                        Name = "Tramwaje",
                        IsActive = true,
                        StartDate = DateTime.Now.AddMonths(-1),
                        EndDate = DateTime.Now.AddMonths(1),
                        PriorityId = 2
                    });
                    campaigns.Add(new CampaignItem
                    {
                        Id = 11,
                        Name = "Auta",
                        IsActive = true,
                        StartDate = DateTime.Now.AddDays(-1),
                        EndDate = DateTime.Now.AddDays(1),
                        PriorityId = 4
                    });
                    campaigns.Add(new CampaignItem
                    {
                        Id = 12,
                        Name = "Lekarze",
                        IsActive = true,
                        StartDate = DateTime.Now.AddMonths(-1),
                        EndDate = DateTime.Now.AddMonths(1),
                        PriorityId = 1
                    });
                    campaigns.Add(new CampaignItem
                    {
                        Id = 13,
                        Name = "Leki",
                        IsActive = true,
                        StartDate = DateTime.Now.AddMonths(1),
                        EndDate = DateTime.Now.AddMonths(2),
                        PriorityId = 2
                    });
                    campaigns.Add(new CampaignItem
                    {
                        Id = 14,
                        Name = "Pielęgniarki",
                        IsActive = false,
                        StartDate = DateTime.Now.AddMonths(-1),
                        EndDate = DateTime.Now.AddMonths(1),
                        PriorityId = 2
                    });
                }
                return campaigns;
            }
        }
        #endregion

        #region Kategorie
        private static List<CategoryItem> categories = null;

        /// <summary>
        /// Kategorie
        /// </summary>
        public static List<CategoryItem> Categories
        {
            get
            {
                if (categories == null)
                {
                    categories = new List<CategoryItem>();
                    categories.Add(new CategoryItem
                        {
                            ID = 1,
                            Code = "KAT1",
                            Name = "Kategoria 1"
                        });
                    categories.Add(new CategoryItem
                    {
                        ID = 2,
                        Code = "KAT2",
                        Name = "Kategoria 2"
                    });
                    categories.Add(new CategoryItem
                    {
                        ID = 3,
                        Code = "KAT3",
                        Name = "Kategoria 3"
                    });
                    categories.Add(new CategoryItem
                    {
                        ID = 4,
                        Code = "KAT4",
                        Name = "Kategoria 4"
                    });
                    categories.Add(new CategoryItem
                    {
                        ID = 5,
                        Code = "MTR",
                        Name = "Motoryzacja"
                    });
                    categories.Add(new CategoryItem
                    {
                        ID = 6,
                        Code = "MED",
                        Name = "Medycyna"
                    });
                }
                return categories;
            }
        }
        #endregion

        #region Priorytety
        private static List<PriorityItem> priorities = null;

        /// <summary>
        /// Priorytety
        /// </summary>
        public static List<PriorityItem> Priorities
        {
            get
            {
                if (priorities == null)
                {
                    priorities = new List<PriorityItem>();
                    priorities.Add(new PriorityItem
                    {
                        Id = 1,
                        Code = 1,
                        Name = "Normalny"
                    });
                    priorities.Add(new PriorityItem
                    {
                        Id = 2,
                        Code = 2,
                        Name = "Średni"
                    });
                    priorities.Add(new PriorityItem
                    {
                        Id = 3,
                        Code = 3,
                        Name = "Wysoki"
                    });
                    priorities.Add(new PriorityItem
                    {
                        Id = 4,
                        Code = 4,
                        Name = "Krytyczny"
                    });
                }
                return priorities;
            }
        }
        #endregion

        #region Typy obiektów
        private static List<MultimediaTypeItem> types = null;

        /// <summary>
        /// Typy
        /// </summary>
        public static List<MultimediaTypeItem> Types
        {
            get
            {
                if (types == null)
                {
                    types = new List<MultimediaTypeItem>();
                    types.Add(new MultimediaTypeItem
                        {
                            ID = 1,
                            Height = 60,
                            Width = 60,
                            Name = "Kwadrat 60"
                        });
                    types.Add(new MultimediaTypeItem
                    {
                        ID = 2,
                        Height = 40,
                        Width = 40,
                        Name = "Kwadrat 40"
                    });
                    types.Add(new MultimediaTypeItem
                    {
                        ID = 3,
                        Height = 120,
                        Width = 80,
                        Name = "Prostokąt x1"
                    });
                    types.Add(new MultimediaTypeItem
                    {
                        ID = 4,
                        Height = 180,
                        Width = 250,
                        Name = "Prostokąt x2"
                    });
                    types.Add(new MultimediaTypeItem
                    {
                        ID = 5,
                        Height = 220,
                        Width = 40,
                        Name = "Prostokąt x3"
                    });
                    types.Add(new MultimediaTypeItem
                    {
                        ID = 6,
                        Height = 250,
                        Width = 250,
                        Name = "Kwadrat 250"
                    });

                }
                return types;
            }
        }
        #endregion

        #region Obiekty multimedialne
        private static List<MultimediaObjectItem> objects = null;

        /// <summary>
        /// Obiekty multimedialne
        /// </summary>
        public static List<MultimediaObjectItem> Objects
        {
            get
            {
                if (objects == null)
                {
                    objects = new List<MultimediaObjectItem>();
                    objects.Add(new MultimediaObjectItem
                        {
                            ID = 1,
                            Name = "Chrysanthemum",
                            FileName = "Chrysanthemum.jpg",
                            Width = Types.First(t => t.ID == 1).Width,
                            Height = Types.First(t => t.ID == 1).Height,
                            TypeId = Types.First(t => t.ID == 1).ID,
                            TypeName = Types.First(t => t.ID == 1).Name
                        });

                    objects.Add(new MultimediaObjectItem
                    {
                        ID = 2,
                        Name = "Desert",
                        FileName = "Desert.jpg",
                        Width = Types.First(t => t.ID == 1).Width,
                        Height = Types.First(t => t.ID == 1).Height,
                        TypeId = Types.First(t => t.ID == 1).ID,
                        TypeName = Types.First(t => t.ID == 1).Name
                    });
                    objects.Add(new MultimediaObjectItem
                    {
                        ID = 3,
                        Name = "Hydrangeas",
                        FileName = "Hydrangeas.jpg",
                        Width = Types.First(t => t.ID == 1).Width,
                        Height = Types.First(t => t.ID == 1).Height,
                        TypeId = Types.First(t => t.ID == 1).ID,
                        TypeName = Types.First(t => t.ID == 1).Name
                    });
                    objects.Add(new MultimediaObjectItem
                    {
                        ID = 4,
                        Name = "Jellyfish",
                        FileName = "Jellyfish.jpg",
                        Width = Types.First(t => t.ID == 2).Width,
                        Height = Types.First(t => t.ID == 2).Height,
                        TypeId = Types.First(t => t.ID == 2).ID,
                        TypeName = Types.First(t => t.ID == 2).Name
                    });
                    objects.Add(new MultimediaObjectItem
                    {
                        ID = 5,
                        Name = "Koala",
                        FileName = "Koala.jpg",
                        Width = Types.First(t => t.ID == 3).Width,
                        Height = Types.First(t => t.ID == 3).Height,
                        TypeId = Types.First(t => t.ID == 3).ID,
                        TypeName = Types.First(t => t.ID == 3).Name
                    });
                    objects.Add(new MultimediaObjectItem
                    {
                        ID = 6,
                        Name = "Lighthouse",
                        FileName = "Lighthouse.jpg",
                        Width = Types.First(t => t.ID == 3).Width,
                        Height = Types.First(t => t.ID == 3).Height,
                        TypeId = Types.First(t => t.ID == 3).ID,
                        TypeName = Types.First(t => t.ID == 3).Name
                    });
                    objects.Add(new MultimediaObjectItem
                    {
                        ID = 7,
                        Name = "Penguins",
                        FileName = "Penguins.jpg",
                        Width = Types.First(t => t.ID == 4).Width,
                        Height = Types.First(t => t.ID == 4).Height,
                        TypeId = Types.First(t => t.ID == 4).ID,
                        TypeName = Types.First(t => t.ID == 4).Name
                    });
                    objects.Add(new MultimediaObjectItem
                    {
                        ID = 8,
                        Name = "Tulips",
                        FileName = "Tulips.jpg",
                        Width = Types.First(t => t.ID == 4).Width,
                        Height = Types.First(t => t.ID == 4).Height,
                        TypeId = Types.First(t => t.ID == 4).ID,
                        TypeName = Types.First(t => t.ID == 4).Name
                    });
                    objects.Add(new MultimediaObjectItem
                    {
                        ID = 9,
                        Name = "Image 12",
                        FileName = "Jellyfish.jpg",
                        Width = Types.First(t => t.ID == 4).Width,
                        Height = Types.First(t => t.ID == 4).Height,
                        TypeId = Types.First(t => t.ID == 4).ID,
                        TypeName = Types.First(t => t.ID == 4).Name
                    });
                    objects.Add(new MultimediaObjectItem
                    {
                        ID = 10,
                        Name = "Image 13",
                        FileName = "Lighthouse.jpg",
                        Width = Types.First(t => t.ID == 5).Width,
                        Height = Types.First(t => t.ID == 5).Height,
                        TypeId = Types.First(t => t.ID == 5).ID,
                        TypeName = Types.First(t => t.ID == 5).Name
                    });
                    //---
                    objects.Add(new MultimediaObjectItem
                    {
                        ID = 11,
                        Name = "autobus",
                        FileName = "autobus.jpg",
                        Width = Types.First(t => t.ID == 6).Width,
                        Height = Types.First(t => t.ID == 6).Height,
                        TypeId = Types.First(t => t.ID == 6).ID,
                        TypeName = Types.First(t => t.ID == 6).Name
                    });
                    objects.Add(new MultimediaObjectItem
                    {
                        ID = 12,
                        Name = "tramwaj",
                        FileName = "tramwaj.jpg",
                        Width = Types.First(t => t.ID == 6).Width,
                        Height = Types.First(t => t.ID == 6).Height,
                        TypeId = Types.First(t => t.ID == 6).ID,
                        TypeName = Types.First(t => t.ID == 6).Name
                    });
                    objects.Add(new MultimediaObjectItem
                    {
                        ID = 13,
                        Name = "auto",
                        FileName = "auto.jpg",
                        Width = Types.First(t => t.ID == 6).Width,
                        Height = Types.First(t => t.ID == 6).Height,
                        TypeId = Types.First(t => t.ID == 6).ID,
                        TypeName = Types.First(t => t.ID == 6).Name
                    });
                    objects.Add(new MultimediaObjectItem
                    {
                        ID = 14,
                        Name = "lekarz",
                        FileName = "lekarz.jpg",
                        Width = Types.First(t => t.ID == 6).Width,
                        Height = Types.First(t => t.ID == 6).Height,
                        TypeId = Types.First(t => t.ID == 6).ID,
                        TypeName = Types.First(t => t.ID == 6).Name
                    });
                    objects.Add(new MultimediaObjectItem
                    {
                        ID = 15,
                        Name = "leki",
                        FileName = "leki.jpg",
                        Width = Types.First(t => t.ID == 6).Width,
                        Height = Types.First(t => t.ID == 6).Height,
                        TypeId = Types.First(t => t.ID == 6).ID,
                        TypeName = Types.First(t => t.ID == 6).Name
                    });
                    objects.Add(new MultimediaObjectItem
                    {
                        ID = 16,
                        Name = "pielegniarka",
                        FileName = "pielegniarka.jpg",
                        Width = Types.First(t => t.ID == 6).Width,
                        Height = Types.First(t => t.ID == 6).Height,
                        TypeId = Types.First(t => t.ID == 6).ID,
                        TypeName = Types.First(t => t.ID == 6).Name
                    });

                    string imageDirectory = new System.IO.FileInfo(typeof(MockData).Assembly.Location).Directory.FullName + "\\Images\\";

                    foreach (MultimediaObjectItem mo in objects)
                    {
                        string imagePath = imageDirectory + mo.FileName;
                        if (System.IO.File.Exists(imagePath))
                        {
                            mo.Mime = GetMimeType(imagePath);
                            ImageProcesorHelper.ResizeImageResult resizeResult = ImageProcesorHelper.ResizeImage(mo.Width, mo.Height, System.IO.File.ReadAllBytes(imagePath), true);
                            mo.Thumbnail = resizeResult.Thumbnail;
                            mo.FileContent = Convert.ToBase64String(resizeResult.ResizedImage);
                        }
                    }
                }
                return objects;
            }
        }
        #endregion

        #region Powiązania kampanie - kategorie
        private static List<Campaign_CategoryItem> campaignsCategories = null;

        public static List<Campaign_CategoryItem> CampaignsCategories
        {
            get
            {
                if (campaignsCategories == null)
                {
                    campaignsCategories = new List<Campaign_CategoryItem>();

                    campaignsCategories.Add(new Campaign_CategoryItem
                    {
                        ID = 1,
                        CampaignId = 1,
                        CategoryId = 1
                    });

                    campaignsCategories.Add(new Campaign_CategoryItem
                    {
                        ID = 2,
                        CampaignId = 1,
                        CategoryId = 2
                    });

                    campaignsCategories.Add(new Campaign_CategoryItem
                    {
                        ID = 3,
                        CampaignId = 1,
                        CategoryId = 4
                    });

                    campaignsCategories.Add(new Campaign_CategoryItem
                    {
                        ID = 3,
                        CampaignId = 2,
                        CategoryId = 1
                    });

                    campaignsCategories.Add(new Campaign_CategoryItem
                    {
                        ID = 4,
                        CampaignId = 2,
                        CategoryId = 4
                    });

                    campaignsCategories.Add(new Campaign_CategoryItem
                    {
                        ID = 5,
                        CampaignId = 3,
                        CategoryId = 2
                    });

                    campaignsCategories.Add(new Campaign_CategoryItem
                    {
                        ID = 6,
                        CampaignId = 3,
                        CategoryId = 3
                    });


                    campaignsCategories.Add(new Campaign_CategoryItem
                    {
                        ID = 7,
                        CampaignId = 4,
                        CategoryId = 1
                    });


                    campaignsCategories.Add(new Campaign_CategoryItem
                    {
                        ID = 8,
                        CampaignId = 4,
                        CategoryId = 2
                    });


                    campaignsCategories.Add(new Campaign_CategoryItem
                    {
                        ID = 9,
                        CampaignId = 4,
                        CategoryId = 3
                    });

                    campaignsCategories.Add(new Campaign_CategoryItem
                    {
                        ID = 10,
                        CampaignId = 4,
                        CategoryId = 4
                    });


                    campaignsCategories.Add(new Campaign_CategoryItem
                    {
                        ID = 11,
                        CampaignId = 5,
                        CategoryId = 2
                    });

                    campaignsCategories.Add(new Campaign_CategoryItem
                    {
                        ID = 12,
                        CampaignId = 5,
                        CategoryId = 3
                    });

                    campaignsCategories.Add(new Campaign_CategoryItem
                    {
                        ID = 13,
                        CampaignId = 6,
                        CategoryId = 1
                    });


                    campaignsCategories.Add(new Campaign_CategoryItem
                    {
                        ID = 14,
                        CampaignId = 7,
                        CategoryId = 1
                    });

                    campaignsCategories.Add(new Campaign_CategoryItem
                    {
                        ID = 15,
                        CampaignId = 9,
                        CategoryId = 5
                    });

                    campaignsCategories.Add(new Campaign_CategoryItem
                    {
                        ID = 16,
                        CampaignId = 10,
                        CategoryId = 5
                    });

                    campaignsCategories.Add(new Campaign_CategoryItem
                    {
                        ID = 17,
                        CampaignId = 11,
                        CategoryId = 5
                    });

                    campaignsCategories.Add(new Campaign_CategoryItem
                    {
                        ID = 18,
                        CampaignId = 12,
                        CategoryId = 6
                    });

                    campaignsCategories.Add(new Campaign_CategoryItem
                    {
                        ID = 19,
                        CampaignId = 13,
                        CategoryId = 6
                    });

                    campaignsCategories.Add(new Campaign_CategoryItem
                    {
                        ID = 20,
                        CampaignId = 14,
                        CategoryId = 6
                    });
                }

                return campaignsCategories;
            }
        }
        #endregion

        #region Powiązania kampanie - obiekty multimedialne
        private static List<MultimediaObject_CampaignItem> objectsCampaigns = null;

        public static List<MultimediaObject_CampaignItem> ObjectsCampaigns
        {
            get
            {
                if (objectsCampaigns == null)
                {
                    objectsCampaigns = new List<MultimediaObject_CampaignItem>();

                    objectsCampaigns.Add(new MultimediaObject_CampaignItem
                        {
                            ID = 1,
                            CampaignId = 1,
                            MultimediaObjectId = 1
                        });

                    objectsCampaigns.Add(new MultimediaObject_CampaignItem
                    {
                        ID = 2,
                        CampaignId = 2,
                        MultimediaObjectId = 1
                    });

                    objectsCampaigns.Add(new MultimediaObject_CampaignItem
                    {
                        ID = 3,
                        CampaignId = 2,
                        MultimediaObjectId = 3
                    });

                    objectsCampaigns.Add(new MultimediaObject_CampaignItem
                    {
                        ID = 4,
                        CampaignId = 3,
                        MultimediaObjectId = 4
                    });

                    objectsCampaigns.Add(new MultimediaObject_CampaignItem
                    {
                        ID = 5,
                        CampaignId = 3,
                        MultimediaObjectId = 5
                    });

                    objectsCampaigns.Add(new MultimediaObject_CampaignItem
                    {
                        ID = 6,
                        CampaignId = 3,
                        MultimediaObjectId = 6
                    });

                    objectsCampaigns.Add(new MultimediaObject_CampaignItem
                    {
                        ID = 7,
                        CampaignId = 4,
                        MultimediaObjectId = 7
                    });

                    objectsCampaigns.Add(new MultimediaObject_CampaignItem
                    {
                        ID = 8,
                        CampaignId = 4,
                        MultimediaObjectId = 8
                    });

                    objectsCampaigns.Add(new MultimediaObject_CampaignItem
                    {
                        ID = 9,
                        CampaignId = 5,
                        MultimediaObjectId = 1
                    });

                    objectsCampaigns.Add(new MultimediaObject_CampaignItem
                    {
                        ID = 10,
                        CampaignId = 6,
                        MultimediaObjectId = 2
                    });

                    objectsCampaigns.Add(new MultimediaObject_CampaignItem
                    {
                        ID = 11,
                        CampaignId = 7,
                        MultimediaObjectId = 3
                    });
                    objectsCampaigns.Add(new MultimediaObject_CampaignItem
                    {
                        ID = 12,
                        CampaignId = 7,
                        MultimediaObjectId = 2
                    });

                    objectsCampaigns.Add(new MultimediaObject_CampaignItem
                    {
                        ID = 13,
                        CampaignId = 7,
                        MultimediaObjectId = 3
                    });

                    objectsCampaigns.Add(new MultimediaObject_CampaignItem
                    {
                        ID = 14,
                        CampaignId = 8,
                        MultimediaObjectId = 8
                    });

                    objectsCampaigns.Add(new MultimediaObject_CampaignItem
                    {
                        ID = 15,
                        CampaignId = 9,
                        MultimediaObjectId = 11
                    });

                    objectsCampaigns.Add(new MultimediaObject_CampaignItem
                    {
                        ID = 16,
                        CampaignId = 10,
                        MultimediaObjectId = 12
                    });

                    objectsCampaigns.Add(new MultimediaObject_CampaignItem
                    {
                        ID = 17,
                        CampaignId = 11,
                        MultimediaObjectId = 13
                    });

                    objectsCampaigns.Add(new MultimediaObject_CampaignItem
                    {
                        ID = 18,
                        CampaignId = 12,
                        MultimediaObjectId = 14
                    });

                    objectsCampaigns.Add(new MultimediaObject_CampaignItem
                    {
                        ID = 19,
                        CampaignId = 13,
                        MultimediaObjectId = 15
                    });

                    objectsCampaigns.Add(new MultimediaObject_CampaignItem
                    {
                        ID = 20,
                        CampaignId = 14,
                        MultimediaObjectId = 16
                    });
                    //==
                    objectsCampaigns.Add(new MultimediaObject_CampaignItem
                    {
                        ID = 21,
                        CampaignId = 10,
                        MultimediaObjectId = 16
                    });
                    objectsCampaigns.Add(new MultimediaObject_CampaignItem
                    {
                        ID = 22,
                        CampaignId = 9,
                        MultimediaObjectId = 16
                    });
                }
                return objectsCampaigns;
            }
        }
        #endregion

        #region Statystyki
        private static List<StatisticItem> statistics = null;
        public static List<StatisticItem> Statistics
        {
            get
            {
                if (statistics == null)
                {
                    statistics = new List<StatisticItem>();
                }

                return statistics;
            }

            set
            {
                statistics = value;
            }
        } 
        #endregion

        #region Powiązania statystyki - kampanie
        private static List<Statistics_CampaignItem> statistics_campaign = null;
        public static List<Statistics_CampaignItem> Statistics_Campaign
        {
            get
            {
                if (statistics_campaign == null)
                {
                    statistics_campaign = new List<Statistics_CampaignItem>();
                }

                return statistics_campaign;
            }

            set
            {
                statistics_campaign = value;
            }
        } 
        #endregion

        #region Powiązanie statystyki = kategorie
        private static List<Statistics_CategoryItem> statistics_category = null;
        public static List<Statistics_CategoryItem> Statistics_Category
        {
            get
            {
                if (statistics_category == null)
                {
                    statistics_category = new List<Statistics_CategoryItem>();
                }

                return statistics_category;
            }

            set
            {
                statistics_category = value;
            }
        } 
        #endregion

        #region Helpers
        /// <summary>
        /// Pomocniczna metoda określająca mime pliku
        /// </summary>
        /// <param name="fileName">Ścieżka do pliku</param>
        /// <returns></returns>
        public static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();

            return mimeType;
        }
        #endregion
    }
}
