using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using SimpleCMS.CmsContents;
using SimpleCMS.CmsContents.Dtos;
using SimpleCMS.Localization;
using System;
using System.Threading.Tasks;
using Volo.Abp.Users;

namespace SimpleCMS.Blazor.Pages.Articles.View;

public partial class ViewArticle
{
    public CmsContentDto Article { get; set; }

    [Parameter]
    public Guid Id { get; set; }

    [Inject]
    private IMessageService MessageService { get; set; }

    [Inject]
    private ICmsContentAppService cmsContentAppService { get; set; }

    [Inject]
    public ICurrentUser CurrentUser { get; set; }

    [Inject]
    public IStringLocalizer<SimpleCMSResource> L { get; set; }

    public bool IsLoading = true;

    public ViewArticle()
    {

    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Article = await cmsContentAppService.GetAsync(Id);
            IsLoading = false;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            await MessageService.Error($"Inner Exception error occurred: {ex.Message}", "Error");
            Console.WriteLine(ex.Message);
        }
    }

}
