using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroOne.Entity
{
    /// <summary>
    /// 产品返回对象
    /// </summary>
    public class ProInfoResponse : IResult
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
        /// 产品图片地址
        /// </summary>
        public string ProImg { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreationTime { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>
        public List<string> ThumbnailImgs { get; set; }
        /// <summary>
        /// 源图
        /// </summary>
        public List<string> SourceImgs { get; set; }
    }
}
