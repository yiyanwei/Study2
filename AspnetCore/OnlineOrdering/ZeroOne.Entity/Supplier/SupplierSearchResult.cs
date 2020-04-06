using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroOne.Entity
{
    /// <summary>
    /// 供应商信息
    /// </summary>
    [DbOrdering(nameof(Supplier.CreationTime), EOrderRule.Desc, typeof(Supplier))]
    public class SupplierSearchResult : Result
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 供应商编号
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 供应商联系人
        /// </summary>
        public string ContactMan { get; set; }

        /// <summary>
        /// 供应商联系电话
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 县市区
        /// </summary>
        public string Prefecture { get; set; }

        /// <summary>
        /// 供应商详细地址（不包括省市区）
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 数据状态
        /// </summary>
        public int? DataStatus { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        [MainTableRelation(typeof(UserInfo), EJoinType.LeftJoin, nameof(UserInfo.Name))]
        [JoinTableRelation(nameof(UserInfo.Id), typeof(Supplier), nameof(Supplier.CreatorUserId))]
        public string RealName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreationTime { get; set; }

        /// <summary>
        /// 营业执照
        /// </summary>
        public Guid? BusinessLicense { get; set; }


        /// <summary>
        /// 缩略图
        /// </summary>
        [ResultPropIgnore]
        public List<string> ThumbnailImgs { get; set; }
        /// <summary>
        /// 源图
        /// </summary>
        [ResultPropIgnore]
        public List<string> SourceImgs { get; set; }
    }
}
