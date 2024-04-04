using AutoMapper;
using SimpleCMS.Authors.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCMS.Authors;

public class AuthorAutoMapperProfile : Profile
{
    public AuthorAutoMapperProfile()
    {
        CreateMap<Author, AuthorDto>();
    }
    
}
