using SimpleCMS.Localization;
using Volo.Abp.AspNetCore.Components;

namespace SimpleCMS.Blazor;

public abstract class SimpleCMSComponentBase : AbpComponentBase
{
    protected SimpleCMSComponentBase()
    {
        LocalizationResource = typeof(SimpleCMSResource);
    }
}
