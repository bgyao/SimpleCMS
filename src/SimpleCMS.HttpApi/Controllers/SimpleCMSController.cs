using SimpleCMS.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SimpleCMS.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class SimpleCMSController : AbpControllerBase
{
    protected SimpleCMSController()
    {
        LocalizationResource = typeof(SimpleCMSResource);
    }
}
