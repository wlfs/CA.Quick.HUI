using CA.Quick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CA.HUI.Areas.Admin.Controllers
{
    [Login]
    [AdminAuthorize]
    public class CRUDController<T> : Controller where T:BaseModel,new()
    {
        [AdminAuthorize]
        // GET: Admin/CRUD
        public ActionResult Index()
        {
            return View();
        }
        // GET: Admin/CommonGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/CommonGroups/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var data = collection.GetDynamic();
            var result=UIHelper.Get<T>().Add(data);
            return Json(result);

        }

        // GET: Admin/CommonGroups/Edit/5
        public ActionResult Edit(int id)
        {
            var result=UIHelper.Get<T>().Find(id);
            return View(result);
        }

        // POST: Admin/CommonGroups/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var data = collection.GetDynamic();
            var result=UIHelper.Get<T>().Update(data, id);
            return Json(result);
        }

        // POST: Admin/CommonGroups/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var result = UIHelper.Get<T>().Delete(id);
            return Json(result);
        }
    }
}