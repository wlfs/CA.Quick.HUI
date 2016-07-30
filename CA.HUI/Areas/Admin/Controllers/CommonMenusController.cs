using CA.Quick.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CA.HUI.Areas.Admin.Controllers
{
    [Login]
    [AdminAuthorize]
    public class CommonMenusController : CRUDController<CommonMenus>
    {
        // GET: Admin/CommonMenus
        public ActionResult Index()
        {
            return View();
        }
    }
}
