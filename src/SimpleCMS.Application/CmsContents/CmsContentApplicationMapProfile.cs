using AutoMapper;
using SimpleCMS.CmsContents.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCMS.CmsContents;

public class CmsContentApplicationMapProfile : Profile
{
    public CmsContentApplicationMapProfile()
    {
        CreateMap<CmsContent, CmsContentDto>();
        CreateMap<InsertOrUpdateCmsContentDto, CmsContentDto>();
    }
}
