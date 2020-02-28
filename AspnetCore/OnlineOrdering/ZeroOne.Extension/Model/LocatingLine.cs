using System;

namespace ZeroOne.Extension.Model
{
    /// <summary>
    /// 定位距离对象
    /// </summary>
    public class LocatingLine
    {
        public LocatingLine()
        {

        }

        public LocatingLine(LocatingPoint left, LocatingPoint right)
        {
            this.Start = left;
            this.End = right;
        }
        /// <summary>
        /// 定位起始点
        /// </summary>
        public LocatingPoint Start { get; set; }
        /// <summary>
        /// 定位结束点
        /// </summary>
        public LocatingPoint End { get; set; }

        public double Distance
        {
            get
            {
                return this.CalculateDinstance(Start, End);
            }
        }


        //地球赤道半径，单位米
        //private const double EARTH_RADIUS = 6378137;
        //地球平均半径，单位米
        private const double EARTH_RADIUS = 6371000;

        /// <summary>
        /// 计算两点位置的距离，返回两点的距离，单位 米
        /// 该公式为GOOGLE提供，误差小于0.2米
        /// </summary>
        /// <param name="start">起始经纬度</param>
        /// <param name="lng1">结束经纬度</param>
        /// <returns></returns>
        public double GetDistance(LocatingPoint start, LocatingPoint end)
        {
            return CalculateDinstance(start, end);
        }

        /// <summary>
        /// 计算定位两点之间的距离
        /// </summary>
        /// <param name="start">起始点</param>
        /// <param name="end">结束点</param>
        /// <returns></returns>
        private double CalculateDinstance(LocatingPoint start, LocatingPoint end)
        {
            double radLat1 = Rad(start.Latitude);
            double radLng1 = Rad(start.Longitude);
            double radLat2 = Rad(end.Latitude);
            double radLng2 = Rad(end.Longitude);
            double a = radLat1 - radLat2;
            double b = radLng1 - radLng2;
            double result = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2))) * EARTH_RADIUS;
            return result;
        }



        /// <summary>
        /// 经纬度转化成弧度
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private double Rad(double d)
        {
            return (double)d * Math.PI / 180d;
        }
    }
}
