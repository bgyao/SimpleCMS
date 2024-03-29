using System;
using System.Collections.Generic;
using System.Text;
using SimpleCMS.Localization;
using Volo.Abp.Application.Services;

namespace SimpleCMS;

/* Inherit your application services from this class.
 */
public abstract class SimpleCMSAppService : ApplicationService
{
    protected SimpleCMSAppService()
    {
        LocalizationResource = typeof(SimpleCMSResource);
    }
}
