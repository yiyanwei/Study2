using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroOne.Entity;
using ZeroOne.Extension;
using ZeroOne.Extension.Model;
using ZeroOne.Repository;

namespace ZeroOne.Application
{
    public class ProCategoryService : BaseService<ProCategory, Guid?, ProCategorySearch>, IProCategoryService
    {

        private new IProCategoryRep Rep;
        public ProCategoryService(IProCategoryRep rep, IMapper mapper) : base(rep, mapper)
        {
            this.Rep = rep;
        }

        public async Task<IList<SelectItem<string, Guid?>>> GetSelectItems()
        {
            return await this.Rep.GetSelectItems();
        }

        public async Task<ProCategory> AddEntityAsync(ProCategoryAddRequest request)
        {
            var entity = request.Map<ProCategory>();
            return await this.Rep.AddEntityAsync(entity);
        }

        public async Task<bool> UpdateEntityAsync(ProCategoryEditRequest reqeust)
        {
            var entity = reqeust.Map<ProCategory>();
            return await this.Rep.UpdateEntityNotNullAsync(entity);
        }

        protected override IList<BaseRepModel> GetBaseRepBySearch(ProCategorySearch search)
        {
            throw new NotImplementedException();
        }

        //public async Task<bool> DeleteAsync(Guid? id, string userId)
        //{

        //}
    }
}