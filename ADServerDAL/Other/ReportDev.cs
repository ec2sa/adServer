using ADServerDAL.Extenstions;
using ADServerDAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;

namespace ADServerDAL.Other
{
	public class ReportDev
	{
		private Dictionary<DateTime, int> _lastDays;
		private Dictionary<int, int> _lastWeeks;
		private Dictionary<string, int> _lastMonths;

		public Dictionary<DateTime, int> LastDays { get { return _lastDays; } set { _lastDays = value; } }

		public Dictionary<int, int> LastWeeks { get { return _lastWeeks; } set { _lastWeeks = value; } }

		public Dictionary<string, int> LastMonths { get { return _lastMonths; } set { _lastMonths = value; } }

		public void Load(IQueryable<Statistic> query)
		{
			query = query.Where(it => it.RequestDate > DbFunctions.AddYears(DateTime.Now, -1));

			var days = query.GroupBy(it => DbFunctions.TruncateTime(it.RequestDate));
			var month = query.GroupBy(it => it.RequestDate.Month);
			var weeks = query.GroupBy(it => DbFunctions.DiffDays(DbFunctions.CreateDateTime(DateTime.Now.Year, 1, 1, 1, 1, 1), it.RequestDate) / 7);
			var week = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) - 10;
			_lastDays = days
				.Where(it => it.Key.Value > DbFunctions.AddMonths(DateTime.Now, -1))
				.ToDictionary(q => q.Key.Value, q => q.DistinctBy(it => it.RequestIP).Count());
			_lastWeeks = weeks
				.Where(it => it.Key > week)
				.ToDictionary(q => q.Key.Value,
					q => q.DistinctBy(it => it.RequestIP).Count());
			_lastMonths = month
				.ToDictionary(q => q.Key > DateTime.Now.Month - 1 ? DateTime.Now.Year + "-" + (q.Key > 9 ? q.Key.ToString() : "0" + q.Key) : DateTime.Now.Year - 1 + "-" + (q.Key > 9 ? q.Key.ToString() : "0" + q.Key),
					q => q.DistinctBy(it => it.RequestIP).Count());
		}
	}
}