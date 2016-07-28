using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CA.QuickTests
{
    public class Helper
    {
        public static string tableNameToClassName(string tableName)
        {
            var namses = tableName.Split('-','_');
            var name = "";
            for (var i = 0; i < namses.Length; i++)
            {
                name += namses[i][0].ToString().ToUpper() + namses[i].Substring(1);
            }
            return name;
        }
        //public static bool xx() {
        //       
        //}
    }
}
