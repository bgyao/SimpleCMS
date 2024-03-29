using Xunit;

namespace SimpleCMS.EntityFrameworkCore;

[CollectionDefinition(SimpleCMSTestConsts.CollectionDefinitionName)]
public class SimpleCMSEntityFrameworkCoreCollection : ICollectionFixture<SimpleCMSEntityFrameworkCoreFixture>
{

}
