using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroOne.Extension.Model
{
    /// <summary>
    /// 上传设置对象
    /// </summary>
    public class UploadSettings
    {
        /// <summary>
        /// 图片原始保存地址
        /// </summary>
        public string SourceImgRootPath { get; set; }
        /// <summary>
        /// 缩略图保存地址
        /// </summary>
        public string ThumbnailImgRootPath { get; set; }
    }
}
