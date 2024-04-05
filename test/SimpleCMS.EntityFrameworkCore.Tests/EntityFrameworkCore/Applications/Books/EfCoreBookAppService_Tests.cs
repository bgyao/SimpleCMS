using SimpleCMS.Books;
using Xunit;

namespace SimpleCMS.EntityFrameworkCore.Applications.Books;

[Collection(SimpleCMSTestConsts.CollectionDefinitionName)]
public class EfCoreBookAppService_Tests :
    BookAppService_Tests<SimpleCMSEntityFrameworkCoreTestModule>
{
    
}
