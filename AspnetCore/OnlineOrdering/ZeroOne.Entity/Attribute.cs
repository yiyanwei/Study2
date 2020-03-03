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


    public class JoinTableAttribute : Attribute
    {
        public Type EntityType { get; set; }

        public string JoinField { get; set; }

        public EJoinType JoinType { get; set; }

        public JoinTableAttribute(Type entityType, string joinField, EJoinType joinType = EJoinType.InnerJoin)
        {
            this.EntityType = entityType;
            this.JoinField = joinField;
            this.JoinType = JoinType;
        }
    }
}
