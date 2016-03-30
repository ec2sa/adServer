using System.Security.Cryptography.X509Certificates;
using ADServerDAL.Abstract;
using ADServerDAL.Entities.Presentation;
using ADServerDAL.Filters;
using ADServerDAL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using ADServerDAL.Models;
using ADServerDAL.Other;

namespace ADServerDAL.Concrete
{
	/// <summary>
	/// Implementacja repozytorium statystyk i zestawień
	/// </summary>
	public class EFStatisticRepository : EFBaseRepository, IStatisticRepository
	{
		/// <summary>
		/// Kolekcja dostępnych statystyk
		/// </summary>
		public IQueryable<Statistic> Statistics
		{
			get
			{
				var query = Context.Statistics;

				return query;
			}
		}

		public IQueryable<Statistic> CmpStatistics(int id)
		{
			var query = Context.Statistics.Where(it => it.CampaignId == id);
			return query;
		}

		public IQueryable<Statistic> ObjStatistics(int id)
		{
			var query = Context.Statistics.Where(it => it.MultimediaObjectId == id);
			return query;
		}

		public IQueryable<Statistic> DevStatistics(int id)
		{
			var query = Context.Statistics.Where(it => it.DeviceId == id);
			return query;
		}


		/// <summary>
		/// Kolekcja dostępnych zestawień statystyk
		/// </summary>
		/// <param name="filter"></param>
		/// <param name="paginationInfo"></param>
		/// <param name="statementType"></param>
		/// <param name="userId">Id użytkownika</param>
		public IQueryable<StatisticsStatementItem> StatisticsStatements(StatisticsStatementListViewModelFilter filter,
			Entities.AdPaginationInfo paginationInfo, StatisticsStatementType statementType, int userId)
		{
			//TODO: Poprawić to!
			List<IGrouping<int, StatementQueryRow>> objectsGroup = null;

			var sw = new System.Diagnostics.Stopwatch();

			sw.Start();
			switch (statementType)
			{
				case StatisticsStatementType.Campaign:
					objectsGroup = GetCampaignQuery(filter, userId);
					break;

				case StatisticsStatementType.MultimediaObject:
					objectsGroup = GetMultimediaObjectsQuery(filter, userId);
					break;
			}
			sw.Stop();
			TimeSpan getXQueryTime = sw.Elapsed;
			sw.Reset();
			var skip = 0;

			if (paginationInfo != null)
			{
				paginationInfo.OutResultsFound = objectsGroup.Count();
				skip = GetSkip(paginationInfo.RequestedPage, paginationInfo.ItemsPerPage, paginationInfo.OutResultsFound);
			}

			const int www = (int)ADServerDAL.Models.Statistic.RequestSourceType.WWW;
			const int desktop = (int)ADServerDAL.Models.Statistic.RequestSourceType.Desktop;

			var statements = new List<StatisticsStatementItem>();

			sw.Start();
			var statisticsToCategories = (from c in Context.Categories
										  select new StatisticToParentItem
										  {
											  ParentName = c.Name,
											  StatisticId = 1
										  }).ToList();
			sw.Stop();
			TimeSpan statisticsToCategoriesTime = sw.Elapsed;

			sw.Reset();

			sw.Start();

			if (objectsGroup != null && objectsGroup.Count > 0)
			{
				statements.AddRange(
					objectsGroup.Select(@group => new StatisticsStatementItem
					{
						Id = @group.Key,
						Name = @group.Select(x => x.ObjectName).FirstOrDefault(),
						Type = statementType,
						TotalDisplayCount = @group.Count(x => x.StatisticId > 0),
						WWWDisplayCount = @group.Count(x => x.Source == www),
						DesktopDisplayCount = @group.Count(x => x.Source == desktop),
						Categories = GetCategories(@group.ToList(), statisticsToCategories),
						ClickedCount = @group.Count(x => x.Clicked),
						AdPointsCount = @group.Sum(x => x.AdPoints),
						cAdPoints = statementType == StatisticsStatementType.Campaign ? @group.Select(x => x.cAdPoints).FirstOrDefault() : default(decimal?)
					}));
			}
			sw.Stop();

			TimeSpan foreachTime = sw.Elapsed;

			return
				statements.AsQueryable()
					.OrderByDescending(x => x.TotalDisplayCount)
					.ThenBy(x => x.Name)
					.Skip(skip)
					.Take(paginationInfo == null ? 10 : paginationInfo.ItemsPerPage);
		}

