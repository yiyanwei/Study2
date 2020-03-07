using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroOne.Entity
{
    /// <summary>
    /// 分页查询结果对象
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class PageSearchResult<TResult> : BaseSearchResult<TResult>
        where TResult : IResult
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
