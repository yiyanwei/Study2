namespace ZeroOne.Entity
{
    /// <summary>
    /// 批量导入产品对象
    /// </summary>
    public class ProInfoBulk : BaseEntity, IBulkModel
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProName { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProCode { get; set; }

        /// <summary>
        /// 产品描述
        /// </summary>
        public string ProDesc { get; set; }

        /// <summary>
        /// 产品图片地址
        /// </summary>
        public string ProImg { get; set; }

        /// <summary>
        /// 产品的基本单位
        /// </summary>
        public string ProBaseUnit { get; set; }

        /// <summary>
        /// 批量导入标识
        /// </summary>
        public string BulkIdentity { get; set; }
    }
}
