using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroOne.Entity
{
    /// <summary>
    /// 产品分类输出基类型
    /// </summary>
    public interface IProCategoryResult : IResult
    {

    }




    public class ProCategoryResult : BaseResult<Guid>, IProCategoryResult
    {

        /// <summary>
        /// 产品类别名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 父级分类Id
        /// </summary>
        public Guid ParentId { get; set; }
    }
}
