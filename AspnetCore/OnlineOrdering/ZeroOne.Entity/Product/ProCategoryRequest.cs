using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroOne.Entity
{

    /// <summary>
    /// 产品分类新增请求对象
    /// </summary>
    public class ProCategoryAddRequest : AddEntity, IEntity<Guid>, IAdd
    {
        public ProCategoryAddRequest() : base()
        {
            this.Id = Guid.NewGuid();
        }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 父级id
        /// </summary>
        public Guid ParentId { get; set; }
        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid Id { get; set; }
    }

    /// <summary>
    /// 产品分类编辑请求对象
    /// </summary>
    public class ProCategoryEditRequest : UpdateEntity, IEntity<Guid>, IUpdated
    {
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 父级id
        /// </summary>
        public Guid ParentId { get; set; }
        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid Id { get; set; }
    }
}
