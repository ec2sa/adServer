using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ADServerManagementWebApplication.Infrastructure;

namespace ADServerManagementWebApplication.Controllers
{
	public class DefaultController : AdServerBaseController
    {
        public ActionResult Index(string ctr, string act = "")
        {
	        if (string.IsNullOrEmpty(ctr))
		        ctr = "Campaign";
	        if (string.IsNullOrEmpty(act))
		        act = "Index";
	        ViewBag.url = Url.Action(act, ctr);
            return View();
        }
	}
}