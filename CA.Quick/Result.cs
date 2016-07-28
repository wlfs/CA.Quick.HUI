using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.Quick
{
    /// <summary>
    /// 状态
    /// </summary>
    /// <typeparam name="T">数据对象泛型</typeparam>
    public class Result<T>
    {
        public int status { get; set; }
        public string info { get; set; }
        public T data { get; set; }
        /// <summary>
        /// 返回成功信息
        /// </summary>
        /// <param name="info">成功描述</param>
        /// <param name="_status">成功状态值</param>
        /// <returns></returns>
        public static Result<T> Success(string info="",int _status=1) {
            return new Result<T>() { status = _status, info = info };
        }
        /// <summary>
        /// 返回异常状态信息
        /// </summary>
        /// <param name="errorInfo">异常描述</param>
        /// <param name="_status">异常状态值</param>
        /// <returns></returns>
        public static Result<T> Error(string errorInfo, int _status = -1) {
            return new Result<T>() { status = _status, info = errorInfo };
        }
        /// <summary>
        /// 返回数据
        /// </summary>
        /// <typeparam name="DT">数据类型</typeparam>
        /// <param name="d">数据值</param>
        /// <param name="_status">状态</param>
        /// <param name="_info">描述</param>
        /// <returns></returns>
        public static Result<T> ResultData(T d, int _status = 1, string _info = "成功") {
            return new Result<T>() { status = _status, info = _info,data=d };
        }
    }
}
