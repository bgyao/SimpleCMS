using SimpleCMS.CmsContents.Dtos;
using SimpleCMS.Shared;
using SimpleCMS.Shared.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SimpleCMS.CmsContents;

public interface ICmsContentAppService : ICrudAppService<
    CmsContentDto,
    Guid,
    PagedAndSortedResultRequestDto,
    CreateUpdateCmsContentDto>, //should be separate by SOLID Principles
    IAuthorLookupAppService
{
    Task<GetAllCmsContentDetailsDto> GetAllCmsContentDetailsAsync(PagedAndSortedResultRequestDto input);
    Task InsertOrUpdateCmsContentAsync(CreateUpdateCmsContentDto input);
    Task<ListResultDto<AuthorLookupDto>> GetAuthorLookupAsync();
    Task<CreateUpdateCmsContentDto> GetCmsContentAsync(Guid id);
}
