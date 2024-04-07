using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SimpleCMS.CmsContents;
using SimpleCMS.CmsContents.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace SimpleCMS.Blazor.Pages.Home.Components.LatestSection;

public partial class LatestSection
{
    public GetAllCmsContentDetailsDto ContentDetails { get; set; }
    public List<CmsContentDetailDto> Articles { get; set; }

    [Inject]
    private IJSRuntime _js { get; set; }

    [Inject]
    private ICmsContentAppService cmsContentAppService { get; set; }
    public bool IsLoading = true;
    public LatestSection()
    {

    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ContentDetails = await cmsContentAppService.GetAllCmsContentDetailsAsync( 
                new PagedAndSortedResultRequestDto() );
            Articles = ContentDetails.Items.Where(x => x.IsFeatured == false).ToList();
            IsLoading = false;
            StateHasChanged();
        }
        catch(Exception ex)
        {
            await _js.InvokeVoidAsync("defaultMessage", "error", "Failed to load", ex.Message);
        }
    }
}
