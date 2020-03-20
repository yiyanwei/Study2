namespace ZeroOne.Extension.Model
{
    /// <summary>
    /// 图片上传参数对象
    /// </summary>
    public class ImageUploadParameter
    {
        /// <summary>
        /// 缩放方式
        /// </summary>
        public EThumbnailWay ThumbnailWay { get; set; }
        /// <summary>
        /// 如果指定了按照高度缩放或高宽缩放，提供此参数
        /// </summary>
        public int? Height { get; set; }
        /// <summary>
        /// 如果制定了按找宽度缩放或高宽缩放，提供此参数
        /// </summary>
        public int? Width { get; set; }

    }
}
