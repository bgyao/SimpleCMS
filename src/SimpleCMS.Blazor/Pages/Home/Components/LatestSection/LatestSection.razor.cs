using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SimpleCMS.CmsContents;
using SimpleCMS.CmsContents.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleCMS.Blazor.Pages.Home.Components.LatestSection;

public partial class LatestSection
{
    public GetAllCmsContentDetailsDto ContentDetails { get; set; }

    [Inject]
    public IJSRuntime _js { get; set; }

    [Inject]
    private ICmsContentAppService cmsContentAppService { get; set; }

    public LatestSection()
    {

    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ContentDetails = await cmsContentAppService.GetAllCmsContentDetailsAsync();
            StateHasChanged();
        }
        catch(Exception ex)
        {
            await _js.InvokeVoidAsync("defaultMessage", "error", "Failed to load", ex.Message);
        }
    }
}
