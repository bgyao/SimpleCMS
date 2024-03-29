using SimpleCMS.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SimpleCMS.Permissions;

public class SimpleCMSPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(SimpleCMSPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(SimpleCMSPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<SimpleCMSResource>(name);
    }
}
