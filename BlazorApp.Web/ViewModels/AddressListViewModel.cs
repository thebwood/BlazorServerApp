using BlazorApp.Web.Models;
using BlazorApp.Web.Services;

namespace BlazorApp.Web.ViewModels;

public class AddressListViewModel : BaseViewModel
{
    private readonly AddressService _addressService;

    public List<AddressDto> Addresses { get; private set; } = new();

    public AddressListViewModel(AddressService addressService)
    {
        _addressService = addressService;
    }

    public async Task LoadAddressesAsync()
    {
        await ExecuteAsync(async () =>
        {
            var result = await _addressService.GetAllAddressesAsync();
            
            if (result.IsSuccess && result.Value != null)
            {
                Addresses = result.Value;
                OnPropertyChanged(nameof(Addresses));
            }
            else
            {
                SetError(result.Error);
                Addresses = new();
            }
        }, "Failed to load addresses");
    }

    public async Task<bool> DeleteAddressAsync(Guid id)
    {
        var result = await _addressService.DeleteAddressAsync(id);
        
        if (result.IsSuccess)
        {
            await LoadAddressesAsync();
            return true;
        }
        
        SetError(result.Error);
        return false;
    }
}
