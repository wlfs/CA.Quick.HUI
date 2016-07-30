/*************************************************************************************
 * 创建时间：07/28/2016 15:06:01
 * 作    者： ivier
 * 说    明： 
 * 修改时间：
 * 修 改 人：
 *************************************************************************************/
using CA.Quick.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CA.Quick.Models
{
    /// <summary>
    /// Entity Model 
    /// </summary>    
    [Serializable]
    public class CommonAdmin : BaseModel
    {
        public PagedList<dynamic> pageLis(string keyword = "", int status = 0)
        {
            var where = CreateWhere();
            if (!string.IsNullOrEmpty(keyword))
            {
                where.FullLike("u.name|u.mobile|u.login_name", keyword);
            }
            if (status > 0)
            {
                where = where + " and u.status=" + status;
            }
            var data = PageList(tableName + " as u", "u.*", where);
            data.IntToString("status", GetStatus());
            return data;
        }
        public IDictionary<int, String> GetStatus()
        {
            var dic = new Dictionary<int, String>();
            dic.Add(1, "正常");
            dic.Add(2, "禁用");
            return dic;
        }
        /**
         * 添加管理员
         */
        public Result<int> addResult(dynamic d)
        {
            int lnl = d.login_name.Length;
            if (lnl > 4 && lnl <= 20)
            {

            }
            else
            {
                return Result<int>.Error("登录名必须是5到20位的字符串！");
            }
            d.created = DateTime.Now;
            var where = CreateWhere();
            where = where + " and  login_name=" + d.login_name.AsQueryParameter();
            var c = QueryCount(where);
            if (c > 0)
            {
                return Result<int>.Error("用户名已存在");
            }
            d.salt = Helper.RndNum(4);
            d.password = Helper.MD5(Helper.MD5(d.password) + d.sqlt);
            d.last_login_time = DateTime.Now;
            //$d['last_login_ip'] = get_client_ip();
            //$d['last_login_address'] = getIPAddress($d['last_login_ip']);
            Result<int> r = AddResult(d);
            //添加用户组
            if (d.group_ids.Length > 0)
            {
                var lis = new List<dynamic>();
                foreach (var item in d.group_ids)
                {
                    var g = CreateDynamic();
                    g.admin_id = r.data;
                    g.group_id = item;
                    lis.Add(g);
                }
                new CommonAdminGroup().AddAll(lis);
            }
            return r;
        }
        public void updateResult(dynamic d, int id)
        {
            Update(d, id);
            var agModel = new CommonAdminGroup();
            var where = CreateWhere();
            where = where + " and admin_id=" + id;
            agModel.Delete(where);
            if (d.group_ids.Length > 0)
            {
                var lis = new List<dynamic>();
                foreach (var item in d.group_ids)
                {
                    var g = CreateDynamic();
                    g.admin_id = id;
                    g.group_id = item;
                    lis.Add(g);
                }
                agModel.AddAll(lis);
            }


        }

        /**
         * 保存权限组
         * @param  int $id   管理员编号
         * @param  array $gids 权限组编号数组
         * @return [type]       
         */
        public void  saveGroups(int id,int[] gids)
        {
            var agModel = new CommonAdminGroup();
            var where = CreateWhere();
            where = where + " and admin_id=" + id;
            agModel.Delete(where);
            var lis = new List<dynamic>();
            foreach (var item in gids)
            {
                var g = CreateDynamic();
                g.admin_id = id;
                g.group_id = item;
                lis.Add(g);
            }
            agModel.AddAll(lis);
        }
        
        public Result<dynamic> Login(string uname, string pass)
        {
            var where = CreateWhere();
            where = where + " and login_name=" + uname.AsQueryParameter();
            var ainfo=Find(where);
            if (ainfo!=null) {
                //验证用户密码是否正确
                if (ainfo.password == Helper.MD5(pass+ainfo.salt)){
                    var up=CreateDynamic();
                    up.last_login_time = DateTime.Now;
                    Update(up, ainfo.id);
                    //ainfo.password = "";
                    //ainfo.salt = "";
                    var lrModel = new CommonLoginRecord();
                    var lrd = CreateDynamic();
                    lrd.admin_id = ainfo.id;
                    lrModel.Add(lrd);
                    return Result<dynamic>.ResultData(ainfo);
                } else {
                    return Result<dynamic>.Error("用户名或密码错误");
                }
            }
            return Result<dynamic>.Error("用户名或密码错误");
        }
        /**
         * 重置密码
         * @param  int $userId 用户编号
         * @return resultObj        
         */
        public void ResetPwd(int id,string pwd)
        {
            var d=CreateDynamic();
            d.salt = Helper.RndNum(4);
            d.password = Helper.MD5(Helper.MD5(pwd) + d.salt);
            Update(d, id);
        }

        /**
         * 重置密码
         * @param  int $userId 用户编号
         * @return resultObj        
         */
        public Result<string> resetPwd2(int id,string oldPass,string pwd)
        {
            var info = Find(id);
            if (info.password == Helper.MD5(Helper.MD5(oldPass)+info.salt)) {
                var d = CreateDynamic();
                d.salt = Helper.RndNum(4);
                d.password = Helper.MD5(Helper.MD5(pwd) + d.salt);
                Update(d, id);
                return Result<string>.Success("修改成功");
            } else {
                return Result<string>.Error("密码错误！");
            }
        }

        public List<string> GetActions(int id)
        {
            var where = CreateWhere();
            where = where + " and ug.admin_id=" + id;
            return QueryField<string>("common_admin_group ug join common_group_action  ga on ga.group_id=ug.group_id join common_actions a on a.id=ga.action_id ", "code",where);
        }

        /**
         * 设置状态
         * @param  int $userId 用户编号
         * @param  int $state  用户状态
         * @return [type]         [description]
         */
        public void updateStatus(int id, int state)
        {
            var d = CreateDynamic();
            d.status = state;
            Update(d, id);
        }

        /**
         * 禁用
         * @param [type] $user_id 用户编号
         */
        public void Disable(int id)
        {
            updateStatus(id, 2);
        }

        /**
         * 恢复
         * @param  [type] $user_id [description]
         * @return [type]          [description]
         */
        public void Recovery(int id)
        {
            updateStatus(id, 1);
        }

        /**
         * 删除
         * @param  [type] $id [description]
         * @return [type]     [description]
         */
        public new Result<string> Delete(int id)
        {
            if (id == 1)
            {
                return Result<string>.Error("超级管理员不能删除");
            }
            else
            {
                return base.Delete(id);
            }
        }

        #region 自动生成
        protected override string pkName
        {
            get
            {
                return "id";
            }
        }

        protected override string tableName
        {
            get
            {
                return "common_admin";
            }
        }
        #endregion
    }
}
