<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%= (bool?) ViewData["PreventLineBreak"]  == null || (bool?) ViewData["PreventLineBreak"]  == false ? Html.Raw(ViewData.Model) : Html.Raw(ViewData.Model.ToString().Replace(" ", "&nbsp;")) %>


