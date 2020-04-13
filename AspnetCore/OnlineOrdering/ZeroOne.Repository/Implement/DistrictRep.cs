using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using ZeroOne.Entity;

namespace ZeroOne.Repository
{
    public class DistrictRep : BaseRep<District, Guid>, IDistrictRep
    {
        public DistrictRep(ISqlSugarClient client) : base(client)
        {

        }
    }
}
