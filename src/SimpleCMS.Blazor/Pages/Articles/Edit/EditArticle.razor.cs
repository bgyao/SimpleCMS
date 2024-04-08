using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using SimpleCMS.CmsContents;
using SimpleCMS.CmsContents.Dtos;
using SimpleCMS.Localization;
using SimpleCMS.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web;

namespace SimpleCMS.Blazor.Pages.Articles.Edit;

public partial class EditArticle
{
    public EditArticle() { }

    [Parameter]
    public Guid Id { get; set; }
    
    [Inject]
    public ICmsContentAppService CmsContentAppService { get; set; }

    [Inject]
    public IStringLocalizer<SimpleCMSResource> L { get; set; }

    [Inject]
    public AbpBlazorMessageLocalizerHelper<SimpleCMSResource> LH { get; set; }

    [Inject]
    public IJSRuntime _js { get; set; }

    public CmsContentDto EditingEntity { get; set; }
    public CreateUpdateCmsContentDto ArticleUpdates { get; set; }
    public IReadOnlyList<AuthorLookupDto> authorList = Array.Empty<AuthorLookupDto>();
    public bool IsLoading = true;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            EditingEntity = await CmsContentAppService.GetAsync(Id);
            authorList = (await CmsContentAppService.GetAuthorLookupAsync()).Items;
            IsLoading = false;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            //await _js.InvokeVoidAsync("defaultMessage", "error", "", ex.Message);
            Console.WriteLine(ex.Message);
        }
    }

    public async Task UpdateArticleAsync()
    {
        try
        {
            ArticleUpdates = CmsContentAppService.MapInputForUpdating(EditingEntity);
            await CmsContentAppService.UpdateAsync(ArticleUpdates.Id, ArticleUpdates);
        }
        catch(Exception ex)
        {
            //await _js.InvokeVoidAsync("defaultMessage", "error", "", ex.Message);
            Console.WriteLine($"An error occurred while saving changes. /n" +
                $"Inner exception: {ex.Message}");
        }
    }
}
