using AutoMapper;
using SimpleCMS.Books.Dtos;

namespace SimpleCMS.Books;

public class BookAutoMapperProfile : Profile
{
    public BookAutoMapperProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<CreateUpdateBookDto, Book>();
    }
}
