using System;

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

    /// <summary>
    /// 关联对象
    /// </summary>
    public class JoinTableAttribute : Attribute
    {
        public string MyJoinProp { get; set; }

        public Type EntityType { get; set; }

        public string JoinProp { get; set; }

        public EJoinType JoinType { get; set; }

        public string DestProp { get; set; }

        public JoinTableAttribute(string myJoinProp, Type entityType, string joinProp, EJoinType joinType = EJoinType.InnerJoin, string destProp = null)
        {
            this.MyJoinProp = myJoinProp;
            this.EntityType = entityType;
            this.JoinProp = joinProp;
            this.JoinType = JoinType;
            this.DestProp = destProp;
        }
    }

    public class DbOperationAttribute : Attribute
    {
        public ECompareOperator CompareOperator { get; set; }

        public ELogicalOperator LogicalOperator { get; set; }

        public Type EntityType { get; set; }

        public string JoinName { get; set; }

        public int? GroupKey { get; set; }

        public DbOperationAttribute(ECompareOperator compareOperator = ECompareOperator.Equal, ELogicalOperator logicalOperator = ELogicalOperator.And, 
            Type entityType = null,string joinName = null, int? groupKey = null)
        {
            this.CompareOperator = compareOperator;
            this.LogicalOperator = logicalOperator;
            this.EntityType = entityType;
            this.JoinName = joinName;
            this.GroupKey = groupKey;
        }
    }
}
