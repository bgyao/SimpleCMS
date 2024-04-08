using Blazorise;
using Blazorise.RichTextEdit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using SimpleCMS.CmsContents;
using SimpleCMS.CmsContents.Dtos;
using SimpleCMS.Localization;
using SimpleCMS.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
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
    public IMessageService MessageService { get; set; }
    
    [Inject]
    protected NavigationManager navigationManager { get; set; }

    public CreateUpdateCmsContentDto Article { get; set; }
    public CreateUpdateCmsContentDto ArticleUpdates { get; set; }

    public IReadOnlyList<AuthorLookupDto> authorList = Array.Empty<AuthorLookupDto>();

    public bool IsLoading;
    protected RichTextEdit richTextEditRef;
    protected bool readOnly;
    protected string contentAsHtml;
    protected string contentAsDeltaJson;
    protected string contentAsText;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            IsLoading = true;
            Article = await CmsContentAppService.GetCmsContentAsync(Id);
            authorList = (await CmsContentAppService.GetAuthorLookupAsync()).Items;
        }
        catch (Exception ex)
        {
            await MessageService.Error($"Inner Exception error occurred: {ex.Message}", "Error");
            Console.WriteLine(ex.Message);
            IsLoading = false;
            StateHasChanged();
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
            await MessageService.Error($"Inner Exception error occurred: {ex.Message}","Error");
            Console.WriteLine(ex.Message);
            IsLoading = false;
            StateHasChanged();
        }
    }
    public async Task OnContentChanged()
    {
        contentAsHtml = await richTextEditRef.GetHtmlAsync();
        contentAsDeltaJson = await richTextEditRef.GetDeltaAsync();
        contentAsText = await richTextEditRef.GetTextAsync();
    }

    public Task OnSelectedAuthorChanged(Guid id)
    {
        Article.AuthorId = id;
        StateHasChanged();
        return Task.CompletedTask;
    }
    public async Task OnSave()
    {
        Article.Content = await richTextEditRef.GetHtmlAsync();
    }

    public async Task UpdateArticleAsync()
    {
        try
        {
            await OnSave();
            await CmsContentAppService.InsertOrUpdateCmsContentAsync(Article);
            await MessageService.Success("", "Changes successfully saved.");
            await Task.Delay(3000);
            navigationManager.NavigateTo($"/articles/{Id}");
            StateHasChanged();
        }
        catch (Exception ex)
        {
            await MessageService.Error($"Inner Exception error occurred: {ex.Message}", "Error");
            Console.WriteLine($"An error occurred while saving changes. /n" +
                $"Inner exception: {ex.Message}");
        }
    }

    public async Task ConfirmCancel()
    {
        if (await MessageService.Confirm("Your changes will not be saved", "Are you sure you want to cancel?"))
        {
            navigationManager.NavigateTo($"/articles/{Id}");
        }
    }

    public async Task DeleteArticle()
    {
        try
        {
            if (await MessageService.Confirm("This cannot be undone.", "Are you sure you want to delete this article?",
                option => option.ConfirmButtonColor = "Color.Danger"))
            {
                await CmsContentAppService.DeleteAsync(Id);
                navigationManager.NavigateTo($"/");
            }
        }
        catch (Exception ex)
        {
            await MessageService.Error($"Inner Exception error occurred: {ex.Message}", "Error");
            Console.WriteLine($"An error occurred while deleting the article. /n" +
                $"Inner exception: {ex.Message}");
        }
    }
}
