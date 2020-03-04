using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroOne.Entity
{
    /// <summary>
    /// 查询结果对象
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class BaseSearchResult<TResult>
        where TResult : IResult
    {
        /// <summary>
        /// 返回的数据列表
        /// </summary>
        public IList<TResult> Items { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
