using BlazorApp.Web.Models;
using BlazorApp.Web.Services;

namespace BlazorApp.Web.ViewModels;

public abstract class AddressFormViewModelBase : BaseViewModel
{
    protected readonly AddressService _addressService;
    
    public bool IsSaving { get; protected set; }

    protected AddressFormViewModelBase(AddressService addressService)
    {
        _addressService = addressService;
    }

    public abstract Task<bool> SaveAsync();
}
