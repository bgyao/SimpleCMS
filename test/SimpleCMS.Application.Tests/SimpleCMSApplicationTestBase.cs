using Volo.Abp.Modularity;

namespace SimpleCMS;

public abstract class SimpleCMSApplicationTestBase<TStartupModule> : SimpleCMSTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
