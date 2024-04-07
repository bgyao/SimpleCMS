using AutoMapper;
using SimpleCMS.Authors;
using SimpleCMS.Books.Dtos;
using SimpleCMS.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCMS.Books;

public class BookAutoMapperProfile : Profile
{
    public BookAutoMapperProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<CreateUpdateBookDto, Book>();
        CreateMap<Author, AuthorLookupDto>();
    }
}
