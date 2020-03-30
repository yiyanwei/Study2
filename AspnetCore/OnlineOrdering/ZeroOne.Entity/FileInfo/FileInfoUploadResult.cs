using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroOne.Entity
{
    public class FileInfoUploadResult : IResult
    {
        /// <summary>
        /// 上传文件Id
        /// </summary>
        public Guid? UploadId { get; set; }

        /// <summary>
        /// 文件信息集合
        /// </summary>
        public IList<FileInfoResult> FileInfosResult { get; set; }

        //public IList<string> SourceFileUrls { get; set; }

        //public IList<string> TargetFileUrls { get; set; }
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
    /// 产品上传图片结果对象
    /// </summary>
    public class ProductUploadFileResult : Result
    {

        /// <summary>
        /// 上传Id
        /// </summary>
        public Guid? UploadId { get; set; }


        /// <summary>
        /// 文件地址
        /// </summary>
        [EntityPropName(nameof(FileInfo.TargetFileUrl))]
        public string Url { get; set; }

        /// <summary>
        /// 源文件地址
        /// </summary>
        [EntityPropName(nameof(FileInfo.SourceFileUrl))]
        public string SourceUrl { get; set; }
    }
}
