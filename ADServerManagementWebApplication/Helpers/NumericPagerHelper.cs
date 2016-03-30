using System;
using System.Security.Policy;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using ADServerManagementWebApplication.Models;
using Microsoft.Ajax.Utilities;

namespace ADServerManagementWebApplication.Helpers
{
    /// <summary>
    /// Helper HTML obsługujący stronnicowanie list
    /// </summary>
    public static class NumericPagerHelper
    {
        #region - Public methods -
        /// <summary>
        /// Obsługuje stronnicowanie list
        /// </summary>
        /// <param name="helper">HTML</param>
        /// <param name="totalNumResults">Liczba wszystkich rekordów</param>
        /// <param name="itemsPerPage">Liczba rekordów na stronę</param>
        /// <param name="currentPage">Aktualna strona</param>
        /// 

        public static MvcHtmlString CreateNumericPager(this HtmlHelper helper, int totalNumResults, int itemsPerPage, int currentPage)
        {
            return CreateNumericPager(helper, totalNumResults, itemsPerPage, currentPage, null, null);
        }

        public static MvcHtmlString CreateNumericPager(this HtmlHelper helper, int totalNumResults, int itemsPerPage, int currentPage, string prefix=null, int? innerId=null)
        {
            ///Obliczenie liczby strony
            int numberOfPages = (int)Math.Ceiling((double)totalNumResults / (double)itemsPerPage);

            ///Maksymalna liczba linków stronnicowania
            int maxNumberOfPagesShown = 10;

            ///Określenie czy należy wyświetlać linki do przejścia do pierwszej/ostatniej strony
            bool showFirstAndLast = numberOfPages > maxNumberOfPagesShown;

            ///Określenie strony startowej
            int startPage = getStartPage(numberOfPages, currentPage, maxNumberOfPagesShown);

            ///Określenie strony końcowej
            int endPage = getEndPage(numberOfPages, currentPage, startPage, maxNumberOfPagesShown);

            ///Zbudowanie linków stronnicowania
            StringBuilder builder = new StringBuilder();

            builder.Append("<ul>");

            if (showFirstAndLast && startPage > 1)
            {
                builder.Append("<li>");
                builder.Append(buildActionLink(helper, "...", 1, prefix, innerId));
                builder.Append("</li>");
            }
            for (int i = startPage; i <= endPage; i++)
            {
                string PageLinkText = i.ToString();
                builder.Append("<li>");
                if (i != currentPage)
                {
                    builder.Append(buildActionLink(helper, PageLinkText, i, prefix, innerId));
                }
                else
                {
                    builder.Append(i);
                }
                builder.Append("</li>");
            }
            if (showFirstAndLast && (endPage != numberOfPages))
            {
                builder.Append("<li>");
                builder.Append(buildActionLink(helper, "...", numberOfPages, prefix, innerId));
                builder.Append("</li>");
            }

            builder.Append("</ul>");
            return MvcHtmlString.Create(builder.ToString());
        } 
        #endregion

        #region - Private methods -
        /// <summary>
        /// Wyliczenie strony startowej
        /// </summary>
        /// <param name="numberOfPages">Liczba stron</param>
        /// <param name="currentPage">Bieżąca strona</param>
        private static int getStartPage(int numberOfPages, int currentPage, int maxPages)
        {
            int minToDisplay = 1;

            if (numberOfPages > maxPages)
            {
                if (currentPage > (maxPages / 2))
                {
                    minToDisplay = currentPage - ((maxPages / 2) - 1);
                }

                if (currentPage > (numberOfPages - (maxPages / 2)))
                {
                    minToDisplay = numberOfPages - maxPages;

                    if (minToDisplay == 1)
                    {
                        minToDisplay++;
                    }
                }
            }

            return minToDisplay;
        }

        /// <summary>
        /// Wyliczenie strony końcowej
        /// </summary>
        /// <param name="numberOfPages">Liczba stron</param>
        /// <param name="currentPage">Bieżąca strona</param>
        /// <param name="startPage">Strona startowa</param>
        private static int getEndPage(int numberOfPages, int currentPage, int startPage, int maxPages)
        {
            int maxToDisplay = startPage + maxPages - 1;
            if (maxToDisplay > numberOfPages)
            {
                maxToDisplay = maxToDisplay - (maxToDisplay - numberOfPages);
            }
            if ((currentPage > numberOfPages - (maxPages/2)) && (startPage != 1))
            {
                maxToDisplay = numberOfPages;
            }

            return maxToDisplay;
        }

        /// <summary>
        /// Zbudowanie linku HTML pozwalającego na przejście do wybranej strony
        /// </summary>
        /// <param name="helper">HTML helper</param>
        /// <param name="linkText">Tekst wyświetlany na linku</param>
        /// <param name="pageParam">Numer żądanej strony</param>
        private static string buildActionLink(HtmlHelper helper, string linkText, int pageParam, string prefix=null, int? innerId=null)
        {
	        var url = helper.ViewContext.RouteData.Values["controller"] + "/" + helper.ViewContext.RouteData.Values["action"] ;
            var link = @"<a onclick=""ActionLink('{0}','{1}',{2}, '{3}');"">{3}</a>";
            var innerLink = @"<a onclick=""InnerActionLink('{0}','{1}',{2}, '{3}', '{5}', {6});"">{3}</a>";

            if (!String.IsNullOrWhiteSpace(prefix) && innerId.HasValue)
                return string.Format(innerLink, url, ((ListViewModel)helper.ViewData.Model).SortExpression, ((ListViewModel)helper.ViewData.Model).SortAccending ? "true" : "false", pageParam, pageParam, prefix, innerId);
            else
                return string.Format(link, url, ((ListViewModel)helper.ViewData.Model).SortExpression, ((ListViewModel)helper.ViewData.Model).SortAccending ? "true" : "false", pageParam, pageParam);
            ///Sprawdzenie czy request posiada parametry
            if (helper.ViewContext.HttpContext.Request.QueryString.HasKeys())
            {
                ///Pobranie parametru dot. pola sortowania
                string sort = helper.ViewContext.HttpContext.Request.QueryString["SortExpression"];


                ///Pobranie parametru dot. kierunku sortowania
                string accending = helper.ViewContext.HttpContext.Request.QueryString["Ascending"];


                ///Zbudowanie linku na podstawie przekazanych parametrów
                return helper.ActionLink(linkText, helper.ViewContext.RouteData.Values["action"].ToString(),
                        new
                        {
                            SortExpression = sort,
                            Page = pageParam,
                            Accending = accending
                        }).ToString();
            }
            else
            {
				
                ///Zwrócenie domyślnego linka
                return helper.ActionLink(linkText, helper.ViewContext.RouteData.Values["action"].ToString(),
                                        new
                                        {
                                            Page = pageParam
                                        }).ToString();
            }
        } 
        #endregion
    }
}