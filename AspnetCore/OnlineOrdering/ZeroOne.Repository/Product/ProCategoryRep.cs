using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Linq.Expressions;
using SqlSugar;
using ZeroOne.Entity;

namespace ZeroOne.Repository
{
    public class ProCategoryRep : BaseRep<ProCategorySearch, ProCategory>, IProCategoryRep
    {
        private ISqlSugarClient _client;
        public ProCategoryRep(ISqlSugarClient client) : base(client)
        {
            this._client = client;
			//methodof

			//Expression.Property()
		}

		//public async Task<Pro_Info> GetProByName(string name)
		//{
		//	ProInfoRep.<> c__DisplayClass2_0 CS$<> 8__locals1 = new ProInfoRep.<> c__DisplayClass2_0();
		//	CS$<> 8__locals1.name = name;
		//	ISugarQueryable<Pro_Info> query = this._client.Queryable<Pro_Info>();
		//	ISugarQueryable<Pro_Info> sugarQueryable = query;
		//	ParameterExpression parameterExpression = Expression.Parameter(typeof(Pro_Info), "t");
		//	return await sugarQueryable.Where(Expression.Lambda<Func<Pro_Info, bool>>(Expression.Call(
		//		Expression.Property(parameterExpression, methodof(Pro_Info.get_ProName())), methodof(string.Contains(string)), new Expression[]
		//	{
		//		Expression.Call(Expression.Field(Expression.Constant(CS$<>8__locals1, typeof(ProInfoRep.<>c__DisplayClass2_0)), fieldof(ProInfoRep.<>c__DisplayClass2_0.name)), methodof(string.Trim()), Array.Empty<Expression>())
		//	}), new ParameterExpression[]
		//	{
		//		parameterExpression
		//	})).FirstAsync();
		//}

    }
}