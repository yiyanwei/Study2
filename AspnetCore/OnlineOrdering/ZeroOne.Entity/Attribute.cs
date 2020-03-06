using System;
using System.Reflection;

namespace ZeroOne.Entity
{
    public class TableAttribute : Attribute
    {
        public TableAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }

    public class ColumnNameAttribute : Attribute
    {
        /// <summary>
        /// 数据库字段名称
        /// </summary>
        public string ColName { get; set; }

        public ColumnNameAttribute(string colName)
        {
            this.ColName = ColName;
        }
    }

    public class JoinTableRelationAttribute : Attribute
    {
        /// <summary>
        /// MainTableRelationAttribute类型EntityType的属性
        /// </summary>
        public string PropName { get; set; }
        /// <summary>
        /// 算术比较运算符
        /// </summary>
        public ECompareOperator CompareOperator { get; set; }
        /// <summary>
        /// 目标关联表类型
        /// </summary>
        public Type DestEntityType { get; set; }
        /// <summary>
        /// 目标关联字段
        /// </summary>
        public string DestRelPropName { get; set; }
        /// <summary>
        /// 逻辑运算符
        /// </summary>
        public ELogicalOperator LogicalOperator { get; set; }

        public JoinTableRelationAttribute(string propName, Type destEntityType, string destRelPropName,
            ECompareOperator compareOperator = ECompareOperator.Equal, ELogicalOperator logicalOperator = ELogicalOperator.None)
        {
            this.PropName = propName;
            this.DestEntityType = destEntityType;
            this.DestRelPropName = destRelPropName;
            this.CompareOperator = compareOperator;
            this.LogicalOperator = logicalOperator;
        }
    }

    /// <summary>
    /// 表关联主关联表
    /// </summary>
    public class MainTableRelationAttribute : Attribute
    {
        public Type EntityType { get; set; }

        public EJoinType JoinType { get; set; }

        public string DestPropName { get; set; }


        public MainTableRelationAttribute(Type entityType, EJoinType joinType = EJoinType.InnerJoin, string destPropName = null)
        {
            this.EntityType = entityType;
            this.JoinType = joinType;
            this.DestPropName = destPropName;
        }
    }

    /// <summary>
    /// 数据库操作特性
    /// </summary>
    public class DbOperationAttribute : Attribute
    {
        public ECompareOperator CompareOperator { get; set; }

        public ELogicalOperator LogicalOperator { get; set; }

        public Type EntityType { get; set; }

        public string PropName { get; set; }

        public int? GroupKey { get; set; }

        public int? ParentGroupKey { get; set; }

        public PropertyInfo Prop { get; set; }

        public object Value { get; set; }

        public DbOperationAttribute()
        {
            this.CompareOperator = ECompareOperator.Equal;
            this.LogicalOperator = ELogicalOperator.And;
        }

        public DbOperationAttribute(ECompareOperator compareOperator)
        {
            this.CompareOperator = compareOperator;
            this.LogicalOperator = ELogicalOperator.And;
        }

        public DbOperationAttribute(ECompareOperator compareOperator, ELogicalOperator logicalOperator)
        {
            this.CompareOperator = compareOperator;
            this.LogicalOperator = logicalOperator;
        }


        public DbOperationAttribute(Type entityType, string propName = null,
             int? groupKey = null, int? parentGroupKey = null, ECompareOperator compareOperator = ECompareOperator.Equal,
             ELogicalOperator logicalOperator = ELogicalOperator.And, ELogicalOperator parentLogicalOperator = ELogicalOperator.And)
        {
            this.CompareOperator = compareOperator;
            this.LogicalOperator = logicalOperator;
            this.EntityType = entityType;
            this.PropName = propName;
            this.GroupKey = groupKey;
            this.ParentGroupKey = parentGroupKey;
        }
    }
}
