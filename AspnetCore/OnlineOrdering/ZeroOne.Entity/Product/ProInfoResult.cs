using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroOne.Entity
{
    public interface IProInfoResult : IResult
    {

    }

    /// <summary>
    /// 文件返回对象
    /// </summary>
    public class FileInfoResult
    {
        /// <summary>
        /// 文件Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 文件地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 源文件地址
        /// </summary>
        public string SourceUrl { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ProInfoSingleResult : BaseEntity<Guid>, IProInfoResult
    {
        /// <summary>
        /// 产品分类Id
        /// </summary>
        public Guid? CategoryId { get; set; }

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
        /// 产品的基本单位
        /// </summary>
        public string ProBaseUnit { get; set; }

        /// <summary>
        /// 上传文件对象
        /// </summary>
        public List<FileInfoResult> FileInfos { get; set; }
    }

    public class ProInfoResult : BaseEntity<Guid>, IProInfoResult
    {
        /// <summary>
        /// 产品分类Id
        /// </summary>
        public Guid? CategoryId { get; set; }

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
        /// 产品的基本单位
        /// </summary>
        public string ProBaseUnit { get; set; }

    }
}
