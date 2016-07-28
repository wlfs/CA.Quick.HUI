using CA.Quick.Models;
using CA.Quick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CA.HUI.Areas.Admin.Controllers
{
    public class CommonGroupsController : CRUDController<CommonGroups>
    {
        // GET: Admin/CommonGroups
        public ActionResult Index(HttpRequest req)
        {
            var model=UIHelper.Get<CommonGroups>();
            ViewBag.lis=model.List(req.QueryString["kw"]);
            return View();
        }
    }
}
