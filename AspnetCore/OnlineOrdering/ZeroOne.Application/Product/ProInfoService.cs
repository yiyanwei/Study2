using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using ZeroOne.Entity;
using ZeroOne.Repository;

namespace ZeroOne.Application
{
    public class ProInfoService : BaseService<ProInfo, Guid, ProInfoSearch>, IProInfoService
    {
        private IProInfoRep _ProInfoRep;
        private IProCategoryRep ProCategoryRep;
        public ProInfoService(IProInfoRep proInfoRep, IProCategoryRep proCategoryRep) : base(proInfoRep)
        {
            this._ProInfoRep = proInfoRep;
            this.ProCategoryRep = proCategoryRep;
        }

        public override TResult FormatResult<TResult>(TResult result)
        {
            if (result != null && result is ProInfoResult)
            {
                var convertResult = result as ProInfoResult;
                //获取产品分类的信息
                if (convertResult.CategoryId.HasValue)
                {
                    convertResult.ProCategory = this.ProCategoryRep.GetEntityById(convertResult.CategoryId.Value);
                }
            }
            return result;
        }

        public async Task<ProInfo> GetProByName(string name)
        {
            return await this._ProInfoRep.GetProByName(name);
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
                product = new ProInfoBulk()
                {
                    Id = Guid.NewGuid(),
                    ProName = "产品" + (i + 1),
                    ProCode = (i + 1).ToString(),
                    BulkIdentity = bulkVal
                };
                products.Add(product);
            }
            this._ProInfoRep.BulkAddOrUpdate<ProInfoBulk, ProInfo>(products, beforeAction: this._ProInfoRep.BeforeAction);
        }

        protected override IList<BaseRepModel> GetBaseRepBySearch(ProInfoSearch search)
        {
            throw new NotImplementedException();
        }
    }
}
