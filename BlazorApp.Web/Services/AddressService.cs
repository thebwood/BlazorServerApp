using System.Net.Http.Json;
using System.Text.Json;
using BlazorApp.Web.Models;

namespace BlazorApp.Web.Services;

public class AddressService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private const string BaseUrl = "https://localhost:7208/api/Addresses";

    public AddressService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<Result<List<AddressDto>>> GetAllAddressesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync(BaseUrl);
            
            if (response.IsSuccessStatusCode)
            {
                var addresses = await response.Content.ReadFromJsonAsync<List<AddressDto>>(_jsonOptions);
                return addresses != null 
                    ? Result.Success(addresses) 
                    : Result.Failure<List<AddressDto>>("Failed to deserialize response");
            }
            
            var errorMessage = await response.Content.ReadAsStringAsync();
            return Result.Failure<List<AddressDto>>(errorMessage);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON Error fetching addresses: {ex.Message}");
            return Result.Failure<List<AddressDto>>($"JSON deserialization error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching addresses: {ex.Message}");
            return Result.Failure<List<AddressDto>>($"Unexpected error: {ex.Message}");
        }
    }

    public async Task<Result<AddressDto>> GetAddressByIdAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var address = await response.Content.ReadFromJsonAsync<AddressDto>(_jsonOptions);
                return address != null 
                    ? Result.Success(address) 
                    : Result.Failure<AddressDto>("Failed to deserialize response");
            }
            
            var errorMessage = await response.Content.ReadAsStringAsync();
            return Result.Failure<AddressDto>(errorMessage);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON Error fetching address {id}: {ex.Message}");
            return Result.Failure<AddressDto>($"JSON deserialization error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching address {id}: {ex.Message}");
            return Result.Failure<AddressDto>($"Unexpected error: {ex.Message}");
        }
    }

    public async Task<Result<AddressDto>> CreateAddressAsync(CreateAddressDto address)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, address, _jsonOptions);
            
            if (response.IsSuccessStatusCode)
            {
                var createdAddress = await response.Content.ReadFromJsonAsync<AddressDto>(_jsonOptions);
                return createdAddress != null 
                    ? Result.Success(createdAddress) 
                    : Result.Failure<AddressDto>("Failed to deserialize response");
            }
            
            // Handle validation errors (400 BadRequest returns a dictionary)
            var errorMessage = await response.Content.ReadAsStringAsync();
            return Result.Failure<AddressDto>(errorMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating address: {ex.Message}");
            return Result.Failure<AddressDto>($"Unexpected error: {ex.Message}");
        }
    }

    public async Task<Result<AddressDto>> UpdateAddressAsync(Guid id, UpdateAddressDto address)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", address, _jsonOptions);
            
            if (response.IsSuccessStatusCode)
            {
                var updatedAddress = await response.Content.ReadFromJsonAsync<AddressDto>(_jsonOptions);
                return updatedAddress != null 
                    ? Result.Success(updatedAddress) 
                    : Result.Failure<AddressDto>("Failed to deserialize response");
            }
            
            var errorMessage = await response.Content.ReadAsStringAsync();
            return Result.Failure<AddressDto>(errorMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating address {id}: {ex.Message}");
            return Result.Failure<AddressDto>($"Unexpected error: {ex.Message}");
        }
    }

    public async Task<Result> DeleteAddressAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                return Result.Success();
            }
            
            var errorMessage = await response.Content.ReadAsStringAsync();
            return Result.Failure(errorMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting address {id}: {ex.Message}");
            return Result.Failure($"Unexpected error: {ex.Message}");
        }
    }
}
