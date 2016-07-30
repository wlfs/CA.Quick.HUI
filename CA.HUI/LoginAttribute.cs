using CA.Quick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CA.HUI
{
    public class LoginAttribute : ActionFilterAttribute
    {
        /// <summary> 
        /// 验证权限（action执行前会先执行这里） 
        /// </summary> 
        /// <param name="filterContext"></param> 
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!UIHelper.IsLogin())
            {
               var r= new ContentResult();
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    var content = new JsonResult();
                    content.Data = Result<String>.Error("请先登录！", -100);
                    filterContext.Result = content;
                }
                else
                {
                    filterContext.Result = new RedirectResult("Common/Login");
                }
            }
        }
    }
}