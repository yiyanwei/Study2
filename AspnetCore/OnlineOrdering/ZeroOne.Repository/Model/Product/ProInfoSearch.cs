using System;
using System.Collections.Generic;

namespace ZeroOne.Repository
{
    /// <summary>
    /// 产品分类查询类
    /// </summary>
    public class ProInfoSearch : BaseSearch
    {
        public string ProName{get;set;}

        public IList<int> DataStatus { get; set; }
    }
}