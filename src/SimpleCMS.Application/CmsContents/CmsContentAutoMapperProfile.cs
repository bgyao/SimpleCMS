using AutoMapper;
using SimpleCMS.CmsContents.Dtos;

namespace SimpleCMS.CmsContents;

public class CmsContentAutoMapperProfile : Profile
{
    public CmsContentAutoMapperProfile()
    {
        CreateMap<CmsContent, CmsContentDto>();
        CreateMap<CreateUpdateCmsContentDto, CmsContentDto>().ReverseMap();
        CreateMap<CreateUpdateCmsContentDto, CmsContent>();
    }
}
