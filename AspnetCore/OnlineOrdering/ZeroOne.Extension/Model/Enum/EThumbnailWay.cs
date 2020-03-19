using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroOne.Extension
{
    /// <summary>
    /// 缩放方式
    /// </summary>
    public enum EThumbnailWay
    {
        /// <summary>
        /// 按照高度等比例缩放
        /// </summary>
        Height = 1,
        /// <summary>
        /// 按照宽度等比例缩放
        /// </summary>
        Width = 2,
        /// <summary>
        /// 按照高宽缩放
        /// </summary>
        HW = 3
    }
}
