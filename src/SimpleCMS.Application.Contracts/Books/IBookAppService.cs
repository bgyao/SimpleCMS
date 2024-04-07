using SimpleCMS.Books.Dtos;
using SimpleCMS.Shared;
using SimpleCMS.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SimpleCMS.Books;

public interface IBookAppService :
    ICrudAppService<
        BookDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateBookDto>, //must be separate in line with SOLID Principles
    IAuthorLookupAppService
{
}
