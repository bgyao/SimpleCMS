using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleCMS.Data;
using Volo.Abp.DependencyInjection;

namespace SimpleCMS.EntityFrameworkCore;

public class EntityFrameworkCoreSimpleCMSDbSchemaMigrator
    : ISimpleCMSDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreSimpleCMSDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the SimpleCMSDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<SimpleCMSDbContext>()
            .Database
            .MigrateAsync();
    }
}
