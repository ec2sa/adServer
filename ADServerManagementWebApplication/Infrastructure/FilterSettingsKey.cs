using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADServerManagementWebApplication.Infrastructure
{
    /// <summary>
    /// Klucze do przechowywania informacji o filtrach stron
    /// </summary>
    public enum FilterSettingsKey
    {
        /// <summary>
        /// Lista kategorii
        /// </summary>
        CampaignCategoriesControllerFilterList,

        /// <summary>
        /// Lista kampanii
        /// </summary>
        CampaignControllerFilterList,

        /// <summary>
        /// Lista priorytetów
        /// </summary>
        CampaignPrioritiesControllerFilterList,

        /// <summary>
        /// Lista obiektów
        /// </summary>
        MultimediaObjectsControllerFilterList,

        /// <summary>
        /// Lista typów
        /// </summary>
        MultimediaTypesListViewModelFilter,


        /// <summary>
        /// Lista statystyk
        /// </summary>
        StatisticsControllerFilterList,

        /// <summary>
        /// Zestawienia statystyk
        /// </summary>
        StatisticsStatementControllerFilterList,

		UserListViewModelFilter,
		DevsListViewModelFilter,

		DeviceFilterList,

		RoleFilterList,

		CmpDetailsFilterList,
		ObjDetailsFilterList,
		DevDetailsFilterList
    }
}