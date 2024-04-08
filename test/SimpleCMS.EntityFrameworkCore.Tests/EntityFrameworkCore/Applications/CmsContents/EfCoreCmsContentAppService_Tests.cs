using SimpleCMS.CmsContents;
using Xunit;

namespace SimpleCMS.EntityFrameworkCore.Applications.CmsContents;

[Collection(SimpleCMSTestConsts.CollectionDefinitionName)]
public class EfCoreCmsContentAppService_Tests : 
    CmsContentAppService_Tests<SimpleCMSEntityFrameworkCoreTestModule>
{
}
