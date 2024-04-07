using SimpleCMS.Shared.Dtos;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace SimpleCMS.Shared;

public interface IAuthorLookupAppService
{
    Task<ListResultDto<AuthorLookupDto>> GetAuthorLookupAsync();
}
