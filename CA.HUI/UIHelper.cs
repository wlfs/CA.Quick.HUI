using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CA.HUI
{
    public  class UIHelper
    {
        public static T Get<T>() where T : new() {
            return new T();
        }
        public static string GetSkin() {
            dynamic dy = HttpContext.Current.Session["AdminInfo"];
            if (string.IsNullOrEmpty(dy.skin)) {
                return "default";
            }
            return dy.skin;
        }
    }
}