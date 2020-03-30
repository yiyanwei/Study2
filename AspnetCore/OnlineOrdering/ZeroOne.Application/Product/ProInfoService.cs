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

        public async Task<PageSearchResult<ProInfoResponse>> SearchPageListResponse(ProInfoPageSearch pageSearch)
        {
            var results = await this.SearchPageResultAsync<ProInfoPageSearch, ProInfoSearchResult, PageSearchResult<ProInfoSearchResult>>(pageSearch);
            //获取所有的上传文件uploadid集合
            var proUploadIds = results.Items.Where(t => t.UploadId.HasValue).Select(t => new KeyValuePair<Guid, Guid>(t.Id.Value, t.UploadId.Value)).ToList();
            var search = new ProductUploadFileSearch()
            {
                UploadId = proUploadIds.Select(t => t.Value).ToList()
            };
            //获取所有的图片上传uploadid
            var imgList = await this.FileInfoRep.GetResultListAsync<ProductUploadFileResult, ProductUploadFileSearch>(search);

            var res = Mapper.Map<PageSearchResult<ProInfoResponse>>(results);
            if (imgList?.Count > 0)
            {
                res.Items = res.Items.Select(t =>
                {
                    var items = proUploadIds.Where(y => y.Key == t.Id.Value);
                    if (items.Count() > 0)
                    {
                        var first = items.First();
                        t.ThumbnailImgs = imgList.Where(x => x.UploadId == first.Value).Select(t => t.Url).ToList();
                        t.SourceImgs = imgList.Where(x => x.UploadId == first.Value).Select(t => t.SourceUrl).ToList();
                    }
                    return t;
                }).ToList();
            }
            return res;
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
