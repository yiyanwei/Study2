using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeroOne.Entity;

namespace ZeroOne.WebApi
{
    public class ViewMappingProfile : Profile
    {
        public ViewMappingProfile()
        {
            //产品分类分页结果对象映射为前端所需的响应对象
            CreateMap<PageSearchResult<ProCategorySearchResult>, PageSearchResult<ProCategoryResponse>>();
            CreateMap<ProCategorySearchResult, ProCategoryResponse>()
            .ForMember(x => x.CreationTime, x => x.MapFrom(y => y.CreationTime.HasValue ? y.CreationTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty));
            //产品映射
            CreateMap<PageSearchResult<ProInfoSearchResult>, PageSearchResult<ProInfoResponse>>();
            CreateMap<ProInfoSearchResult, ProInfoResponse>()
                .ForMember(x => x.CreationTime, x => x.MapFrom(y => y.CreationTime.HasValue ? y.CreationTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty));

            //产品分类添加对象，默认未提供父级分类的id则为根级分类

            CreateMap<ProCategoryAddRequest, ProCategory>()
                .ForMember(x => x.ParentId, x => x.MapFrom(t => t.ParentId.HasValue ? t.ParentId.Value : Guid.Empty));

            //添加产品对象映射
            CreateMap<ProInfoAddRequest, ProInfo>();
        }
    }
}
