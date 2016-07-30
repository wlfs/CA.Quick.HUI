using CA.Quick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CA.HUI
{

    public class AdminAuthorizeAttribute : ActionFilterAttribute
    {

        /// <summary> 
        /// 角色名称 
        /// </summary> 
        public string Code { get; set; }
        /// <summary> 
        /// 验证权限（action执行前会先执行这里） 
        /// </summary> 
        /// <param name="filterContext"></param> 
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!UIHelper.IsLogin())
            {
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
            //filterContext.HttpContext.Session["AdminActions"];
            //string[] Role = CheckLogin.Instance.GetUser().Roles.Split(',');//获取所有角色 
            //if (!Role.Contains(Code))//验证权限 
            //{
            //    //验证不通过 
            //    ContentResult Content = new ContentResult();
            //    Content.Content = "<script type='text/javascript'>alert('权限验证不通过！');history.go(-1);</script>";
            //    filterContext.Result = Content;
            //}
        }
    }
}