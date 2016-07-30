using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace CA.HUI
{
    public  class UIHelper
    {
        public static bool IsLogin() {
            if (UserID>0)
            {
                return true;
            }
            else {
                return false;
            }

        }
        public static dynamic AdminInfo() {
            
            var session = HttpContext.Current.Session;
            return  session["AdminInfo"];
        }
        public static T Get<T>() where T : new() {
            return new T();
        }
        public static string GetSkin() {
            dynamic dy = AdminInfo();
            if (dy==null||string.IsNullOrEmpty(dy.skin)) {
                return "default";
            }
            return dy.skin;
        }
        public static int UserID {
            get {

                var id=HttpContext.Current.Request.Cookies["Admin_Id"];
                if (id != null && id.Value != null && !string.IsNullOrEmpty(id.Value)) {
                    return int.Parse(id.Value);
                }
                return 0;
                //dynamic dy = AdminInfo();
                //if (dy == null || string.IsNullOrEmpty(dy.id))
                //{
                //    return 0;
                //}
                //return dy.id;
            }
        }
        public static string GetIP4Address()
        {
            string IP4Address = String.Empty;


            foreach (IPAddress IPA in Dns.GetHostAddresses(System.Web.HttpContext.Current.Request.UserHostAddress))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            if (IP4Address != String.Empty)
            {
                return IP4Address;
            }

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            return IP4Address;
        }
    }
}