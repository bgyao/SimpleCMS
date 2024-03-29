using SimpleCMS.Samples;
using Xunit;

namespace SimpleCMS.EntityFrameworkCore.Applications;

[Collection(SimpleCMSTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<SimpleCMSEntityFrameworkCoreTestModule>
{

}
