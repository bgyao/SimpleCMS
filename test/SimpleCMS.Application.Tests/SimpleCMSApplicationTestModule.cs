using Volo.Abp.Modularity;

namespace SimpleCMS;

[DependsOn(
    typeof(SimpleCMSApplicationModule),
    typeof(SimpleCMSDomainTestModule)
)]
public class SimpleCMSApplicationTestModule : AbpModule
{

}
