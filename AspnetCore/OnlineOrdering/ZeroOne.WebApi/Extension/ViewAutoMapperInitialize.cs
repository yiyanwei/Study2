using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ZeroOne.Entity;

namespace ZeroOne.WebApi
{
    public class ViewAutoMapperInitialize
    {
        public static void WebInitAutoMap(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<PageSearchResult<ProCategorySearchResult>, PageSearchResult<ProCategoryResponse>>();
            cfg.CreateMap<ProCategorySearchResult, ProCategoryResponse>()
                .ForMember(x => x.CreationTime, x => x.MapFrom(y => y.CreationTime.HasValue ? y.CreationTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty));
        }
    }
}
