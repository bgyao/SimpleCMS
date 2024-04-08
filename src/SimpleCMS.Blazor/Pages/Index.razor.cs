using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace SimpleCMS.Blazor.Pages;

public partial class Index
{
    public Index()
    {

    }

    [Inject]
    protected NavigationManager navigationManager { get; set; }

    public void AddArticle()
    {
        navigationManager.NavigateTo("/add");
    }
}
