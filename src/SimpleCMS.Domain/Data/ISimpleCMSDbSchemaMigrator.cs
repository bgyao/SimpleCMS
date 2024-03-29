using System.Threading.Tasks;

namespace SimpleCMS.Data;

public interface ISimpleCMSDbSchemaMigrator
{
    Task MigrateAsync();
}
