using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace SimpleCMS.Data;

/* This is used if database provider does't define
 * ISimpleCMSDbSchemaMigrator implementation.
 */
public class NullSimpleCMSDbSchemaMigrator : ISimpleCMSDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
