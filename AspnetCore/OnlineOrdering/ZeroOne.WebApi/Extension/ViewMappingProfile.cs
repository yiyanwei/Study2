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
        }
    }
}
