namespace ZeroOne.Extension.Model
{
    /// <summary>
    /// 高德行政区域查询接口对象
    /// </summary>
    public class DistrictSettings
    {
        public string BaseUrl { get; set; }

        public string DistrictUrl { get; set; }
        /// <summary>
        /// 0：中华人民共和国，1：省，2：省、市，3：省、市、区，4：省、市、区、乡镇/街道
        /// </summary>
        public int SubDistrict { get; set; } = 3;
        /// <summary>
        /// 返回的数据格式JSON/XML，默认JSON
        /// </summary>
        public string Output { get; set; }
    }
}
