using CA.Quick.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CA.HUI
{
    class QuickTraceListener : TraceListener
    {
        //StringBuilder 
        private static string LogDir = Helper.GetBinPath()+"/Logs";
        private static string cache_key = "log_cache";
        public QuickTraceListener()
        {
            if (!Directory.Exists(LogDir)) {
                Directory.CreateDirectory(LogDir);
            }
        }
        public override void Write(string message)
        {
            cacheLog(message + Environment.NewLine);
        }

        public override void WriteLine(string message)
        {
            cacheLog("【"+DateTime.Now.ToString("mm:ss.fffffff")+"】"+message + Environment.NewLine);
        }
        public override void Flush()
        {
            base.Flush();
            if (HttpContext.Current.Items[cache_key] != null)
            {
                var str = (string)HttpContext.Current.Items[cache_key];
                writeLog(str);
            }
           
        }
        private void cacheLog(string message) {
            var str = "";
            if (HttpContext.Current.Items[cache_key] != null) {
                str = (string)HttpContext.Current.Items[cache_key];
            }
            HttpContext.Current.Items[cache_key]=str+ message;
        }

        private void writeLog(string message) {
            var file = LogDir+"/" +DateTime.Now.ToString("yyyyMMdd") + ".log";
            File.AppendAllText(file,message+ Environment.NewLine);
            FileInfo fileInfo = new FileInfo(file);
            if (fileInfo.Length> 2097152)
            {
                //大于2M
                fileInfo.MoveTo(LogDir+"/" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".log");
            }
        }
    }
}
