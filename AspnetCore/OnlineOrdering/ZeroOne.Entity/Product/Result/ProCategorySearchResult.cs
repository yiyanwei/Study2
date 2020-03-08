using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroOne.Entity
{
    public class ProCategorySearchResult : Result
    {
        public string CategoryName { get; set; }

        [MainTableRelation(typeof(ProCategory),EJoinType.LeftJoin,nameof(ProCategory.CategoryName),false)]
        [JoinTableRelation(nameof(ProCategory.Id),typeof(ProCategory),nameof(ProCategory.ParentId))]
        public string ParentCategoryName { get; set; }
    }
}
