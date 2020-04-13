using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZeroOne.Entity;
using ZeroOne.Extension.Model;
using ZeroOne.Repository;

namespace ZeroOne.Application
{
    public class DistrictService : BaseService<District, Guid>, IDistrictService
    {

        public DistrictService(IDistrictRep rep) : base(rep)
        {

        }

        public async Task<int> BulkAddAsync(List<District> models)
        {
            int affectRow = 0;
            if (models?.Count > 0)
            {
                object result = await this.Rep.BulkAddAsync(models);
                if (result is int)
                {
                    affectRow = Convert.ToInt32(result);
                }
            }
            return affectRow;
        }

        public async Task<IList<SelectItem<string, Guid>>> GetSelectItems()
        {
            IDistrictRep tempRep = this.Rep as IDistrictRep;
            return await tempRep.GetSelectItems();
        }
    }
}
