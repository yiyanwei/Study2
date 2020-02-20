using System;
using SqlSugar;

namespace ZeroOne.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class User_Info : BaseEntity<Guid>,IRowVersion
    {
        /// <summary>
        /// 
        /// </summary>
        public User_Info()
        {
        }

        /// <summary>
        /// 手机号
        /// </summary>
        public string MobileNum { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int? Sex { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 微信OpenId
        /// </summary>
        public string OpenId { get; set; }
        public Guid? RowVersion { get; set; }
    }
}