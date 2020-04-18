using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroOne.Entity;
using ZeroOne.Repository;

namespace ZeroOne.Application
{
    public class SupplierService : BaseService<Supplier, Guid>, ISupplierService
    {
        protected new ISupplierRep Rep { get; set; }

        protected IFileInfoRep FileRep { get; set; }
        public SupplierService(ISupplierRep rep, IFileInfoRep fileRep, IMapper mapper) : base(rep, mapper)
        {
            this.Rep = rep;
            this.FileRep = fileRep;
        }

        public async Task<PageSearchResult<SupplierSearchResult>> SearchPageListResponse(SupplierPageSearch pageSearch)
        {
            var results = await this.SearchPageResultAsync<SupplierPageSearch, SupplierSearchResult>(pageSearch);
            //获取所有的上传文件uploadid集合
            var proUploadIds = results.Items.Where(t => t.BusinessLicense.HasValue).Select(t => new KeyValuePair<Guid, Guid>(t.Id, t.BusinessLicense.Value)).ToList();
            if (proUploadIds?.Count > 0)
            {
                var search = new UploadFileSearch()
                {
                    UploadId = proUploadIds.Select(t => t.Value).ToList()
                };
                //获取所有的图片上传uploadid
                var imgList = await this.FileRep.GetResultListAsync<UploadFileResult, UploadFileSearch>(search); ;
                if (imgList?.Count > 0)
                {
                    results.Items = results.Items.Select(t =>
                    {
                        var items = proUploadIds.Where(y => y.Key == t.Id);
                        if (items.Count() > 0)
                        {
                            var first = items.First();
                            t.ThumbnailImgs = imgList.Where(x => x.UploadId == first.Value).Select(t => t.Url).ToList();
                            t.SourceImgs = imgList.Where(x => x.UploadId == first.Value).Select(t => t.SourceUrl).ToList();
                        }
                        return t;
                    }).ToList();
                }
            }
            return results;
        }

        public async Task<SupplierDetailResult> GetResultById(Guid id)
        {
            var supplierResult = await this.Rep.GetResultAsync<SupplierDetailResult>(id);
            if (supplierResult != null && supplierResult.BusinessLicense.HasValue)
            {
                Guid uploadId = supplierResult.BusinessLicense.Value;
                var fileEntityList = await this.FileRep.GetEntityListAsync(nameof(FileInfo.UploadId), uploadId);
                var fileResultList = Mapper.Map<List<FileInfoResult>>(fileEntityList);
                supplierResult.FileInfos = fileResultList;
            }
            return supplierResult;
        }
    }
}
