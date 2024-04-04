using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleCMS.Localization;
using SimpleCMS.MultiTenancy;
using Volo.Abp.Identity.Blazor;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.TenantManagement.Blazor.Navigation;
using Volo.Abp.UI.Navigation;

namespace SimpleCMS.Blazor.Menus;

public class SimpleCMSMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var administration = context.Menu.GetAdministration();
        var l = context.GetLocalizer<SimpleCMSResource>();

        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                SimpleCMSMenus.Home,
                l["Menu:Home"],
                "/",
                icon: "fas fa-home",
                order: 0
            )
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                "BookStore",
                l["Menu:BookStore"],
                icon: "fa fa-book"
                ).AddItem(
                new ApplicationMenuItem(
                    "BookStore.Books",
                    l["Menu:Books"],
                    url: "/books")
                )
            );

        if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        administration.SetSubItemOrder(SettingManagementMenus.GroupName, 3);

        return Task.CompletedTask;
    }
}
