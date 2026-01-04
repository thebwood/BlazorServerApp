using Microsoft.AspNetCore.Components;
using BlazorApp.Web.Services;
using BlazorApp.Web.ViewModels;
using MudBlazor;

namespace BlazorApp.Web.Components.Pages.Addresses;

public partial class CreateAddress : ComponentBase
{
    [Inject]
    private AddressService AddressService { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    protected CreateAddressViewModel ViewModel { get; set; } = default!;

    protected override void OnInitialized()
    {
        ViewModel = new CreateAddressViewModel(AddressService);
    }

    protected async Task HandleSubmit()
    {
        var success = await ViewModel.SaveAsync();

        if (success)
        {
            Snackbar.Add("Address created successfully!", Severity.Success);
            NavigationManager.NavigateTo("/addresses");
        }
        else
        {
            Snackbar.Add(ViewModel.ErrorMessage ?? "Failed to create address", Severity.Error);
        }
    }

    protected void NavigateToList()
    {
        NavigationManager.NavigateTo("/addresses");
    }
}
