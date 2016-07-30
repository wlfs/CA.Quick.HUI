using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CA.HUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void Application_BeginRequest(object sender, EventArgs e) {
            Trace.Write("【" + DateTime.Now.ToString("HH:mm:ss") + "】"+ UIHelper.GetIP4Address()+"  " + Request.RawUrl);
        }
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            Trace.Flush();
        }
        protected void Page_Error(object sender, EventArgs e)
        {
           
            Trace.WriteLine(Server.GetLastError());
            Trace.Flush();
            string errorMsg = String.Empty;
            Exception currentError = Server.GetLastError();
            errorMsg += "来自页面的异常处理<br />";
            errorMsg += "系统发生错误:<br />";
            errorMsg += "错误地址:" + Request.Url + "<br />";
            errorMsg += "错误信息:" + currentError.Message + "<br />";
            Response.Write(errorMsg);
            Server.ClearError();//清除异常(否则将引发全局的Application_Error事件)
        }
    }
}
