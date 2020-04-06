using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroOne.Entity
{
    /// <summary>
    /// 供应商查询
    /// </summary>
    public class SupplierPageSearch : BasePageSearch
    {
        /// <summary>
        /// 供应商名称
        /// </summary>
        [DbOperation(typeof(Supplier), propName: nameof(Supplier.SupplierName), compareOperator: ECompareOperator.Contains, logicalOperator: ELogicalOperator.And)]
        public string LikeSupplierName { get; set; }
        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 供应商联系人
        /// </summary>
        public string ContactMan { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactPhone { get; set; }
        /// <summary>
        /// 创建开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 创建结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}
