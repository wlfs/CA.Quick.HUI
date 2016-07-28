/*************************************************************************************
 * 创建时间：07/28/2016 15:06:01
 * 作    者： ivier
 * 说    明： 
 * 修改时间：
 * 修 改 人：
 *************************************************************************************/
using System;
using System.Runtime.Serialization;

namespace CA.Quick.Models
{
    /// <summary>
    /// Entity Model 
    /// </summary>    
    [Serializable]
    public class CommonGroupAction: BaseModel
    {
		
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
                return "common_group_action";
            }
        }
		#endregion
	}
}
