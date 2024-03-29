using Volo.Abp.Modularity;

namespace SimpleCMS;

[DependsOn(
    typeof(SimpleCMSDomainModule),
    typeof(SimpleCMSTestBaseModule)
)]
public class SimpleCMSDomainTestModule : AbpModule
{

}
