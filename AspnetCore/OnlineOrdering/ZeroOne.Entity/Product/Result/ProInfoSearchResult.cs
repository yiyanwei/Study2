using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroOne.Entity
{
    /// <summary>
    /// 产品信息
    /// </summary>
    [DbOrdering(nameof(ProInfo.CreationTime), EOrderRule.Desc, typeof(ProInfo))]
    public class ProInfoSearchResult : Result
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid? Id { get; set; }
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
        /// 产品基本单位
        /// </summary>
        public string ProBaseUnit { get; set; }

        /// <summary>
        /// 图片上传id
        /// </summary>
        public Guid? UploadId { get; set; }

        /// <summary>
        /// 产品图片地址
        /// </summary>
        public string ProImg { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        [MainTableRelation(typeof(ProCategory), EJoinType.InnerJoin)]
        [JoinTableRelation(nameof(ProCategory.Id),typeof(ProInfo),nameof(ProInfo.CategoryId))]
        public string CategoryName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreationTime { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [MainTableRelation(typeof(UserInfo), EJoinType.LeftJoin, nameof(UserInfo.Name))]
        [JoinTableRelation(nameof(UserInfo.Id), typeof(ProInfo), nameof(ProInfo.CreatorUserId))]
        public string RealName { get; set; }
    }
}
