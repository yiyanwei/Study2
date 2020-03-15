using System;

namespace ZeroOne.Entity
{
    /// <summary>
    /// 产品分类查询对象
    /// </summary>
    public class ProCategorySearch : BaseSearch
    {
        [DbOperation(entityType: typeof(ProCategory), compareOperator: ECompareOperator.Contains)]
        public string CategoryName { get; set; }

        public Guid? ParentId { get; set; }

        [DbOperation(entityType: null, propName: nameof(ProCategory.CategoryName), mainTablePropName: nameof(ProCategorySearchResult.ParentCategoryName))]
        public string ParentCategoryName { get; set; }
    }

    /// <summary>
    /// 产品分类分页查询对象
    /// </summary>
    public class ProCategoryPageSearch : ProCategorySearch, IPageSearch
    {
        /// <summary>
        /// 页索引
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }
    }
}