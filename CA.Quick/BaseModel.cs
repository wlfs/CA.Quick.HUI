using CA.Quick;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.Quick
{
    public abstract class  BaseModel
    {
        private MySqlHelper _helper;
        public BaseModel()
        {
            _helper = new MySqlHelper();
        }
        public MySqlHelper DB { get {
                return _helper;
            }
        }
        /// <summary>
        /// 字段词典
        /// </summary>
        private static Dictionary<String, List<string>> _fields_dic= new Dictionary<string, List<string>>();
        protected List<String> GetFields()
        {
            if (!_fields_dic.ContainsKey(tableName)) {
                _fields_dic[tableName] = _getFields(tableName);
            }
            return _fields_dic[tableName];
        }
        private  List<String> _getFields(String _tbName) {
            var sql = string.Format("select COLUMN_NAME from information_schema.COLUMNS where table_name = '{0}' and table_schema = '{1}'", _tbName, MySqlHelper.DatabaseName);
            var reader = DB.ExecuteDataReader(sql);
            var lis = new List<String>();
            while (reader.Read())
            {
                lis.Add(reader.GetString("COLUMN_NAME"));
            }
            reader.Close();
            return lis;
        }
        protected abstract string tableName { get; }
        protected abstract String pkName {  get; }
        public Result<int> Add(dynamic obj) {
            List<String> fields= new List<string>();
            List<DbParameter> _parameters = new List<DbParameter>();
            var _tfilds = GetFields();
            foreach (var property in (IDictionary<String, Object>)obj)
            {
                var name=property.Key.ToLower();
                if (_tfilds.Contains(name)) {
                    fields.Add(name);
                    _parameters.AddParam(name, property.Value);
                }
            }
            if (fields.Count > 0)
            {
                var sql = string.Format("insert into {0} (`{1}`) VALUES(@{2})", tableName, fields.Implode("`,`"), fields.Implode(",@"));
                var count = DB.ExecuteNonQuery(sql, _parameters.ToArray());
                return Result<int>.ResultData(count);
            }
            else {
                return Result<int>.Error("无数据添加！");
            }
        }
        public Result<int> AddAll(ICollection<dynamic> lis) {
            List<String> fields = new List<string>();
            List<DbParameter> _parameters = new List<DbParameter>();
            var where = CPQuery.New();
            var _tfilds = GetFields();
            var i =0;
            foreach (IDictionary<String, Object> obj in lis)
            {
                var j = 0;
                if (i == 0)
                {
                    where = where + "(";
                    foreach (var property in obj)
                    {
                        var name = property.Key.ToLower();
                        if (_tfilds.Contains(name))
                        {
                            fields.Add(name);
                            if (j == 0)
                            {
                                where = where + property.Value.AsQueryParameter();
                                j++;
                            }
                            else {
                                where = where +","+ property.Value.AsQueryParameter();
                            }
                        }
                    }
                    where = where + ")";
                    i++;
                }
                else {
                    where = where + "(";
                    foreach (var field in fields)
                    {
                       
                        if (obj.ContainsKey(field))
                        {
                            if (j == 0)
                            {
                                where = where + obj[field].AsQueryParameter();
                                j++;
                            }
                            else
                            {
                                where = where + "," + obj[field].AsQueryParameter();
                            }
                        }
                    }
                    where = where + ")";
                }
            }
            if (fields.Count > 0)
            {
                where.BindParameter(_parameters);
                var sql = string.Format("insert into {0} (`{1}`) VALUES(@{2})", tableName, fields.Implode("`,`"), where.ToString());
                var count = DB.ExecuteNonQuery(sql, _parameters.ToArray());
                return Result<int>.ResultData(count);
            }
            else
            {
                return Result<int>.Error("无数据添加！");
            }
        }
        public static dynamic CreateDynamic() {
            return new ExpandoObject();
        }
        public static CPQuery CreateWhere() {
            return CPQuery.New() + " 1=1 ";
        }
        public Result<int> AddResult(dynamic obj) {
            List<String> fields = new List<string>();
            List<DbParameter> _parameters = new List<DbParameter>();
            var _tfilds = GetFields();
            foreach (var property in (IDictionary<String, Object>)obj)
            {
                var name = property.Key.ToLower();
                if (_tfilds.Contains(name))
                {
                    fields.Add(name);
                    _parameters.AddParam(name, property.Value);
                }
            }
            if (fields.Count > 0)
            {
                var sql = string.Format("insert into {0} (`{1}`) VALUES(@{2});select last_insert_id() as id;", tableName, fields.Implode("`,`"), fields.Implode(",@"));
                var id = DB.ExecuteScalar<int>(sql, _parameters.ToArray());
                return Result<int>.ResultData(id);
            }
            else {
                return Result<int>.Error("无数据添加！");
            }
        }
        public Result<int> Update(dynamic obj,int id) {
            var where = CPQuery.New()+" id = " +id;
            return Update(obj,where);
        }
        public Result<int> Update(dynamic obj, CPQuery where) {
            List<String> fields = new List<string>();
            List<DbParameter> _parameters = new List<DbParameter>();
            var _tfilds = GetFields();
            foreach (var property in (IDictionary<String, Object>)obj)
            {
                var name = property.Key.ToLower();
                if (_tfilds.Contains(name))
                {
                    fields.Add(string.Format("`{0}` = @{0}", name));
                    _parameters.AddParam(name, property.Value);
                }
            }
            if (fields.Count > 0)
            {
                var sql = string.Format("update {0} set {1} where {2}", tableName, fields.Implode(","), where.ToString());
                Debug.WriteLine("SQL:" + sql);
                where.BindParameter(_parameters);//绑定where条件
                var count = DB.ExecuteNonQuery(sql, _parameters.ToArray());
                return Result<int>.ResultData(count);
            }
            else {
                return Result<int>.Error("无数据修改！");
            }
        }
        public Result<String> Delete(int id) {
            var where = CPQuery.New() + " id = " + id;
            return Delete(where);
        }
        public Result<String> Delete(CPQuery where) {
            List<DbParameter> _parameters = new List<DbParameter>();
            var sql = string.Format("delete from {0} where {1} ", tableName, where.ToString());
            where.BindParameter(_parameters);//绑定where条件
            
            var c= DB.ExecuteNonQuery(sql, _parameters.ToArray());
            if (c > 0) {
                return Result<String>.Success("删除成功！");
            }
            return Result<String>.Error("删除失败！");
        }
        public dynamic Find(int id) {
            var where = CPQuery.New() + " id = " + id;
            return Find(where);
        }
       
        public dynamic Find(CPQuery where) {
            List<DbParameter> _parameters = new List<DbParameter>();
            where.BindParameter(_parameters);//绑定where条件
            var sql = string.Format("select *  from {0} where {1} ", tableName, where.ToString());
            var reader = DB.ExecuteDataReader(sql, _parameters.ToArray());
            var lis = new List<dynamic>();
            if(reader.Read())
            {
                Dictionary<String, Object> dic = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    dic.Add(reader.GetName(i), reader[i]);
                }
                reader.Close();
                return new DynamicDataObject(dic);
            }
            return null;
        }
        public int QueryCount(CPQuery where = null) {
            return QueryCount(tableName, where);
        }
        public int QueryCount(string table,CPQuery where=null) {
            if (where == null)
            {
                where = CreateWhere();
            }
            List<DbParameter> _parameters = new List<DbParameter>();
            where.BindParameter(_parameters);//绑定where条件
            var countSql = String.Format("select count(1) cp_count from {0} where {1} ", table, where);
            return DB.ExecuteScalar<int>(countSql, _parameters.ToArray());
        }
        public PagedList<dynamic> PageList(String table,String fields, CPQuery where=null, int pageIndex = 1, int pageSize = 20) {
            if (where == null)
            {
                where = CreateWhere();
            }
            List<DbParameter> _parameters = new List<DbParameter>();
            where.BindParameter(_parameters);//绑定where条件
            var countSql = String.Format("select count(1) cp_count from {0} where  {1} ",table,where);
            var selectSql= String.Format("select {2} from {0} where  {1} limit {3},{4}", table, where,fields,(pageIndex-1)* pageSize, pageSize);
            var reader = DB.ExecuteDataReader(countSql+";"+selectSql, _parameters.ToArray());
            var totalCount = 0;
            if (reader.Read())
            {
                totalCount = reader.GetInt32("cp_count");
            }
            else {
                return new PagedList<dynamic>();
            }
            reader.NextResult();
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
            return new PagedList<dynamic>(lis, totalCount, pageIndex, pageSize);
        }

        public List<dynamic> Query(CPQuery where = null)
        {
            if (where == null)
            {
                where = CPQuery.New();
            }
            List<DbParameter> _parameters = new List<DbParameter>();
            where.BindParameter(_parameters);//绑定where条件
            var sql = string.Format("select *  from {0} where {1} ", tableName, where.ToString());
            var reader = DB.ExecuteDataReader(sql, _parameters.ToArray());
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
        public List<dynamic> Query(String table,String fields,CPQuery where = null)
        {
            if (where == null)
            {
                where = CreateWhere();
            }
            List<DbParameter> _parameters = new List<DbParameter>();
            where.BindParameter(_parameters);//绑定where条件
            var sql = string.Format("select {2}  from {0} where {1} ", table, where.ToString(), fields);
            var reader = DB.ExecuteDataReader(sql, _parameters.ToArray());
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
        public List<VT> QueryField<VT>(String field, CPQuery where = null) {
            return QueryField<VT>(tableName, field, where);
        }

        public List<VT> QueryField<VT>(string table, String field, CPQuery where = null) {
            if (where == null)
            {
                where = CreateWhere();
            }
            List<DbParameter> _parameters = new List<DbParameter>();
            where.BindParameter(_parameters);//绑定where条件
            var sql = string.Format("select distinct {2}  from {0} where {1} ", table, where.ToString(), field);
            var reader = DB.ExecuteDataReader(sql, _parameters.ToArray());
            var lis = new List<VT>();
            while (reader.Read())
            {
                lis.Add((VT)Convert.ChangeType(reader[field], typeof(VT)));
            }
            reader.Close();
            return lis;
        } 
    }
}
