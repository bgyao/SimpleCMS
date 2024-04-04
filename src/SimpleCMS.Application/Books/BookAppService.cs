using SimpleCMS.Books.Dtos;
using SimpleCMS.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace SimpleCMS.Books;

public class BookAppService : CrudAppService<
    Book,
    BookDto,
    Guid,
    PagedAndSortedResultRequestDto,
    CreateUpdateBookDto>,
    IBookAppService
{

    public BookAppService(IRepository<Book, Guid> repository)
        :base(repository)
    {
        #region Authorization Policies
        GetPolicyName = SimpleCMSPermissions.Books.Default;
        GetListPolicyName = SimpleCMSPermissions.Books.Default;
        CreatePolicyName = SimpleCMSPermissions.Books.Create;
        UpdatePolicyName = SimpleCMSPermissions.Books.Edit;
        DeletePolicyName = SimpleCMSPermissions.Books.Delete;
        #endregion
    }
}
