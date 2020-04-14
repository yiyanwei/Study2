using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroOne.Entity;
using ZeroOne.Extension;
using ZeroOne.Extension.Model;

namespace ZeroOne.Repository
{
    public class DistrictRep : BaseRep<District, Guid>, IDistrictRep
    {
        public DistrictRep(ISqlSugarClient client) : base(client)
        {

        }

        /// <summary>
        /// 获取省市区递归数据
        /// </summary>
        /// <returns></returns>
        public async Task<IList<SelectItem<string, Guid>>> GetSelectItems()
        {
            var dbResult = (await this.Queryable.Where(t => t.Level != "street" && SqlFunc.IsNull(t.IsDeleted, false) == false).ToListAsync()).Select(t => new SelectItem<string, Guid>()
            { Label = t.Name, Value = t.Id, ParentValue = t.ParentId.HasValue ? t.ParentId.Value : Guid.Empty }).ToList();
            IList<SelectItem<string, Guid>> selectItems = new List<SelectItem<string, Guid>>();
            SelectItem<string, Guid> root = new SelectItem<string, Guid>(string.Empty, Guid.Empty, Guid.Empty);
            dbResult.SelectRecursionCall(root, selectItems);
            return selectItems;
        }
    }
}
