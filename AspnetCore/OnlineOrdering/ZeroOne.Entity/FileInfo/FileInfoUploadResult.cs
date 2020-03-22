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

        public IList<string> SourceFileUrls { get; set; }

        public IList<string> TargetFileUrls { get; set; }
    }
}
