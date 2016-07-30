using CA.Quick.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CA.Quick;

namespace CA.HUI.Areas.Admin.Controllers
{

    public class IndexController : Controller
    {
        // GET: Admin/Index
        public ActionResult Index()
        {
            ViewBag.AdminInfo = new ExpandoObject();
            ViewBag.AdminInfo.name= "Admin" + Session["AdminInfo2"]+ Session["AdminInfo"];
            return View();
        }
        public ActionResult Welcome() {
            return View();
        }
        public ActionResult SetSkin(FormCollection coll) {

            var model=UIHelper.Get<CommonAdmin>();
            var r=model.Update(coll.GetDynamic(),UIHelper.UserID);
            return Json(r);
        }
    }
}
