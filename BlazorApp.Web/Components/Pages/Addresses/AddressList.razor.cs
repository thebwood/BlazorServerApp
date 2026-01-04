using Microsoft.AspNetCore.Components;
using BlazorApp.Web.Services;
using BlazorApp.Web.ViewModels;
using BlazorApp.Web.Components.Dialogs;
using MudBlazor;

namespace BlazorApp.Web.Components.Pages.Addresses;

public partial class AddressList : ComponentBase
{
    [Inject]
    private AddressService AddressService { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    private ILogger<AddressList> Logger { get; set; } = default!;

    protected AddressListViewModel ViewModel { get; set; } = default!;

    protected override void OnInitialized()
    {
        ViewModel = new AddressListViewModel(AddressService);
    }

    protected override async Task OnInitializedAsync()
    {
        await ViewModel.LoadAddressesAsync();
    }

    protected async Task OpenDeleteDialog(Guid id)
    {
        Logger.LogInformation("Opening delete confirmation dialog for address {AddressId}", id);

        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        var dialog = DialogService.Show<DeleteConfirmationDialog>(
            "Confirm Delete",
            options);

        var result = await dialog.Result;

        Logger.LogInformation("Dialog result - Canceled: {Canceled}, Data: {Data}", result.Canceled, result.Data);

        if (!result.Canceled)
        {
            Logger.LogInformation("User confirmed deletion for address {AddressId}", id);
            await DeleteAddress(id);
        }
        else
        {
            Logger.LogInformation("User canceled deletion for address {AddressId}", id);
        }
    }

    protected async Task DeleteAddress(Guid id)
    {
        var success = await ViewModel.DeleteAddressAsync(id);
        
        if (success)
        {
            Snackbar.Add("Address deleted successfully!", Severity.Success);
        }
        else
        {
            Snackbar.Add(ViewModel.ErrorMessage ?? "Failed to delete address", Severity.Error);
        }
        
        await InvokeAsync(StateHasChanged);
    }
}
