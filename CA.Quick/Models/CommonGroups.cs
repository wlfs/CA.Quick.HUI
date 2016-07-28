/*************************************************************************************
 * 创建时间：07/28/2016 15:06:01
 * 作    者： ivier
 * 说    明： 
 * 修改时间：
 * 修 改 人：
 *************************************************************************************/
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CA.Quick.Models
{
    /// <summary>
    /// Entity Model 
    /// </summary>    
    [Serializable]
    public class CommonGroups: BaseModel
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="keyword">查询关键词</param>
        /// <returns>动态数据集合</returns>
        public List<dynamic> List(String keyword) {
            var query=CPQuery.New() + " 1=1 ";
            return Query(query.FullLike("name", keyword));
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
                return "common_groups";
            }
        }
		#endregion
	}
}
