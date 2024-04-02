using AutoMapper;
using SimpleCMS.CmsContents.Dtos;

namespace SimpleCMS.CmsContents;

public class CmsContentApplicationMapProfile : Profile
{
    public CmsContentApplicationMapProfile()
    {
        CreateMap<CmsContent, CmsContentDto>();
        CreateMap<InsertOrUpdateCmsContentDto, CmsContentDto>();
    }
}
