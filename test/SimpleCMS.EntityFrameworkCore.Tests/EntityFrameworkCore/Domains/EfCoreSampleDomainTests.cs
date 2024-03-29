using SimpleCMS.Samples;
using Xunit;

namespace SimpleCMS.EntityFrameworkCore.Domains;

[Collection(SimpleCMSTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<SimpleCMSEntityFrameworkCoreTestModule>
{

}
