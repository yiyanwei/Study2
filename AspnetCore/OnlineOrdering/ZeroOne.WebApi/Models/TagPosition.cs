using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZeroOne.WebApi
{
    /// <summary>
    /// 标签位置
    /// </summary>
    public class TagPosition
    {
        public int? code { get; set; }

        public string message { get; set; }

        public string status { get; set; }

        public long? responseTS { get; set; }

        public IList<TagPositionItem> tags { get; set; }
    }

    /// <summary>
    /// 区域
    /// </summary>
    public class Zone
    {
        /// <summary>
        /// 区域Id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string name { get; set; }
    }

    /// <summary>
    /// 位置详情
    /// </summary>
    public class TagPositionItem
    {
        public string id { get; set; }

        public string name { get; set; }
        /// <summary>
        /// 平滑后的标签位置，包含 x，y，z 坐标，以米为单位
        /// </summary>
        public float[] smoothedPosition { get; set; }
        /// <summary>
        /// 从定位系统中获取的标签原始位置，包括 x， y，z 坐标，以米为单位
        /// </summary>
        public float[] position { get; set; }

        public long? positionTS { get; set; }

        //public float? smoothedPositionAccuracy { get; set; }
        //public float? positionAccuracy { get; set; }

        public string coordinateSystemId { get; set; }

        public string coordinateSystemName { get; set; }
        /// <summary>
        /// 标签位置计算时对应的跟踪区域 ID
        /// </summary>
        public string areaId { get; set; }
        /// <summary>
        /// 标签位置计算时对应的区域名称
        /// </summary>
        public string areaName { get; set; }
        /// <summary>
        /// 区域对象数组（每个区域对象都有'id'和'name'键）。如
        /// 果标签的位置为空数组，或者不在任何区域内，则为
        /// null（标签的位置无法计算）。该区域可能用于例如地
        /// 理围栏应用程序，标签也可能同时位于多个重叠区域。
        /// </summary>
        public IList<Zone> zones { get; set; }
        //{
        //    "id": "51dac207a006",
        //    "smoothedPosition": [
        //        12.25,
        //        -19,
        //        1
        //    ],
        //    "position": [
        //        12.25,
        //        -19,
        //        1
        //    ],
        //    "positionTS": 1584413037209,
        //    "smoothedPositionAccuracy": 0.35,
        //    "positionAccuracy": 0.35,
        //    "coordinateSystemId": "coordinateSystemId9000",
        //    "coordinateSystemName": "coordinateSystemName9000",
        //    "zones": [
        //        {
        //            "id": "4",
        //            "name": "模拟区域4"
        //        }
        //    ]
        //}
    }
}
