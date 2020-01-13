using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroOne.Entity;
using ZeroOne.Repository;

namespace ZeroOne.Application
{
    public class ProInfoService : IProInfoService
    {
        private IProInfoRep _ProInfoRep;
        public ProInfoService(IProInfoRep proInfoRep)
        {
            this._ProInfoRep = proInfoRep;
        }

        public async Task<Pro_Info> GetProByName(string name)
        {
            return await this._ProInfoRep.GetProByName(name);
        }

        public async Task<Pro_Info> GetProductInfo(Guid id)
        {
            return await this._ProInfoRep.GetModel(id);
        }

        public async Task<IList<Pro_Info>> GetProducts()
        {
            IList<BaseRepModel> operators = new List<BaseRepModel>();
            ProInfoSearch search = new ProInfoSearch();
            search.ProName = "红富";
            search.IsDeleted = false;
            search.DataStatus = new List<int>(new int[] { 2 });

            operators.Add(new BaseRepModel(nameof(search.ProName), ECompareOperator.Contains, ELogicalOperatorType.And));
            operators.Add(new BaseRepModel(nameof(search.IsDeleted)));
            operators.Add(new BaseRepModel(nameof(search.DataStatus), ECompareOperator.Contains, ELogicalOperatorType.And));
            return await this._ProInfoRep.GetModelList(operators, search);
        }
    }
}
