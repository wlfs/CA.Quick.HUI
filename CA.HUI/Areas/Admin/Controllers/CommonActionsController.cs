using CA.Quick.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CA.HUI.Areas.Admin.Controllers
{
    public class CommonActionsController : CRUDController<CommonActions>
    {
        // GET: Admin/CommonActions
        public ActionResult Index()
        {
            return View();
        }
    }
}