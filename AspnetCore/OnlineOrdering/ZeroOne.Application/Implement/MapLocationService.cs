using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroOne.Extension;

namespace ZeroOne.Application
{
    public class MapBaseResponse
    {
        public string status { get; set; }

        public string info { get; set; }

        public string infocode { get; set; }
    }

    public class MapRoute
    {
        public string origin { get; set; }
        public string destination { get; set; }
        public List<MapPath> paths { get; set; }
    }

    public class MapPath
    {
        public string distance { get; set; }

        public string duration { get; set; }

        public string strategy { get; set; }

        public string tolls { get; set; }

        public string toll_distance { get; set; }

        public List<MapStep> steps { get; set; }
    }

    public class MapStep
    {
        public string instruction { get; set; }
        public string orientation { get; set; }
        public string road { get; set; }
        public string distance { get; set; }
        public string tolls { get; set; }
        public string toll_distance { get; set; }
        //public string toll_road { get; set; }
        public string duration { get; set; }
        public string polyline { get; set; }
        public string action { get; set; }
        public string assistant_action { get; set; }
    }


    public class MapDirectionDrive : MapBaseResponse
    {
        public string count { get; set; }

        public MapRoute route { get; set; }
    }



    /// <summary>
    /// 地图定位设置对象
    /// </summary>
    public class MapLocationSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Output { get; set; }
    }

    public enum EDriveDistanceType
    {
        /// <summary>
        /// 速度优先
        /// </summary>
        SpeedFirst = 0,
        /// <summary>
        /// 费用优先
        /// </summary>
        FeeFirst = 1,
        /// <summary>
        /// 距离优先
        /// </summary>
        DistanceFirst = 2,
        /// <summary>
        /// 不走快速路
        /// </summary>
        UnPassFastRoad = 3,
        /// <summary>
        /// 躲避拥堵
        /// </summary>
        AvoidTrafficJams = 4,
        /// <summary>
        /// 多策略（同时使用速度优先、费用优先、距离优先三个策略计算路径）
        /// </summary>
        MoreStrategy = 5,
        /// <summary>
        /// 不走高速
        /// </summary>
        NotGoHighSpeed = 6,
        /// <summary>
        /// 不走高速且避免收费
        /// </summary>
        NotGoHighSpeedAndNotFee = 7,
        /// <summary>
        /// 躲避收费和拥堵
        /// </summary>
        NotFeeAndAvoidTrafficJams = 8,
        /// <summary>
        /// 不走高速且躲避收费和拥堵
        /// </summary>
        NotHSFeeTrafficJams = 9

    }

    public class TruckDirectionDriveRequest
    {
        public string key { get; set; }

        public string origin { get; set; }

        public string destination { get; set; }

        public string strategy { get; set; }

        public string height { get; set; }

        public string width { get; set; }
        /// <summary>
        /// 用汉字填入车牌省份缩写。用于判断是否限行 
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 填入除省份及标点之外的字母和数字（需大写），用于判断限行相关。
        /// 支持6位传统车牌和7位新能源车牌。
        /// </summary>
        public string number { get; set; }
    }

    public enum ETruckDriveDistanceType
    {
        /// <summary>
        /// 返回的结果考虑路况，尽量躲避拥堵而规划路径；对应导航SDK货导策略12
        /// </summary>
        AvoidTrafficeJams = 1,
        /// <summary>
        /// 不考虑路况，返回速度优先的路线，此路线不一定距离最短；如果不需要路况干扰计算结果，推荐使用此策略；（导航SDK货导策略无对应，真实导航时均会考虑路况）
        /// </summary>
        SpeedFirst = 10
        //1，返回的结果考虑路况，尽量躲避拥堵而规划路径；对应导航SDK货导策略12；

        //2，返回的结果考虑路况，不走高速；对应导航SDK货导策略13；

        //3，返回的结果考虑路况，尽可能规划收费较低甚至免费的路径；对应导航SDK货导策略14；

        //4，返回的结果考虑路况，尽量躲避拥堵，并且不走高速；对应导航SDK货导策略15；

        //5，返回的结果考虑路况，尽量不走高速，并且尽量规划收费较低甚至免费的路径结果；对应导航SDK货导策略16；

        //6，返回的结果考虑路况，尽量的躲避拥堵，并且规划收费较低甚至免费的路径结果；对应导航SDK货导策略17；

        //7，返回的结果考虑路况，尽量躲避拥堵，规划收费较低甚至免费的路径结果，并且尽量不走高速路；对应导航SDK货导策略18；

        //8，返回的结果考虑路况，会优先选择高速路；对应导航SDK货导策略19；

        //9，返回的结果考虑路况，会优先考虑高速路，并且会考虑路况躲避拥堵；对应导航SDK货导策略20；

        //10，
        //11，返回的结果会考虑路况，躲避拥堵，速度优先以及费用优先；500Km规划以内会返回多条结果，500Km以外会返回单条结果；考虑路况情况下的综合最优策略，推荐使用；对应导航SDK货导策略10；
    }

    //  /**
    //* 高德地图WebAPI : 驾车路径规划 计算两地之间行驶的距离(米)
    //* String origins:起始坐标
    //* String destination:终点坐标
    //*/

    /// <summary>
    /// 地图定位服务类（高德地图）
    /// </summary>
    public class MapLocationService : IMapLocationService
    {
        protected MapLocationSettings MLSettingOpt { get; set; }
        public MapLocationService(IOptions<MapLocationSettings> mlSettingOpt)
        {
            this.MLSettingOpt = mlSettingOpt.Value;
        }

        private static readonly string BASE_PATH = "https://restapi.amap.com/v3";
        /// <summary>
        /// 高德地图WebAPI : 驾车路径规划
        /// String origins:起始坐标
        /// String destination:终点坐标
        /// </summary>
        /// <param name="origin">起始地址坐标</param>
        /// <param name="destination">目的地地址坐标</param>
        /// <returns></returns>
        public async Task<MapDirectionDrive> GetDirectionDrive(string origin, string destination, EDriveDistanceType driveDistanceType)
        {
            /**
             * 0:速度优先（时间）; 1:费用优先（不走收费路段的最快道路）;2:距离优先; 3:不走快速路 4躲避拥堵;
             * 5:多策略（同时使用速度优先、费用优先、距离优先三个策略计算路径）;6:不走高速; 7:不走高速且避免收费;
             * 8:躲避收费和拥堵; 9:不走高速且躲避收费和拥堵
             */
            string url = BASE_PATH + "/direction/driving?" + "origin=" + origin + "&destination=" + destination
                    + "&strategy=" + (int)driveDistanceType + "&extensions=base&key=" + MLSettingOpt.Key;
            var mapDrive = await this.GetResponse<MapDirectionDrive>(url, 10, t =>
            {
                var temp = System.Text.RegularExpressions.Regex.Replace(t, "\"action\":.?\\[\\]", "\"action\": null");
                return System.Text.RegularExpressions.Regex.Replace(temp, "\"assistant_action\":.?\\[\\]", "\"assistant_action\": null");
            });
            return mapDrive;
        }

        //public async Task<float> GetDriveDistanceFirst(string origin, string destination)
        //{
        //    var directionDrive = await this.GetDirectionDrive(origin, destination, EDriveDistanceType.SpeedFirst);
        //    if (directionDrive?.route?.paths?.Count > 0)
        //    {
        //        string distance = directionDrive.route.paths[0].distance;

        //    }
        //}
    }
}
