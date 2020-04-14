using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ZeroOne.Entity;
using ZeroOne.Repository;

namespace ZeroOne.Application
{
    public class FileInfoService : BaseService<FileInfo, Guid>, IFileInfoService
    {
        public FileInfoService(IFileInfoRep rep,IMapper mapper) : base(rep, mapper)
        {

        }
    }
}
