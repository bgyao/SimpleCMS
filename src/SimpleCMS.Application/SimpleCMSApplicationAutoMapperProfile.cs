﻿using AutoMapper;
using SimpleCMS.Authors;
using SimpleCMS.Shared.Dtos;

namespace SimpleCMS;

public class SimpleCMSApplicationAutoMapperProfile : Profile
{
    public SimpleCMSApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Author, AuthorLookupDto>();
    }
}
