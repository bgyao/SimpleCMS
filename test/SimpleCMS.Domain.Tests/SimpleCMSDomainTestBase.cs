using Volo.Abp.Modularity;

namespace SimpleCMS;

/* Inherit from this class for your domain layer tests. */
public abstract class SimpleCMSDomainTestBase<TStartupModule> : SimpleCMSTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
