using AutoMapper;
using SimpleCMS.Books.Dtos;

namespace SimpleCMS.Blazor;

public class SimpleCMSBlazorAutoMapperProfile : Profile
{
    public SimpleCMSBlazorAutoMapperProfile()
    {
        CreateMap<BookDto, CreateUpdateBookDto>();
    }
}
