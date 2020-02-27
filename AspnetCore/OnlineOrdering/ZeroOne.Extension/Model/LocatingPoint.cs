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
}
