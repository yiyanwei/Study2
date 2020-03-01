using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using ZeroOne.Entity;

namespace ZeroOne.Application
{
    public interface IProInfoService
    {
        Task<ProInfo> GetProductInfo(Guid id);

        Task<IList<ProInfo>> GetProducts();

        Task<ProInfo> GetProByName(string name);

        Task<ProInfo> AddProductInfo(ProInfo model);

        void ImportData();
    }
}