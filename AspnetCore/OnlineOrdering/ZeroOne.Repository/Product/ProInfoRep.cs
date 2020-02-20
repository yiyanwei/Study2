using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SqlSugar;
using ZeroOne.Entity;

namespace ZeroOne.Repository
{
    public class ProInfoRep : BaseRep<ProInfoSearch, ProInfo, Guid>, IProInfoRep
    {
        public ISqlSugarClient Client { get; set; }
        private ISqlSugarClient _client;
        public ProInfoRep(ISqlSugarClient client) : base(client)
        {
            this.Client = this._client = client;
        }

        public async Task<ProInfo> GetProByName(string name)
        {
            //测试1
            var queryTest1 = this._client.Queryable<ProInfo>();
            return await queryTest1.Where(x => x.ProName.Contains(name) || x.IsDeleted == true && x.CreationTime > DateTime.Now).FirstAsync();
            // var xx = await queryTest1.FirstAsync();
            // //测试2
            // var queryTest2 = this._client.Queryable<Pro_Info>();
            // queryTest2 = queryTest2.Where(x=>x.ProName.Contains(name));
            // queryTest2 = queryTest2.Where(y=>y.IsDeleted == true);
            // var first = await queryTest2.FirstAsync();
            // //正式返回的数据
            // var query = this._client.Queryable<Pro_Info>();          
            // return await query.Where(t => t.ProName.Contains(name.Trim())).FirstAsync();
        }

        //public new async Task<IList<Pro_Info>> GetModelList(IList<BaseRepModel> items, ProInfoSearch search)
        //{
        //    //Expression.Call(Expression.Property(Expression.Field(Expression.Constant(CS$<> 8__locals1, typeof(ProInfoRep.<> c__DisplayClass3_0)), fieldof(ProInfoRep.<> c__DisplayClass3_0.search)), methodof(ProInfoSearch.get_DataStatus())), methodof(ICollection<int>.Contains(T)), new Expression[]
        //    //   {
        //    //    Expression.Property(Expression.Property(parameterExpression, methodof(Pro_Info.get_DataStatus())), methodof(int?.get_Value()))
        //    //   })
        //    return await this._client.Queryable<Pro_Info>().Where(it => it.ProName.Contains(search.ProName) && it.IsDeleted == search.IsDeleted && search.DataStatus.Contains(it.DataStatus.Value)).ToListAsync();
        //}
    }
}