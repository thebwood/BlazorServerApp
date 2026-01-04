using BlazorApp.Web.Models;
using BlazorApp.Web.Services;

namespace BlazorApp.Web.ViewModels;

public class CreateAddressViewModel : AddressFormViewModelBase
{
    private CreateAddressDto _address = new();

    public CreateAddressDto Address
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }

    public CreateAddressViewModel(AddressService addressService) : base(addressService)
    {
    }

    public override async Task<bool> SaveAsync()
    {
        IsSaving = true;
        ClearMessages();
        bool success = false;

        try
        {
            var result = await _addressService.CreateAddressAsync(Address);
            success = result.IsSuccess;
            
            if (success)
            {
                SetSuccess("Address created successfully!");
            }
            else
            {
                SetError(result.Error);
            }
        }
        catch (Exception ex)
        {
            SetError($"An error occurred while creating the address: {ex.Message}");
        }
        finally
        {
            IsSaving = false;
            OnPropertyChanged(nameof(IsSaving));
        }

        return success;
    }

    public void Reset()
    {
        Address = new CreateAddressDto();
        ClearMessages();
    }
}
