using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroOne.Entity;
using ZeroOne.Extension;
using ZeroOne.Extension.Model;
using ZeroOne.Repository;

namespace ZeroOne.Application
{
    public class ProCategoryService : BaseService<ProCategory, Guid>, IProCategoryService
    {
        private IProCategoryRep Rep;
        public ProCategoryService(IProCategoryRep rep) : base(rep)
        {
            this.Rep = rep;
        }

        public async Task<IList<SelectItem<string, Guid>>> GetSelectItems()
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

        //public async Task<bool> DeleteAsync(Guid? id, string userId)
        //{

        //}
    }
}