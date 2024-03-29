using Volo.Abp.Settings;

namespace SimpleCMS.Settings;

public class SimpleCMSSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(SimpleCMSSettings.MySetting1));
    }
}
