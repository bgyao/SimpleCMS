using SimpleCMS.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace SimpleCMS.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(SimpleCMSEntityFrameworkCoreModule),
    typeof(SimpleCMSApplicationContractsModule)
    )]
public class SimpleCMSDbMigratorModule : AbpModule
{
}
