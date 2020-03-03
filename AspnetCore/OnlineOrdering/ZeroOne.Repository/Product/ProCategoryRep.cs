using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Linq.Expressions;
using SqlSugar;
using ZeroOne.Entity;
using ZeroOne.Extension;
using ZeroOne.Extension.Model;

namespace ZeroOne.Repository
{
    public class ProCategoryRep : BaseRep<ProCategory, Guid, ProCategorySearch>, IProCategoryRep
    {
        public ProCategoryRep(ISqlSugarClient client) : base(client)
        {

        }

        public async Task<IList<SelectItem<string, Guid>>> GetSelectItems()
        {
            var dbResult = (await this.GetListAsync()).Select(t => new SelectItem<string, Guid>() { Label = t.CategoryName, Value = t.Id, ParentValue = t.ParentId }).ToList();
            IList<SelectItem<string, Guid>> selectItems = new List<SelectItem<string, Guid>>();
            SelectItem<string, Guid> root = new SelectItem<string, Guid>(string.Empty, Guid.Empty, Guid.Empty);
            dbResult.SelectRecursionCall(root, selectItems);
            return selectItems;
        }
    }
}