		private class StatisticToParentItem
		{
			public string ParentName { get; set; }

			public int StatisticId { get; set; }
		}

		private List<IGrouping<int, StatementQueryRow>> GetMultimediaObjectsQuery(
			StatisticsStatementListViewModelFilter filter, int userId)
		{
			var allObjects = (from m in Context.MultimediaObjects
							  where m.UserId == userId || userId == 0
							  select new
							  {
								  moID = m.Id,
								  moName = m.Name
							  }).ToList();

			var q1 = from m in Context.MultimediaObjects
					 join s in Context.Statistics on m.Id equals s.MultimediaObjectId into gj
					 where m.UserId == userId || userId == 0
					 from stat in gj.DefaultIfEmpty()
					 select new
					 {
						 moID = m.Id,
						 moName = m.Name,
						 stat,
						 clicked = stat.Clicked == null ? false : stat.Clicked,
						 AdPoints = stat.AdPoints == null ? (decimal)0.0 : stat.AdPoints
					 };


			#region filtering

			if (filter != null && filter.Filtering)
			{
				if (filter.FilterDateFrom.HasValue)
				{
					var dtS = filter.FilterDateFrom.Value.Date;
					q1 = q1.Where(q => q.stat == null || (q.stat != null && q.stat.RequestDate >= dtS));
				}

				if (filter.FilterDateTo.HasValue)
				{
					var dtE = filter.FilterDateTo.Value.Date.AddDays(1).AddSeconds(-1);
					q1 = q1.Where(q => q.stat == null || (q.stat != null && q.stat.RequestDate <= dtE));
				}
			}

			#endregion filtering

			var tt = q1.ToList();

			var q1a = from q in q1
					  select new
					  {
						  StatisticId = (q.stat == null ? -q.moID : q.stat.Id),
						  Source = (q.stat == null ? -1 : q.stat.RequestSource),
						  ObjectId = q.moID,
						  ObjectName = q.moName,
						  q.clicked,
						  q.AdPoints
					  };

			//obiekty, które występują w statystykach
			var q1x = q1.Select(x => new { ID = x.moID, x.clicked, x.AdPoints }).ToList().Distinct();

			//obiekty, których nie ma w statystykach za dany okres
			var q1b = from m in allObjects
					  from q in q1x
					  where q.ID == m.moID
					  select new
					  {
						  StatisticId = -m.moID,
						  Source = -1,
						  ObjectId = m.moID,
						  ObjectName = m.moName,
						  q.clicked,
						  q.AdPoints
					  };

			var query = q1a.ToList().Union(q1b.ToList()).Distinct();

			var q2 = from q in query
					 select new StatementQueryRow
					 {
						 StatisticId = q.StatisticId,
						 Source = q.Source,
						 ObjectId = q.ObjectId,
						 ObjectName = q.ObjectName,
						 Clicked = q.clicked,
						 AdPoints = q.AdPoints
					 };

			var q3 = q2.GroupBy(x => x.ObjectId).OrderByDescending(x => x.Count()).ToList();

			return q3;
		}

