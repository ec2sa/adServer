using ADServerDAL.Entities;
using ADServerDAL.Entities.Presentation;
using ADServerDAL.Filters;
using ADServerDAL.Models;
using System;
using System.Linq;
using ADServerDAL.Other;

namespace ADServerDAL.Abstract
{
	/// <summary>
	/// Interfejs factory obsługujący statystyki
	/// </summary>
	public interface IStatisticRepository : IDisposable
	{
		/// <summary>
		/// Kolekcja dostępnych statystyk
		/// </summary>
		IQueryable<Statistic> Statistics { get; }

		/// <summary>
		/// Kolekcja dostępnych zestawień statystyk
		/// </summary>
		/// <param name="filter"></param>
		/// <param name="paginationInfo"></param>
		/// <param name="statementType"></param>
		IQueryable<StatisticsStatementItem> StatisticsStatements(StatisticsStatementListViewModelFilter filter, AdPaginationInfo paginationInfo, StatisticsStatementType statementType, int userId);

		/// <summary>
		/// Kolekcja dostępnych statystyk dla kampanii
		/// </summary>
		IQueryable<Statistic> CmpStatistics(int id);

		/// <summary>
		/// Kolekcja dostępnych statystyk dla objektów
		/// </summary>
		IQueryable<Statistic> ObjStatistics(int id);

		/// <summary>
		/// Kolekcja dostępnych statystyk dla nośników
		/// </summary>
		IQueryable<Statistic> DevStatistics(int id);

		IQueryable<StatisticsStatementItem> StatisticStatementDevice(int id);

		ReportDev ReportStatistic(int id);

		/// <summary>
		/// Możliwość ustawienia innego kontekstu niż wbudowany
		/// </summary>
		/// <param name="context">Nowy kontekst EF</param>
		void SetContext(System.Data.Entity.DbContext context);

		ApiResponse Save(StatisticsEntry statisticsEntry);
	}
}