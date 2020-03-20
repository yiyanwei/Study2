using System;
using SqlSugar;

namespace ZeroOne.Entity
{
    /// <summary>
    /// 上传文件对象
    /// </summary>
    [SugarTable("file_info")]
    public class FileInfo : IEntity<Guid?>
    {
        /// <summary>
        /// 
        /// </summary>
        public FileInfo()
        {

        }

        /// <summary>
        /// 主键id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Guid? Id { get; set; }

        /// <summary>
        /// 文件名（不包括扩展名）
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string FileExt { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 文件原始路径
        /// </summary>
        public string SourceFileUrl { get; set; }

        /// <summary>
        /// 缩略图文件路径
        /// </summary>
        public string TargetFileUrl { get; set; }

        /// <summary>
        /// 数据状态
        /// </summary>
        public int? DataStatus { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatorUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreationTime { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string LastModifierUserId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// 是否删除 false(0):未删除，true(1):已删除
        /// </summary>
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// 删除操作人Id
        /// </summary>
        public string DeleterUserId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeletionTime { get; set; }
    }
}