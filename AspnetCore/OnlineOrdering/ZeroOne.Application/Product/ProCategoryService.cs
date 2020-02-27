using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroOne.Extension.Model;
using ZeroOne.Repository;

namespace ZeroOne.Application
{
    public class ProCategoryService : IProCategoryService
    {
        private IProCategoryRep proCategoryRep;
        public ProCategoryService(IProCategoryRep rep)
        {
            this.proCategoryRep = rep;
        }

        public async Task<IList<SelectItem<string, Guid>>> GetSelectItems()
        {
            return await this.proCategoryRep.GetSelectItems();
        }
    }
}