using Microsoft.AspNetCore.Components;
using BlazorApp.Web.Services;
using BlazorApp.Web.ViewModels;
using MudBlazor;

namespace BlazorApp.Web.Components.Pages.Addresses;

public partial class EditAddress : ComponentBase
{
    [Parameter]
    public Guid Id { get; set; }

    [Inject]
    private AddressService AddressService { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    protected UpdateAddressViewModel ViewModel { get; set; } = default!;

    protected override void OnInitialized()
    {
        ViewModel = new UpdateAddressViewModel(AddressService);
    }

    protected override async Task OnInitializedAsync()
    {
        await ViewModel.LoadAddressAsync(Id);
        StateHasChanged();
    }

    protected async Task HandleSubmit()
    {
        var success = await ViewModel.SaveAsync();

        if (success)
        {
            Snackbar.Add("Address updated successfully!", Severity.Success);
            NavigationManager.NavigateTo("/addresses");
        }
        else
        {
            Snackbar.Add(ViewModel.ErrorMessage ?? "Failed to update address", Severity.Error);
        }
    }

    protected void NavigateToList()
    {
        NavigationManager.NavigateTo("/addresses");
    }
}
