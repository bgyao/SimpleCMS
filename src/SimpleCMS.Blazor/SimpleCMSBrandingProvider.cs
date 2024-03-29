using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace SimpleCMS.Blazor;

[Dependency(ReplaceServices = true)]
public class SimpleCMSBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "SimpleCMS";
}
