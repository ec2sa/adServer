using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADServerManagementWebApplication.Infrastructure
{
    /// <summary>
    /// Klucze do przechowywania informacji o ustawieniach stron (numery stron, pole sortowane, kierunek sortowania)
    /// </summary>
    public enum PageSettingsKey
    {
        /// <summary>
        /// Lista kategorii
        /// </summary>
        CampaignCategoriesPageSettings,


        /// <summary>
        /// Lista kampanii
        /// </summary>
        CampaignPageSettings,


        /// <summary>
        /// Lista priorytetów
        /// </summary>
        CampaignPrioritiesPageSettings,


        /// <summary>
        /// Lista obiektów
        /// </summary>
        MultimediaObjectsPageSettings,


        /// <summary>
        /// Lista typów obiektów
        /// </summary>
        MultimediaTypesPageSettings,


        /// <summary>
        /// Lista statystyk
        /// </summary>
        StatisticsPageSettings,


        /// <summary>
        /// Zestawienia kampanii
        /// </summary>
        StatisticsStatementCampaignsPageSettings,

		/// <summary>
		/// Zestawienia obiektów multimedialnych
		/// </summary>
		StatisticsStatementObjectsPageSettings,

		/// <summary>
		/// Zestawienia NOsników
		/// </summary>
		StatisticsStatementDevicePageSettings,

		UsersPageSettings,

		DevicePageSettings,

		RolePageSettings,

		CmpDetailsSettings,
		ObjDetailsSettings,
		DevDetailsSettings
    }
}