using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroOne.Entity
{
    /// <summary>
    /// 产品新增请求对象
    /// </summary>
    public class ProInfoAddRequest:AddEntity,IEntity<Guid?>, IAddRequest
    {
        public ProInfoAddRequest() : base()
        {
            this.Id = Guid.NewGuid();
        }

        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 产品分类Id
        /// </summary>
        public Guid? CategoryId { get; set; }

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
        /// 上传Id
        /// </summary>
        public Guid? UploadId { get; set; }
    }

    /// <summary>
    /// 产品修改请求对象
    /// </summary>
    public class ProInfoEditRequest : UpdateEntity, IEntity<Guid?>, IEditRequest
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 产品分类Id
        /// </summary>
        public Guid? CategoryId { get; set; }

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
    }
}
