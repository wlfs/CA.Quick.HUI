using CA.Quick;
using CA.Quick.Models;
using CA.Quick.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CA.HUI.Areas.Admin.Controllers
{
    public class CommonController : Controller
    {
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            string v = (string)Session["admin_verify"];
            if (v == collection["verify"])
			{
				var uname = collection["uname"];
				var pass = collection["pass"];
                var reuslt=UIHelper.Get<CommonAdmin>().Login(uname, Helper.MD5(pass));
                if (reuslt.status == 1){
                    _getActions(reuslt.data.id);
					var return_url = (string)Session["login_return_url"];
                    if (string.IsNullOrEmpty(return_url))
                    {
                        return_url=Url.Action("Index","Index");
                    }
                    reuslt.data=return_url;
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                    1,
                    uname,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    false,
                    "admins"
                    );
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    System.Web.HttpCookie authCookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(authCookie);
                    Response.Cookies.Add(new HttpCookie("Admin_ID", reuslt.data.id));
                    Response.Cookies.Add(new HttpCookie("Admin_Name", reuslt.data.name));
                }
                return Json(reuslt);
            }else{
                return Json(Result<String>.Error("验证码错误"));
            }
        }
        public ActionResult Login()
        {
            return View();
        }
        
        public ActionResult Logout()
        {
            Session.Remove("AdminInfo");
            return  RedirectToAction("Login");
        }
        private  void _getActions(int id)
        {
            var result = UIHelper.Get<CommonAdmin>().GetActions(id);
            Session["AdminActions"] = result;
        }
        /// <summary>
        /// xx
        /// </summary>
        /// <returns></returns>
        public ActionResult Verify()
        {
            var vc=Helper.RndNum(4,2);
            Session["admin_verify"] = vc;
            var file=new ValidateCode().GrunImg(vc);
            return File(file.ToArray(),"image/jpeg");
        }
    }
}
