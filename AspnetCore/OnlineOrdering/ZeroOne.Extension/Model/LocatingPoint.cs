using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroOne.Extension.Model
{
    /// <summary>
    /// 经纬度
    /// </summary>
    public struct LocatingPoint
    {

        /// <summary>
        /// 经纬度
        /// </summary>
        /// <param name="latitude">纬度</param>
        /// <param name="longitude">经度</param>
        public LocatingPoint(double latitude,double longitude)
        {
            this.Longitude = longitude;
            this.Latitude = latitude;
        }

        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }

    }

    public static class LocatingPointExtension
    {
        private static LocatingLine LocatingLine = new LocatingLine();

        /// <summary>
        /// 获取两个经纬度之间的距离
        /// </summary>
        /// <param name="start">起始经纬度对象</param>
        /// <param name="end">结束经纬度对象</param>
        /// <returns></returns>
        public static double GetDistance(this LocatingPoint start, LocatingPoint end)
        {
            return LocatingLine.GetDistance(start, end);
        }

        /// <summary>
        /// 获取两个经纬度之间的距离
        /// </summary>
        /// <param name="start">起始经纬度对象</param>
        /// <param name="latitude">结束纬度</param>
        /// <param name="longitude">结束经度</param>
        /// <returns></returns>
        public static double GetDistance(this LocatingPoint start, double latitude, double longitude)
        {
            return LocatingLine.GetDistance(start, new LocatingPoint(latitude,longitude));
        }
    }
}
