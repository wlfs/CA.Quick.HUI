using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.Quick
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class ExtensionMethod
    {
        public static void IntToString(this ICollection<dynamic> data, string field, IDictionary<int, string> dic)
        {
            var maps = new Dictionary<string, IDictionary<int, string>>();
            maps.Add(field,dic);
            data.IntToString(maps);
        }
        public static void IntToString(this ICollection<dynamic> data,Dictionary<string, IDictionary<int, string>>  maps)
        {
            if (maps != null && maps.Count > 0) {
                int i = 0;
                foreach (IDictionary<string, object> item in data)
                {
                    foreach (var map in maps)
                    {
                        if (item.ContainsKey(map.Key)) {
                            int v=(int)item[map.Key];
                            item.Add(map + "_text", map.Value[v]);
                        }
                    }
                    i++;
                }
            }
        }
        /// <summary>
        /// 获取动态数据
        /// </summary>
        /// <param name="coll">集合</param>
        /// <returns>动态数据</returns>
        public static dynamic GetDynamic(this NameValueCollection coll)
        {
            Dictionary<String, Object> dic = new Dictionary<string, object>();
            for (int i = 0; i < coll.Count; i++)
            {
                dic.Add(coll.GetKey(i), coll.Get(i));
            }
            return new DynamicDataObject(dic);
        }
        public static List<dynamic> ToList(this DbDataReader reader) {
            var lis = new List<dynamic>();
            while (reader.Read())
            {
                Dictionary<String, Object> dic = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    dic.Add(reader.GetName(i), reader[i]);
                }
                lis.Add(new DynamicDataObject(dic));
            }
            reader.Close();
            return lis;
        }
        public static void AddParam(this ICollection<DbParameter> lis, string name, Object value) {
            lis.Add(new MySqlParameter("@"+name,value));
        }

       /// <summary>
       /// 将字符串集合连接成为字符串
       /// </summary>
       /// <param name="lis">字符串集合</param>
       /// <param name="pieces">连接字符串</param>
       /// <returns>连接后的字符串</returns>
        public static string Implode(this IEnumerable<String> lis, string pieces)
        {
            StringBuilder sb =new  StringBuilder();
            int i = 0;
            foreach (var item in lis)
            {
                if (i == 0)
                {
                    sb.Append(item);
                    i++;
                }
                else {
                    sb.Append(pieces+item);
                }
            }
            return sb.ToString();
        }
    }
}
