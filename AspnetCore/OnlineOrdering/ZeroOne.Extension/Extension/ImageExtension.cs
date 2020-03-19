using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace ZeroOne.Extension
{

    public static class ImageExtension
    {
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originImage">原始Image对象</param>
        /// <param name="thumbnailWay">生成缩略图方式</param>
        /// <param name="height">高度</param>
        /// <param name="width">宽度</param>
        /// <returns></returns>
        public static Image GenerateThumbImage(this Image originImage, EThumbnailWay thumbnailWay, int? height = null, int? width = null)
        {
            if (originImage == null)
            {
                throw new Exception();
            }
            //图片原始高度
            int originHeight = originImage.Height;
            //图片原始宽度
            int originWidth = originImage.Width;
            //算出比例
            float proportion = 0;
            int thumHeight = 0, thumWidth = 0;
            switch (thumbnailWay)
            {
                case EThumbnailWay.Height:
                    if (!height.HasValue)
                    {
                        throw new Exception();
                    }
                    thumHeight = height.Value;
                    proportion = (float)(thumHeight / originHeight);
                    thumWidth = (int)(originWidth * proportion);
                    break;
                case EThumbnailWay.Width:
                    if (!width.HasValue)
                    {
                        throw new Exception();
                    }
                    thumWidth = width.Value;
                    //算出比例
                    proportion = (float)(thumWidth / originWidth);
                    thumHeight = (int)(originHeight * proportion);
                    break;
                case EThumbnailWay.HW:
                default:
                    if (!height.HasValue || !width.HasValue)
                    {
                        throw new Exception();
                    }
                    thumHeight = height.Value;
                    thumWidth = width.Value;
                    break;
            }
            return originImage.GetThumbnailImage(thumWidth, thumHeight, null, IntPtr.Zero); ;
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originImagePath">原始Image地址</param>
        /// <param name="thumbnailWay">生成缩略图方式</param>
        /// <param name="height">高度</param>
        /// <param name="width">宽度</param>
        /// <returns></returns>
        public static Image GenerateThumbImage(this string originImagePath, EThumbnailWay thumbnailWay, int? height = null, int? width = null)
        {
            Image originImage = Image.FromFile(originImagePath);
            if (originImage == null)
            {
                throw new Exception();
            }
            //图片原始高度
            int originHeight = originImage.Height;
            //图片原始宽度
            int originWidth = originImage.Width;
            //算出比例
            float proportion = 0;
            int thumHeight = 0, thumWidth = 0;
            switch (thumbnailWay)
            {
                case EThumbnailWay.Height:
                    if (!height.HasValue)
                    {
                        throw new Exception();
                    }
                    thumHeight = height.Value;
                    proportion = (float)(thumHeight / originHeight);
                    thumWidth = (int)(originWidth * proportion);
                    break;
                case EThumbnailWay.Width:
                    if (!width.HasValue)
                    {
                        throw new Exception();
                    }
                    thumWidth = width.Value;
                    //算出比例
                    proportion = (float)(thumWidth / originWidth);
                    thumHeight = (int)(originHeight * proportion);
                    break;
                case EThumbnailWay.HW:
                default:
                    if (!height.HasValue || !width.HasValue)
                    {
                        throw new Exception();
                    }
                    thumHeight = height.Value;
                    thumWidth = width.Value;
                    break;
            }
            return originImage.GetThumbnailImage(thumWidth, thumHeight, null, IntPtr.Zero); ;
        }

        // using System.Drawing;


        //public static Image GenerateThumbImage(Image originalImage, int limitedSize)

        //    {

        //        if (limitedSize < 20)

        //        {

        //            throw new InvalidObjectException("limitedSize");

        //        }

        //        int height = originalImage.Height;

        //        int width = originalImage.Width;

        //        if (height > limitedSize || width > limitedSize)

        //        {

        //            int newHeight, newWidth;

        //            if (height > width)

        //            {

        //                newHeight = limitedSize;

        //                double tmp = (limitedSize / height) * width;

        //                newWidth = (int)Math.Round(tmp);

        //            }

        //            else

        //            {

        //                newWidth = limitedSize;

        //                double tmp = (limitedSize / width) * height;

        //                newHeight = (int)Math.Round(tmp);

        //            }

        //            return originalImage.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero);

        //        }

        //        else

        //        {

        //            return originalImage;

        //        }

        //    }

        //这是一个设定长和宽最大值后，等比例缩略的方法
        //其中，InvalidObjectException是我自定义的Exception
        //而if (limitedSize < 20)这句，是我认为不该创建小于20px的缩略图。
        //代码中Image 类是System.Drawing.Image 。
    }
}
