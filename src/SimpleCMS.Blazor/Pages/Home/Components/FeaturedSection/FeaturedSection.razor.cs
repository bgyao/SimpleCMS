using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SimpleCMS.CmsContents;
using SimpleCMS.CmsContents.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace SimpleCMS.Blazor.Pages.Home.Components.FeaturedSection;

public partial class FeaturedSection
{
    public GetAllCmsContentDetailsDto ContentDetails { get; set; }
    public List<CmsContentDetailDto> FeaturedArticles { get; set; }
    [Inject]
    private IJSRuntime _js { get; set; }

    [Inject]
    private ICmsContentAppService cmsContentAppService { get; set; }

    public bool IsLoading = true;

    public FeaturedSection()
    {
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ContentDetails = await cmsContentAppService.GetAllCmsContentDetailsAsync(
            new PagedAndSortedResultRequestDto()
            {
                MaxResultCount = 999,
                SkipCount = 0
            });
            FeaturedArticles = ContentDetails.Items.Where(x => x.IsFeatured == true).ToList();
            IsLoading = false;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            await _js.InvokeVoidAsync("defaultMessage", "error", "Failed to load", ex.Message);
        }
    }
}
