using Microsoft.AspNetCore.Components;
using SimpleCMS.CmsContents;
using SimpleCMS.CmsContents.Dtos;
using System;
using System.Threading.Tasks;

namespace SimpleCMS.Blazor.Pages.Articles.Edit;

public partial class EditArticle
{
    public EditArticle() { }

    [Parameter]
    public Guid Id { get; set; }
    
    [Inject]
    public ICmsContentAppService CmsContentAppService { get; set; }

    public CmsContentDto Article { get; set; }
    public bool IsLoading = true;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Article = await CmsContentAppService.GetAsync(Id);
            IsLoading = false;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            //await _js.InvokeVoidAsync("defaultMessage", "error", "Failed to load", ex.Message);
            Console.WriteLine(ex.Message);
        }
    }
}
