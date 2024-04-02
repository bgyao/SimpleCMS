using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SimpleCMS.CmsContents;
using SimpleCMS.CmsContents.Dtos;
using System;
using System.Threading.Tasks;

namespace SimpleCMS.Blazor.Pages.Articles.View;

public partial class ViewArticle
{
    public CmsContentDto Article { get; set; }

    [Parameter]
    public Guid Id { get; set; }

    [Inject]
    private IJSRuntime _js { get; set; }

    [Inject]
    private ICmsContentAppService cmsContentAppService { get; set; }

    public ViewArticle()
    {

    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Article = await cmsContentAppService.GetAsync(Id);
            StateHasChanged();
        }
        catch (Exception ex)
        {
            await _js.InvokeVoidAsync("defaultMessage", "error", "Failed to load", ex.Message);
        }
    }

    //protected override async Task OnAfterRenderAsync(bool firstRender)
    //{
    //    try
    //    {
    //        if (firstRender)
    //        {
    //            Article = await cmsContentAppService.GetCmsContentAsync(Id);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        await _js.InvokeVoidAsync("defaultMessage", "error", "Failed to load", ex.Message);
    //    }
    //}
}
