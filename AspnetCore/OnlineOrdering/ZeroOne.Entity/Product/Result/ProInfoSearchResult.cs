using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroOne.Entity
{
    public class ProInfoSearchResult : Result
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProName { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProCode { get; set; }

        /// <summary>
        /// 产品描述
        /// </summary>
        public string ProDesc { get; set; }

        /// <summary>
        /// 产品图片地址
        /// </summary>
        public string ProImg { get; set; }

        [JoinTable(nameof(CategoryId), typeof(ProCategory), nameof(ProCategory.Id), EJoinType.InnerJoin)]
        public string CategoryName { get; set; }

        
        public Guid? CategoryId { get; set; }
    }
}