		private List<IGrouping<int, StatementQueryRow>> GetCampaignQuery(StatisticsStatementListViewModelFilter filter,
			int userId)
		{
			bool doFiltering = filter != null && filter.Filtering;

			var allCampaigns = (from c in Context.Campaigns
								where c.UserId == userId || userId == 0
								select new
								{
									c.Id,
									c.Name,
									c.AdPoints
								}).ToList();

			var b = from c in Context.Campaigns
					join s in Context.Statistics on c.Id equals s.CampaignId
					where c.UserId == userId || userId == 0
					select new
					{
						cID = c.Id,
						cName = c.Name,
						cAdPoints = c.AdPoints,
						s,
						clicked = s.Clicked,
						s.AdPoints
					};
			var t = b.ToList();

			#region filtering

			if (doFiltering)
			{
				if (filter.FilterDateFrom.HasValue)
				{
					var dtS = filter.FilterDateFrom.Value.Date;
					b = b.Where(x => x.s == null || (x.s != null && x.s.RequestDate >= dtS));
				}

				if (filter.FilterDateTo.HasValue)
				{
					var dtE = filter.FilterDateTo.Value.Date.AddDays(1).AddSeconds(-1);
					b = b.Where(x => x.s == null || (x.s != null && x.s.RequestDate <= dtE));
				}
			}

			#endregion filtering

			var q1a = from q in b
					  select new
					  {
						  StatisticId = (q.s == null ? -q.cID : q.s.Id),
						  Source = (q.s == null ? -1 : q.s.RequestSource),
						  ObjectId = q.cID,
						  ObjectName = q.cName,
						  clicked = q.s.Clicked,
						  q.s.AdPoints,
						  q.cAdPoints
					  };

			// Lista kampanii w statystykach
			var b_a = b.Select(x => new { ID = x.cID, x.clicked, x.AdPoints }).ToList().Distinct();
			var tt = b_a.ToList();
			// kampanie, których nie ma w statystykach za dany okres
			var q1b = //from c in Context.Campaign
				from c in allCampaigns
				from a in b_a
				where c.Id == a.ID
				select new
				{
					StatisticId = -c.Id,
					Source = -1,
					ObjectId = c.Id,
					ObjectName = c.Name,
					a.clicked,
					a.AdPoints,
					cAdPoints = c.AdPoints
				};

			// łączymy obie grupy
			var joined = q1a.ToList().Union(q1b.ToList()).Distinct();

			var q4 = from q in joined
					 select new StatementQueryRow
					 {
						 StatisticId = q.StatisticId,
						 Source = q.Source,
						 ObjectId = q.ObjectId,
						 ObjectName = q.ObjectName,
						 Clicked = q.clicked,
						 AdPoints = q.AdPoints,
						 cAdPoints = q.cAdPoints
					 };
			var query = q4.GroupBy(x => x.ObjectId).ToList();
			return query;
		}

