using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZeroOne.Entity;
using ZeroOne.Repository;

namespace ZeroOne.Application
{
    public class DistrictService : BaseService<District, Guid>, IDistrictService
    {
        public DistrictService(IDistrictRep rep) : base(rep)
        {

        }
    }
}
