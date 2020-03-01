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

        public async Task<ProInfo> GetProByName(string name)
        {
            return await this._ProInfoRep.GetProByName(name);
        }

        public async Task<ProInfo> GetProductInfo(Guid id)
        {
            return await this._ProInfoRep.GetEntityAsync(id);
        }

        public async Task<ProInfo> AddProductInfo(ProInfo model)
        {
            if (model == null)
            {
                throw new Exception("数据为null");
            }
            model.Id = Guid.NewGuid();
            return await this._ProInfoRep.AddEntityAsync(model);
        }

        public async Task<IList<ProInfo>> GetProducts()
        {
            IList<BaseRepModel> operators = new List<BaseRepModel>();
            ProInfoSearch search = new ProInfoSearch();
            search.ProName = "红富";
            search.IsDeleted = false;
            search.DataStatus = new List<int>(new int[] { 2 });

            operators.Add(new BaseRepModel(nameof(search.ProName), ECompareOperator.Contains, ELogicalOperatorType.And));
            operators.Add(new BaseRepModel(nameof(search.IsDeleted)));
            operators.Add(new BaseRepModel(nameof(search.DataStatus), ECompareOperator.Contains, ELogicalOperatorType.And));
            return await this._ProInfoRep.GetListAsync(operators, search);
        }

        /// <summary>
        /// 测试导入数据
        /// </summary>
        public void ImportData()
        {
            IList<ProInfoBulk> products = new List<ProInfoBulk>();
            ProInfoBulk product;
            string bulkVal = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            for (int i = 0; i < 666; i++)
            {
                product = new ProInfoBulk() {
                    Id = Guid.NewGuid(),
                    ProName = "产品" + (i + 1),
                    ProCode = (i+1).ToString(),
                    BulkIdentity = bulkVal
                };
                products.Add(product);
            }
            this._ProInfoRep.BulkAddOrUpdate<ProInfoBulk,ProInfo>(products,beforeAction: this._ProInfoRep.BeforeAction);
        }
    }
}
