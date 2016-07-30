using CA.Quick.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.Quick
{
    class QuickTraceListener : TraceListener
    {
        //StringBuilder 
        private static string LogDir = Helper.GetBinPath()+"/Logs";
        public QuickTraceListener()
        {
            if (!Directory.Exists(LogDir)) {
                Directory.CreateDirectory(LogDir);
            }
        }
        public override void Write(string message)
        {
            writeLog(message + Environment.NewLine);
        }

        public override void WriteLine(string message)
        {
            writeLog(message + Environment.NewLine);
        }
        public override void Flush()
        {
            base.Flush();
        }

        private void writeLog(string message) {
            var file = LogDir+"/" +DateTime.Now.ToString("yyyyMMdd") + ".log";
            File.AppendAllText(file,"日志时间："+DateTime.Now.ToString("HH:mm:ss")+ Environment.NewLine+message+ Environment.NewLine);
            FileInfo fileInfo = new FileInfo(file);
            if (fileInfo.Length> 2097152)
            {
                //大于2M
                fileInfo.MoveTo(LogDir+"/" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".log");
            }
        }
    }
}
