using SqlSugar;
using System;
using ZeroOne.Entity;

namespace ZeroOne.Repository
{
    public class SupplierRep : BaseRep<Supplier, Guid>, ISupplierRep
    {
        public SupplierRep(ISqlSugarClient client) : base(client)
        { 
            
        }
    }
}
