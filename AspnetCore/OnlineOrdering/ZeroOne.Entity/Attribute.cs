using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ZeroOne.Entity
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public TableAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
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

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class JoinTableRelationAttribute : Attribute
    {
        /// <summary>
        /// MainTableRelationAttribute类型EntityType的属性
        /// </summary>
        public string PropName { get; set; }
        /// <summary>
        /// 当前类型属性
        /// </summary>
        public PropertyInfo Property { get; set; }
        /// <summary>
        /// 算术比较运算符
        /// </summary>
        public ECompareOperator CompareOperator { get; set; }
        /// <summary>
        /// 目标关联表类型
        /// </summary>
        public Type DestEntityType { get; set; }
        /// <summary>
        /// 目标关联字段名称
        /// </summary>
        public string DestRelPropName { get; set; }
        /// <summary>
        /// 目标类型属性
        /// </summary>
        public PropertyInfo DestProperty { get; set; }
        /// <summary>
        /// 逻辑运算符
        /// </summary>
        public ELogicalOperator LogicalOperator { get; set; }

        /// <summary>
        /// 如果不是两个关联表比较，与值比较
        /// </summary>
        public object PropValue { get; set; }
        /// <summary>
        /// 如果不是两个关联表比较，目标值比较
        /// </summary>
        public object DestPropValue { get; set; }
        /// <summary>
        /// 主表的属性
        /// </summary>
        public string MainTableAttrPropName { get; set; }

        public JoinTableRelationAttribute(string propName, Type destEntityType, string destRelPropName,
            ECompareOperator compareOperator = ECompareOperator.Equal, ELogicalOperator logicalOperator = ELogicalOperator.None,
            object propVal = null, object destPropVal = null, string mainTableAttrPropName = null)
        {
            this.PropName = propName;
            this.DestEntityType = destEntityType;
            this.DestRelPropName = destRelPropName;
            this.CompareOperator = compareOperator;
            this.LogicalOperator = logicalOperator;
            this.PropValue = propVal;
            this.DestPropValue = destPropVal;
            this.MainTableAttrPropName = mainTableAttrPropName;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class EntityPropNameAttribute : Attribute
    {

        public string PropName { get; set; }
        public EntityPropNameAttribute(string propName)
        {
            this.PropName = propName;
        }
    }

    /// <summary>
    /// 表关联主关联表
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class MainTableRelationAttribute : Attribute
    {
        public Type EntityType { get; set; }

        public EJoinType JoinType { get; set; }

        public string DestPropName { get; set; }

        public bool IsTogetherSampleType { get; set; }


        public MainTableRelationAttribute(Type entityType, EJoinType joinType = EJoinType.InnerJoin, string destPropName = null, bool isTogetherSampleType = true)
        {
            this.EntityType = entityType;
            this.JoinType = joinType;
            this.DestPropName = destPropName;
            this.IsTogetherSampleType = isTogetherSampleType;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DbOrderingAttribute : Attribute
    {
        public string PropName { get; set; }

        public EOrderRule OrderRule { get; set; }

        public Type EntityType { get; set; }

        public string MainTablePropName { get; set; }

        public DbOrderingAttribute(string propName, EOrderRule orderRule, Type entityType)
        {
            this.PropName = propName;
            this.OrderRule = orderRule;
            this.EntityType = entityType;
        }

        public DbOrderingAttribute(string propName, EOrderRule orderRule, string mainTablePropName)
        {
            this.PropName = propName;
            this.OrderRule = orderRule;
            this.MainTablePropName = mainTablePropName;
        }
    }

    public class DbOperationGroupExpression
    {
        public int? GroupKey { get; set; }

        public ELogicalOperator ParentLogicalOperator { get; set; }

        public int? ParentGroupKey { get; set; }

        public Expression TotalExpression { get; set; }

        public DbOperationGroupExpression(int? groupKey, ELogicalOperator parentLogicalOper, int? parentGroupKey, Expression totalExp)
        {
            this.GroupKey = groupKey;
            this.ParentLogicalOperator = parentLogicalOper;
            this.ParentGroupKey = parentGroupKey;
            this.TotalExpression = totalExp;
        }
    }

    /// <summary>
    /// 数据库操作特性
    /// </summary>
    public class DbOperationAttribute : Attribute
    {
        public string MainTablePropName { get; set; }

        public ECompareOperator CompareOperator { get; set; }

        public ELogicalOperator LogicalOperator { get; set; }

        public Type EntityType { get; set; }

        public string PropName { get; set; }

        public int? GroupKey { get; set; }

        public int? ParentGroupKey { get; set; }

        public ELogicalOperator ParGroupLogicalOperator { get; set; }

        public PropertyInfo Prop { get; set; }

        public object Value { get; set; }

        public DbOperationAttribute()
        {
            this.CompareOperator = ECompareOperator.Equal;
            this.LogicalOperator = ELogicalOperator.And;
            this.ParGroupLogicalOperator = ELogicalOperator.And;
        }

        public DbOperationAttribute(ECompareOperator compareOperator)
        {
            this.CompareOperator = compareOperator;
            this.LogicalOperator = ELogicalOperator.And;
            this.ParGroupLogicalOperator = ELogicalOperator.And;
        }

        public DbOperationAttribute(ECompareOperator compareOperator, ELogicalOperator logicalOperator)
        {
            this.CompareOperator = compareOperator;
            this.LogicalOperator = logicalOperator;
            this.ParGroupLogicalOperator = ELogicalOperator.And;
        }


        public DbOperationAttribute(Type entityType, string propName = null,
             int groupKey = 0, int parentGroupKey = 0, string mainTablePropName = null, ECompareOperator compareOperator = ECompareOperator.Equal,
             ELogicalOperator logicalOperator = ELogicalOperator.And, ELogicalOperator parGroupLogicalOperator = ELogicalOperator.And)
        {
            this.MainTablePropName = mainTablePropName;
            this.CompareOperator = compareOperator;
            this.LogicalOperator = logicalOperator;
            this.EntityType = entityType;
            this.PropName = propName;
            this.GroupKey = groupKey == 0 ? null : (int?)groupKey;
            this.ParentGroupKey = parentGroupKey == 0 ? null : (int?)parentGroupKey;
            this.ParGroupLogicalOperator = ELogicalOperator.And;
        }
    }
}
