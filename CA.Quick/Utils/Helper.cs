using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.Quick.Utils
{
    public class Helper
    {
        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <param name="num">生成个数</param>
        /// <param name="patternIndex">随机种子</param>
        /// <returns>随机字符串</returns>
        public static string RndNum(int num, int patternIndex = 0)
        {
            var pattern = new List<String>(){
                "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ",
                "23456789ABCDEFGHJKLMNPQRSTUVWXYZ",
                "0123456789"
            };
            string str = "";
            Random rand = new Random();
            var len = pattern[patternIndex].Length;
            for (int i = 0; i < num; i++)
            {
                str += pattern[patternIndex][rand.Next(len)];
            }
            return str;
        }
    }
}
