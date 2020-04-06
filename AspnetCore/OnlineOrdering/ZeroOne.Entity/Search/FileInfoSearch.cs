using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroOne.Entity
{
    public class UploadFileSearch : BaseSearch
    {
        /// <summary>
        /// 产品对应的上传图片Uploadid
        /// </summary>
        [DbOperation(logicalOperator: ELogicalOperator.None, compareOperator: ECompareOperator.Contains)]
        public IList<Guid> UploadId { get; set; }
    }
}
