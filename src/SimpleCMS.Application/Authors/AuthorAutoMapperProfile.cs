using AutoMapper;
using SimpleCMS.Authors.Dtos;

namespace SimpleCMS.Authors;

public class AuthorAutoMapperProfile : Profile
{
    public AuthorAutoMapperProfile()
    {
        CreateMap<Author, AuthorDto>();
        CreateMap<AuthorDto, UpdateAuthorDto>();
    }
    
}
