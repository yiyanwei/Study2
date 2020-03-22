using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using ZeroOne.Entity;
using ZeroOne.Repository;
using AutoMapper;

namespace ZeroOne.Application
{
    public class ProInfoService : BaseService<ProInfo, Guid?, ProInfoSearch>, IProInfoService
    {
        protected IProInfoRep ProInfoRep;
        protected IProCategoryRep ProCategoryRep;
        protected IFileInfoRep FileInfoRep;
        public ProInfoService(IProInfoRep proInfoRep, IProCategoryRep proCategoryRep, IFileInfoRep fileInfoRep, IMapper mapper) : base(proInfoRep, mapper)
        {
            this.ProInfoRep = proInfoRep;
            this.ProCategoryRep = proCategoryRep;
            this.FileInfoRep = fileInfoRep;
        }

        public async Task<ProInfoSingleResult> GetSingleProInfoAsync(Guid? id)
        {
            var entity = await this.ProInfoRep.GetEntityByIdAsync(id);
            if (entity == null)
            {
                throw new Exception("");
            }

            var result = Mapper.Map<ProInfoSingleResult>(entity);
            if (entity.UploadId.HasValue)
            {
                Guid uploadId = entity.UploadId.Value;
                var fileEntityList = await this.FileInfoRep.GetEntityListAsync(nameof(FileInfo.UploadId), uploadId);
                var fileResultList = Mapper.Map<List<FileInfoResult>>(fileEntityList);
                result.FileInfos = fileResultList;
            }
            return result;
        }


        protected override IList<BaseRepModel> GetBaseRepBySearch(ProInfoSearch search)
        {
            throw new NotImplementedException();
        }
    }
}
