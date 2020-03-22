using System;
using SqlSugar;
using ZeroOne.Entity;

namespace ZeroOne.Repository
{
    public class FileInfoRep : BaseRep<FileInfo, Guid?>, IFileInfoRep
    {
        public FileInfoRep(ISqlSugarClient client) : base(client)
        {

        }
    }
}
