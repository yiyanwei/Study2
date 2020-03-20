using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using ZeroOne.Entity;
using ZeroOne.Repository;
using AutoMapper;

namespace ZeroOne.Application
{
    public class ProInfoService : BaseService<ProInfo, Guid?, ProInfoSearch>, IProInfoService
    {
        private IProInfoRep _ProInfoRep;
        private IProCategoryRep ProCategoryRep;
        public ProInfoService(IProInfoRep proInfoRep, IProCategoryRep proCategoryRep,IMapper mapper) : base(proInfoRep, mapper)
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

        
        protected override IList<BaseRepModel> GetBaseRepBySearch(ProInfoSearch search)
        {
            throw new NotImplementedException();
        }
    }
}
