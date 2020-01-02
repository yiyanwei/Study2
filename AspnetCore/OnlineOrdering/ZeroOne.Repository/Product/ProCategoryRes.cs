using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using SqlSugar;

using ZeroOne.Entity;

namespace ZeroOne.Repository
{
    public class ProCategoryRes : IProCategoryRes
    {
        private SqlSugarClient _client;
        public ProCategoryRes(SqlSugarClient client)
        {
            this._client = client;
        }

        public async Task<IList<Pro_Category>> GetCategoryList()
        {
            return null;
        }

        public async Task<Pro_Category> GetModel(Guid id)
        {
            return await this._client.Queryable<Pro_Category>().Where(t => t.Id == id).SingleAsync();
        }
    }
}