		private Dictionary<string, int> GetCategories(IEnumerable<StatementQueryRow> rows,
			IEnumerable<StatisticToParentItem> statisticsToCategories)
		{
			var result = new Dictionary<string, int>();

			if (rows != null)
			{
				int[] ids = rows.Where(x => x.StatisticId > 0).Select(x => x.StatisticId).Distinct().ToArray();

				if (ids.Length > 0)
				{
					//var q = (from c in Context.Category
					//         join sc in Context.Statistics_Category on c.Id equals sc.CategoryId
					//         where ids.Contains(sc.StatisticsId)
					//         select c.Name);

					var q = from s in statisticsToCategories
							where ids.Contains(s.StatisticId)
							select s.ParentName;

					var q2 = q.GroupBy(x => x).ToList();

					foreach (var g in q2)
					{
						result.Add(g.Key, g.Count());
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Możliwość ustawienia innego kontekstu niż wbudowany
		/// </summary>
		/// <param name="context">Nowy kontekst EF</param>
		public void SetContext(System.Data.Entity.DbContext context)
		{
			SetNewContext(context);
		}

		public ApiResponse Save(Entities.StatisticsEntry statisticsEntry)
		{
			var response = new ApiResponse();
			statisticsEntry.Statistics.Name = "Stats";
			using (var transaction = Context.Database.BeginTransaction())
			{
				try
				{
					var dbEntry = new Statistic();
					dbEntry.Name = "Stats";

					#region zapis danych podstawowych statystyk

					if (statisticsEntry.Statistics.Id == 0)
					{
						dbEntry.Data1 = statisticsEntry.Statistics.Data1 ?? "";
						dbEntry.MultimediaObjectId = statisticsEntry.Statistics.MultimediaObjectId;
						dbEntry.Data2 = statisticsEntry.Statistics.Data2 ?? "";
						dbEntry.Data3 = statisticsEntry.Statistics.Data3 ?? "";
						dbEntry.Data4 = "";
						dbEntry.RequestDate = statisticsEntry.Statistics.RequestDate;
						dbEntry.RequestIP = statisticsEntry.Statistics.RequestIP;
						dbEntry.RequestSource = statisticsEntry.Statistics.RequestSource;
						statisticsEntry.Statistics.ResponseDate = DateTime.Now;
						dbEntry.ResponseDate = statisticsEntry.Statistics.ResponseDate;
						dbEntry.SessionId = statisticsEntry.Statistics.SessionId;
						dbEntry.Clicked = statisticsEntry.Statistics.Clicked;
						dbEntry.AdPoints = statisticsEntry.Statistics.AdPoints;
						dbEntry.DeviceId = statisticsEntry.Statistics.DeviceId;
						dbEntry.UserId = statisticsEntry.Statistics.UserId;
						dbEntry.CampaignId = statisticsEntry.Statistics.CampaignId;
						Context.Statistics.Add(dbEntry);
						Context.SaveChanges();
					}

					#endregion zapis danych podstawowych statystyk

					#region zapis powiązań statystyki z kampaniami

					//foreach (var cp in statisticsEntry.SelectedMultimediaObjectCampaigns)
					//{
					//	//Context.Statistics_Campaign.Add(new Statistics_Campaign
					//	//{
					//	//	CampaignId = cp.CampaignId,
					//	//	StatisticsId = dbEntry.Id
					//	//});
					//	//Context.SaveChanges();
					//}

					#endregion zapis powiązań statystyki z kampaniami

					#region zapis powiązań statystyki z kategoriami

					foreach (var cp in statisticsEntry.Categories)
					{
						//Context.Statistics_Category.Add(new Statistics_Category
						//{
						//	CategoryId = cp.Value,
						//	StatisticsId = dbEntry.Id
						//});
						//Context.SaveChanges();
					}

					#endregion zapis powiązań statystyki z kategoriami

					transaction.Commit();
				}
				catch (System.Data.Entity.Validation.DbEntityValidationException ex)
				{
					// Obsługa błędów EF
					using (var handler = new DbValidationErrorHandler(ex))
					{
						if (handler.HasErrors)
						{
							response.Errors.AddRange(handler.ValidationErrors);
						}
					}
				}
				catch (Exception ex)
				{
					// Obsługa błędów pozostałych
					var hierarchy = new List<Exception>();
					ExceptionsHandlingHelper.HierarchizeError(ex, ref hierarchy);
					if (hierarchy.Count > 0)
					{
						response.Errors.AddRange(
							hierarchy.Select(s => new ApiValidationErrorItem { Message = s.Message + Environment.NewLine + s.StackTrace })
								.Distinct()
								.AsEnumerable());
					}
				}

				if (response.Errors.Count > 0)
				{
					transaction.Rollback();
				}

				response.Accepted = response.Errors.Count == 0;
				return response;
			}
		}


		public IQueryable<StatisticsStatementItem> StatisticStatementDevice(int id)
		{
			var statements = new List<StatisticsStatementItem>();
			var obj =
				from s in Context.Statistics
				where s.UserId == id || id == 0
				group s by s.DeviceId
				into g
				select new {Key = g.Key, Value = g.ToList()};
			if (obj.Any())
			{
				statements.AddRange(
					obj.Select(@group => new StatisticsStatementItem
					{
						Id = @group.Key ?? 0,
						Name = @group.Value.Select(x => x.Device.Name).FirstOrDefault() ?? "OldDevice",
						Statistics = @group.Value.AsQueryable(),
						Type = StatisticsStatementType.Device,
						TotalDisplayCount = @group.Value.Count(it => !it.Clicked),
						WWWDisplayCount = @group.Value.Count(x => x.RequestSource == (int)Statistic.RequestSourceType.WWW && !x.Clicked),
						DesktopDisplayCount = @group.Value.Count(x => x.RequestSource == (int)Statistic.RequestSourceType.Desktop && !x.Clicked),
						ClickedCount = @group.Value.Count(x => x.Clicked),
						AdPointsCount = @group.Value.Sum(x => x.AdPoints)
					}));
			}

			return statements.AsQueryable();
		}

		public ReportDev ReportStatistic(int id)
		{
			var ret = new ReportDev();
			ret.Load(DevStatistics(id));
			return ret;
		}
	}
}