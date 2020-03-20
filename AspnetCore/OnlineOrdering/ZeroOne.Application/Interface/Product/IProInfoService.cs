using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using ZeroOne.Entity;

namespace ZeroOne.Application
{
    public interface IProInfoService : IBaseService<ProInfo, Guid?, ProInfoSearch>
    {

    }
}