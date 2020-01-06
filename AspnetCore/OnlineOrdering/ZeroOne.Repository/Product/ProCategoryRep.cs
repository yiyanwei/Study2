using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using SqlSugar;

using ZeroOne.Entity;
using ZeroOne.Extension;

namespace ZeroOne.Repository
{
    public class ProCategoryRep : IProCategoryRep
    {
        private SqlSugarClient _client;
        public ProCategoryRep(SqlSugarClient client)
        {
            this._client = client;
        }

        /// <summary>
        /// 获取所有的分类列表
        /// </summary>
        /// <param name="isdeleted">删除状态</param>
        /// <returns></returns>
        public async Task<IList<Pro_Category>> GetCategoryList(bool isdeleted = true)
        {
            string s="";
            s.ToExpression<Func<Pro_Category, bool>>();
            return await this._client.Queryable<Pro_Category>().Where(x=>x.IsDeleted == isdeleted).ToListAsync();
        }

        public async Task<Pro_Category> GetModel(Guid id)
        {
            return await this._client.Queryable<Pro_Category>().Where(t => t.Id == id).SingleAsync();
        }
    }
}