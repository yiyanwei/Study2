using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZeroOne.Entity;
using ZeroOne.Extension.Model;
using ZeroOne.Repository;
using ZeroOne.Extension;
using System.Linq;
using AutoMapper;

namespace ZeroOne.Application
{
    public class DistrictService : BaseService<District, Guid>, IDistrictService
    {

        public DistrictService(IDistrictRep rep, IMapper mapper) : base(rep, mapper)
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


        public async Task<int> SyncDistrictAsync(DistrictSettings settings)
        {
            int affectdRow = 0;
            StringBuilder sbUrl = new StringBuilder();
            sbUrl.Append(settings.BaseUrl);
            sbUrl.Append(settings.DistrictUrl);
            sbUrl.AppendFormat("&subdistrict={0}", settings.SubDistrict);
            if (!string.IsNullOrEmpty(settings.Output))
            {
                sbUrl.AppendFormat("&output={0}", settings.Output);
            }
            DistrictResponse districtResponse = await this.GetResponse<DistrictResponse>(sbUrl.ToString(), CallBack: (responseStr) =>
            {
                return System.Text.RegularExpressions.Regex.Replace(responseStr, "\"citycode\":.?\\[\\]", "\"citycode\": null");
            });
            if (districtResponse?.districts?.Count > 0 && districtResponse.districts[0].districts?.Count > 0)
            {
                //目标对象
                List<District> districts = new List<District>();
                this.AddList(districts, districtResponse.districts[0].districts, null);
                if (districts?.Count > 0)
                {
                    DateTime now = DateTime.Now;
                    districts = districts.Select(t =>
                    {
                        t.CreationTime = now;
                        return t;
                    }).ToList();
                    affectdRow = await this.BulkAddAsync(districts);
                }
            }
            return affectdRow;
        }

        private void AddList(List<District> targets, List<Districts> sources, Guid? parentId)
        {
            if (sources?.Count > 0)
            {
                foreach (var item in sources)
                {
                    District target = Mapper.Map<District>(item);
                    target.Id = Guid.NewGuid();
                    target.ParentId = parentId;
                    targets.Add(target);
                    this.AddList(targets, item.districts, target.Id);
                }
            }
        }
    }
}
