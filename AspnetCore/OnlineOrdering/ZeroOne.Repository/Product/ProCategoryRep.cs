using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using SqlSugar;

using ZeroOne.Entity;

namespace ZeroOne.Repository
{
    public class ProCategoryRep : BaseRep<ProCategorySearch, Pro_Category>, IProCategoryRep
    {
        private ISqlSugarClient _client;
        public ProCategoryRep(ISqlSugarClient client) : base(client)
        {
            this._client = client;
        }

    }
}