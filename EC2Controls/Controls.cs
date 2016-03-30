using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace EC2
{
	public static class Controls
	{
		public static MvcHtmlString AdServerFilter<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, bool withDiv = false, string format = null, object htmlObject = null, string validMsg = null)
		{
			var label = helper.LabelFor(expression);
			var textbox = helper.TextBoxFor(expression, format, htmlObject);
			var ret = label.ToHtmlString() + textbox.ToHtmlString();
			if (withDiv)
			{
				ret =
					string.Format(
						@"<td class=""col-width-180""><div class=""single-filter-div single-filter-div100"">{0}</div></td>", ret);
			}

			return MvcHtmlString.Create(ret);
		}

		public static MvcHtmlString AdServerSegregate(this HtmlHelper helper, string position, string url, string attribut, bool asceding, string name, bool onlyLink = false, string htmlClass = "", string htmlStyle = "", int? thWidth = null, string innerType=null, int? innerId=null)
		{
			var style = thWidth == null ? "" : string.Format(@"width: {0}px;", thWidth);
            var actionLink = String.Empty;
            if (innerId.HasValue && !String.IsNullOrWhiteSpace(innerType))
            {
                actionLink = string.Format(@"<a onclick=""InnerActionLink('{0}','{1}',{2}, 1, '{4}', {5});"">{3}</a>", url, attribut, (!asceding).ToString().ToLower(), name, innerType, innerId);
            }
            else
            {
                actionLink = string.Format(@"<a onclick=""ActionLink('{0}','{1}',{2});"">{3}</a>", url, attribut, (!asceding).ToString().ToLower(), name);
            }
            
			if (onlyLink)
			{
				return MvcHtmlString.Create(actionLink);
			}

			var th = string.Format(@"<th style=""{0}{1}"" class=""text-{2} {3}"">{4}</th>", style, htmlStyle, position, htmlClass, actionLink);
			return MvcHtmlString.Create(th);
		}
	}
}
