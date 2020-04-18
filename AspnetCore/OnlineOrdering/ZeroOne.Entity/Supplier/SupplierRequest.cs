using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZeroOne.Entity
{
    /// <summary>
    /// 供应商新增请求对象
    /// </summary>
    public class SupplierAddRequest : AddEntity, IEntity<Guid>, IAddRequest
    {
        public SupplierAddRequest() : base()
        {
            this.Id = Guid.NewGuid();
        }

        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 供应商编号
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 供应商联系人
        /// </summary>
        [Required]
        public string ContactMan { get; set; }

        /// <summary>
        /// 供应商联系电话
        /// </summary>
        [Required]
        [Phone]
        public string ContactPhone { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        [Required]
        public Guid? Province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public Guid? City { get; set; }

        /// <summary>
        /// 县市区
        /// </summary>
        public Guid? District { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        [Required]
        public string ProvinceName { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 县市区
        /// </summary>
        public string DistrictName { get; set; }

        /// <summary>
        /// 供应商详细地址（不包括省市区）
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 营业执照
        /// </summary>
        public Guid? BusinessLicense { get; set; }
    }

    public class SupplierEditRequest : UpdateEntity, IEntity<Guid>, IEditRequest
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 供应商编号
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 供应商联系人
        /// </summary>
        [Required]
        public string ContactMan { get; set; }

        /// <summary>
        /// 供应商联系电话
        /// </summary>
        [Required]
        [Phone]
        public string ContactPhone { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        [Required]
        public Guid? Province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public Guid? City { get; set; }

        /// <summary>
        /// 县市区
        /// </summary>
        public Guid? District { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        [Required]
        public string ProvinceName { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 县市区
        /// </summary>
        public string DistrictName { get; set; }

        /// <summary>
        /// 供应商详细地址（不包括省市区）
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 营业执照
        /// </summary>
        public Guid? BusinessLicense { get; set; }
    }
}
