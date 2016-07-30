﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//========================================================
// CPQuery 是一个解决拼接SQL的新方法。
// CPQuery 可以让你采用拼接SQL的方式来编写参数化SQL 。
// 关于CPQuery的更多介绍请浏览：http://www.cnblogs.com/fish-li/archive/2012/09/10/CPQuery.html
// CPQuery 是一个开源的工具类，请在使用 CPQuery 时保留这段注释。
// 【 删除开源代码中的注释是可耻的行为！ 】
//========================================================
namespace CA.Quick
{
    public sealed class CPQuery
    {
        private enum SPStep // 字符串参数的处理进度
        {
            NotSet,     // 没开始或者已完成一次字符串参数的拼接。
            EndWith,    // 拼接时遇到一个单引号结束
            Skip        // 已跳过一次拼接
        }

        private int _count;
        private StringBuilder _sb = new StringBuilder(1024);
        private Dictionary<string, QueryParameter> _parameters = new Dictionary<string, QueryParameter>(10);

        private bool _autoDiscoverParameters;
        private SPStep _step = SPStep.NotSet;

        public CPQuery(string text, bool autoDiscoverParameters)
        {
            _sb.Append(text);
            _autoDiscoverParameters = autoDiscoverParameters;
        }
        public static CPQuery New()
        {
            return new CPQuery(null, false);
        }
        public static CPQuery New(bool autoDiscoverParameters)
        {
            return new CPQuery(null, autoDiscoverParameters);
        }

        public override string ToString()
        {
            return _sb.ToString();
        }
        public void BindToCommand(DbCommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            command.CommandText = _sb.ToString();
            command.Parameters.Clear();

            foreach (KeyValuePair<string, QueryParameter> kvp in _parameters)
            {
                DbParameter p = command.CreateParameter();
                p.ParameterName = kvp.Key;
                p.Value = kvp.Value.Value;
                command.Parameters.Add(p);
            }
        }
        /// <summary>
        /// 绑定参数集合
        /// </summary>
        /// <param name="lis"></param>
        public void BindParameter(ICollection<DbParameter> lis) {
            foreach (KeyValuePair<string, QueryParameter> kvp in _parameters)
            {
                lis.AddParam(kvp.Key, kvp.Value.Value);
            }
        }

        private void AddSqlText(string s)
        {
            if (string.IsNullOrEmpty(s))
                return;

            if (_autoDiscoverParameters)
            {
                if (_step == SPStep.NotSet)
                {
                    if (s[s.Length - 1] == '\'')
                    {   // 遇到一个单引号结束
                        _sb.Append(s.Substring(0, s.Length - 1));
                        _step = SPStep.EndWith;
                    }
                    else
                    {
                        object val = TryGetValueFromString(s);
                        if (val == null)
                            _sb.Append(s);
                        else
                            this.AddParameter(val.AsQueryParameter());
                    }
                }
                else if (_step == SPStep.EndWith)
                {
                    // 此时的s应该是字符串参数，不是SQL语句的一部分
                    // _step 在AddParameter方法中统一修改，防止中途拼接非字符串数据。
                    this.AddParameter(s.AsQueryParameter());
                }
                else
                {
                    if (s[0] != '\'')
                        throw new ArgumentException("正在等待以单引号开始的字符串，但参数不符合预期格式。");

                    // 找到单引号的闭合输入。
                    _sb.Append(s.Substring(1));
                    _step = SPStep.NotSet;
                }
            }
            else
            {
                // 不检查单引号结尾的情况，此时认为一定是SQL语句的一部分。
                _sb.Append(s);
            }
        }
        public string AddParams(QueryParameter p) {
            string name = "@p" + (_count++).ToString();
            _parameters.Add(name, p);
            return name;
        }
        private void AddParameter(QueryParameter p)
        {
            if (_autoDiscoverParameters && _step == SPStep.Skip)
                throw new InvalidOperationException("正在等待以单引号开始的字符串，此时不允许再拼接其它参数。");


            string name = "@p" + (_count++).ToString();
            _sb.Append(name);
            _parameters.Add(name, p);


            if (_autoDiscoverParameters && _step == SPStep.EndWith)
                _step = SPStep.Skip;
        }

        private object TryGetValueFromString(string s)
        {
            // 20，可以是byte, short, int, long, uint, ulong ...
            int number1 = 0;
            if (int.TryParse(s, out number1))
                return number1;

            DateTime dt = DateTime.MinValue;
            if (DateTime.TryParse(s, out dt))
                return dt;

            // 23.45，可以是float, double, decimal
            decimal number5 = 0m;
            if (decimal.TryParse(s, out number5))
                return number5;

            // 其它类型全部放弃尝试。
            return null;
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="fields">字段集名称，多个字段用|或&分割</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public CPQuery FullLike(string fields, string value) {
            var fieldArr=fields.Split('|','&');
            if (fieldArr.Length > 1)
            {
                _sb.Append(" and ( ");
                var pname = AddParams(("%" + value + "%").AsQueryParameter());
                foreach (var item in fieldArr)
                {
                    fields=fields.Replace("|"," like "+pname+" or ");
                    fields = fields.Replace("&", " like " + pname + " and ");
                    _sb.Append(fields + " like "+ pname);
                }
                _sb.Append(" ) ");
            }
            else {
                AddSqlText(" and  " + fields + " like ");
                AddParameter(("%" + value + "%").AsQueryParameter());
            }
           
            return this;
        }


        public static CPQuery operator +(CPQuery query, string s)
        {
            query.AddSqlText(s);
            return query;
        }
        public static CPQuery operator +(CPQuery query, QueryParameter p)
        {
            query.AddParameter(p);
            return query;
        }
    }

    public sealed class QueryParameter
    {
        private object _val;

        public QueryParameter(object val)
        {
            _val = val;
        }

        public object Value
        {
            get { return _val; }
        }

        public static explicit operator QueryParameter(string a)
        {
            return new QueryParameter(a);
        }
        public static implicit operator QueryParameter(int a)
        {
            return new QueryParameter(a);
        }
        public static implicit operator QueryParameter(decimal a)
        {
            return new QueryParameter(a);
        }
        public static implicit operator QueryParameter(DateTime a)
        {
            return new QueryParameter(a);
        }
        // 其它需要支持的隐式类型转换操作符重载请自行添加。
    }


    public static class CPQueryExtensions
    {
        public static CPQuery AsCPQuery(this string s)
        {
            return new CPQuery(s, false);
        }
        public static CPQuery AsCPQuery(this string s, bool autoDiscoverParameters)
        {
            return new CPQuery(s, autoDiscoverParameters);
        }

        public static QueryParameter AsQueryParameter(this object b)
        {
            return new QueryParameter(b);
        }
    }
}

