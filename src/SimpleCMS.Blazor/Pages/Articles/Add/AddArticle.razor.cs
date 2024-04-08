using Blazorise;
using Blazorise.RichTextEdit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using SimpleCMS.CmsContents;
using SimpleCMS.CmsContents.Dtos;
using SimpleCMS.Localization;
using SimpleCMS.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web;

namespace SimpleCMS.Blazor.Pages.Articles.Add;

public partial class AddArticle
{
    public AddArticle()
    {

    }
    [Inject]
    private ICmsContentAppService CmsContentAppService { get; set; }

    [Inject]
    public IStringLocalizer<SimpleCMSResource> L { get; set; }

    [Inject]
    public AbpBlazorMessageLocalizerHelper<SimpleCMSResource> LH { get; set; }

    [Inject]
    public IMessageService MessageService { get; set; }

    [Inject]
    protected NavigationManager navigationManager { get; set; }
    public CreateUpdateCmsContentDto NewArticle { get; set; } = new CreateUpdateCmsContentDto();

    public IReadOnlyList<AuthorLookupDto> authorList = Array.Empty<AuthorLookupDto>();

    #region RichTextEdit
    protected RichTextEdit richTextEditRef;
    protected bool readOnly;
    protected string contentAsHtml;
    protected string contentAsDeltaJson;
    protected string contentAsText;
    #endregion
    public bool IsLoading;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            IsLoading = true;
            await base.OnInitializedAsync();
            NewArticle.AuthorId = Guid.NewGuid();
            authorList = (await CmsContentAppService.GetAuthorLookupAsync()).Items;
        }
        catch (Exception ex)
        {
            await MessageService.Error($"Inner Exception error occurred: {ex.Message}", "Error");
            Console.WriteLine(ex.Message);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        try
        {
            if (firstRender)
            {
                await base.OnAfterRenderAsync(firstRender);
                IsLoading = false;
                StateHasChanged();
            }
        }
        catch (Exception ex)
        {
            await MessageService.Error($"Inner Exception error occurred: {ex.Message}", "Error");
            Console.WriteLine(ex.Message);
        }
    }
    public async Task OnContentChanged()
    {
        contentAsHtml = await richTextEditRef.GetHtmlAsync();
        contentAsDeltaJson = await richTextEditRef.GetDeltaAsync();
        contentAsText = await richTextEditRef.GetTextAsync();
    }

    public async Task OnSave()
    {
        NewArticle.Content = await richTextEditRef.GetHtmlAsync();
    }

    public async Task CreateArticleAsync()
    {
        try
        {
            await OnSave();
            NewArticle.FeaturedImage = "https://placeholder.co/500";
            await CmsContentAppService.InsertOrUpdateCmsContentAsync(NewArticle);
            await MessageService.Success("", "Changes successfully saved.");
            await Task.Delay(3000);
            navigationManager.NavigateTo($"/");
            StateHasChanged();
        }
        catch (Exception ex)
        {
            await MessageService.Error($"Inner Exception error occurred: {ex.Message}", "Error");
            Console.WriteLine(ex.Message);
        }
    }
    public async Task ConfirmCancel()
    {
        if (await MessageService.Confirm("Your changes will not be saved", "Are you sure you want to cancel?"))
        {
            navigationManager.NavigateTo($"/");
        }
    }
}
