using BlazorApp.Web.Models;
using BlazorApp.Web.Services;

namespace BlazorApp.Web.ViewModels;

public class UpdateAddressViewModel : AddressFormViewModelBase
{
    private UpdateAddressDto _address = new();
    private Guid _addressId;

    public UpdateAddressDto Address
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }

    public Guid AddressId
    {
        get => _addressId;
        private set => SetProperty(ref _addressId, value);
    }

    public UpdateAddressViewModel(AddressService addressService) : base(addressService)
    {
    }

    public async Task LoadAddressAsync(Guid id)
    {
        AddressId = id;

        await ExecuteAsync(async () =>
        {
            var result = await _addressService.GetAddressByIdAsync(id);
            
            if (result.IsSuccess && result.Value != null)
            {
                var addressDto = result.Value;
                Address = new UpdateAddressDto
                {
                    Street = addressDto.Street,
                    City = addressDto.City,
                    State = addressDto.State,
                    ZipCode = addressDto.ZipCode,
                    Country = addressDto.Country
                };
            }
            else
            {
                SetError(result.Error);
            }
        }, "Failed to load address");
    }

    public override async Task<bool> SaveAsync()
    {
        IsSaving = true;
        ClearMessages();
        bool success = false;

        try
        {
            var result = await _addressService.UpdateAddressAsync(AddressId, Address);
            success = result.IsSuccess;
            
            if (success)
            {
                SetSuccess("Address updated successfully!");
            }
            else
            {
                SetError(result.Error);
            }
        }
        catch (Exception ex)
        {
            SetError($"An error occurred while updating the address: {ex.Message}");
        }
        finally
        {
            IsSaving = false;
            OnPropertyChanged(nameof(IsSaving));
        }

        return success;
    }
}
