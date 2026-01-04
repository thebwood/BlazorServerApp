using Microsoft.AspNetCore.Components;
using BlazorApp.Web.ViewModels;

namespace BlazorApp.Web.Components.Pages;

public partial class Home : ComponentBase
{
    protected HomeViewModel ViewModel { get; } = new();

    protected override void OnInitialized()
    {
        // Any initialization logic can go here
    }
}
