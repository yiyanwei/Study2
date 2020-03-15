using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroOne.Entity
{
    /// <summary>
    /// 产品分类查询结果
    /// </summary>
    [DbOrdering(nameof(ProCategory.CreationTime),EOrderRule.Desc,typeof(ProCategory))]
    [DbOrdering(nameof(ProCategory.CreationTime), EOrderRule.Desc,nameof(ParentCategoryName))]
    public class ProCategorySearchResult : Result
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 父级分类名称
        /// </summary>
        [MainTableRelation(typeof(ProCategory), EJoinType.LeftJoin, nameof(ProCategory.CategoryName), false)]
        [JoinTableRelation(nameof(ProCategory.Id), typeof(ProCategory), nameof(ProCategory.ParentId))]
        public string ParentCategoryName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreationTime { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [MainTableRelation(typeof(UserInfo), EJoinType.LeftJoin, nameof(UserInfo.Name))]
        [JoinTableRelation(nameof(UserInfo.Id), typeof(ProCategory), nameof(ProCategory.CreatorUserId))]
        public string RealName { get; set; }
    }
}
