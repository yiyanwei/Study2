using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZeroOne.Entity;

namespace ZeroOne.Application
{
    public interface ISupplierService:IBaseService<Supplier,Guid>
    {
        Task<PageSearchResult<SupplierSearchResult>> SearchPageListResponse(SupplierPageSearch pageSearch);
    }
}
