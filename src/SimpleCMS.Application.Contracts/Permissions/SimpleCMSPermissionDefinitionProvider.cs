using SimpleCMS.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SimpleCMS.Permissions;

public class SimpleCMSPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {

        #region BookStore Permissions
        var bookStoreGroup = context.AddGroup(
            SimpleCMSPermissions.BookStoreGroup, 
            L("Permission:BookStore"));

        var booksPermission = bookStoreGroup.AddPermission(SimpleCMSPermissions.Books.Default, L("Permission:Books"));
        booksPermission.AddChild(SimpleCMSPermissions.Books.Create, L("Permission:Books.Create"));
        booksPermission.AddChild(SimpleCMSPermissions.Books.Edit, L("Permission:Books.Edit"));
        booksPermission.AddChild(SimpleCMSPermissions.Books.Delete, L("Permission:Books.Delete"));

        var authorsGroup = context.AddGroup(
            SimpleCMSPermissions.AuthorsGroup,
            L("Permission:Authors"));
        var authorsPermission = authorsGroup.AddPermission(
            SimpleCMSPermissions.Authors.Default, L("Permission:Authors"));
        authorsPermission.AddChild(
            SimpleCMSPermissions.Authors.Create, L("Permission:Authors.Create"));
        authorsPermission.AddChild(
            SimpleCMSPermissions.Authors.Edit, L("Permission:Authors.Edit"));
        authorsPermission.AddChild(
            SimpleCMSPermissions.Authors.Delete, L("Permission:Authors.Delete"));
        #endregion

        #region CMS Permissions
        var cmsPermissionsGroup = context.AddGroup(
            SimpleCMSPermissions.GroupName,
            L("Permission:CMSContents"));

        var cmsPermission = cmsPermissionsGroup.AddPermission(
            SimpleCMSPermissions.CmsContentsAdminPolicies.Default, L("Permission:CMSContentManagement"));
        cmsPermission.AddChild(SimpleCMSPermissions.CmsContentsAdminPolicies.Create, L("Permission:CMSContents.Create"));
        cmsPermission.AddChild(SimpleCMSPermissions.CmsContentsAdminPolicies.Edit, L("Permission:CMSContents.Edit"));
        cmsPermission.AddChild(SimpleCMSPermissions.CmsContentsAdminPolicies.Delete, L("Permission:CMSContents.Delete"));
        #endregion
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<SimpleCMSResource>(name);
    }
}
