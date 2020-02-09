using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using ZeroOne.Entity;

namespace ZeroOne.Application
{
    public interface IProInfoService
    {
        Task<Pro_Info> GetProductInfo(Guid id);

        Task<IList<Pro_Info>> GetProducts();

        Task<Pro_Info> GetProByName(string name);

        Task<Guid> AddProductInfo(Pro_Info model);

        void ImportData();
    }
}