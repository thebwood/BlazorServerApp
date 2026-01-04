namespace BlazorApp.Web.ViewModels;

public class HomeViewModel : BaseViewModel
{
    public string WelcomeMessage { get; } = "Welcome to Address Manager!";
    public string IntroText { get; } = "A modern Blazor application built with clean architecture and MVVM pattern:";

    public List<TechnologyCard> Technologies { get; } = new()
    {
        new TechnologyCard
        {
            Title = "Blazor",
            Icon = "Bolt",
            Description = "Build interactive web UIs using C# instead of JavaScript."
        },
        new TechnologyCard
        {
            Title = "MudBlazor",
            Icon = "Palette",
            Description = "Material Design components for beautiful, responsive interfaces."
        },
        new TechnologyCard
        {
            Title = ".NET 10",
            Icon = "Code",
            Description = "Latest version of .NET for high-performance applications."
        }
    };
}

public class TechnologyCard
{
    public string Title { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class InstructionItem
{
    public string Icon { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}
