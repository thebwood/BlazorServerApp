using System.Net.Http.Json;
using System.Text.Json;
using BlazorApp.Web.Models;

namespace BlazorApp.Web.Services;

public class AddressService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AddressService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;
    private const string BaseUrl = "https://localhost:7208/api/Addresses";

    public AddressService(HttpClient httpClient, ILogger<AddressService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
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
            _logger.LogInformation("Fetching all addresses from {BaseUrl}", BaseUrl);
            var response = await _httpClient.GetAsync(BaseUrl);
            
            if (response.IsSuccessStatusCode)
            {
                var addresses = await response.Content.ReadFromJsonAsync<List<AddressDto>>(_jsonOptions);
                _logger.LogInformation("Successfully fetched {Count} addresses", addresses?.Count ?? 0);
                return addresses != null 
                    ? Result.Success(addresses) 
                    : Result.Failure<List<AddressDto>>("Failed to deserialize response");
            }
            
            var errorMessage = await response.Content.ReadAsStringAsync();
            _logger.LogWarning("Failed to fetch addresses. Status: {StatusCode}, Error: {Error}", 
                response.StatusCode, errorMessage);
            return Result.Failure<List<AddressDto>>(errorMessage);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON deserialization error while fetching addresses");
            return Result.Failure<List<AddressDto>>($"JSON deserialization error: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while fetching addresses");
            return Result.Failure<List<AddressDto>>($"Unexpected error: {ex.Message}");
        }
    }

    public async Task<Result<AddressDto>> GetAddressByIdAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Fetching address with ID: {AddressId}", id);
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var address = await response.Content.ReadFromJsonAsync<AddressDto>(_jsonOptions);
                _logger.LogInformation("Successfully fetched address {AddressId}", id);
                return address != null 
                    ? Result.Success(address) 
                    : Result.Failure<AddressDto>("Failed to deserialize response");
            }
            
            var errorMessage = await response.Content.ReadAsStringAsync();
            _logger.LogWarning("Failed to fetch address {AddressId}. Status: {StatusCode}, Error: {Error}", 
                id, response.StatusCode, errorMessage);
            return Result.Failure<AddressDto>(errorMessage);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON deserialization error while fetching address {AddressId}", id);
            return Result.Failure<AddressDto>($"JSON deserialization error: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while fetching address {AddressId}", id);
            return Result.Failure<AddressDto>($"Unexpected error: {ex.Message}");
        }
    }

    public async Task<Result<AddressDto>> CreateAddressAsync(CreateAddressDto address)
    {
        try
        {
            _logger.LogInformation("Creating new address in {City}, {State}", address.City, address.State);
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, address, _jsonOptions);
            
            if (response.IsSuccessStatusCode)
            {
                var createdAddress = await response.Content.ReadFromJsonAsync<AddressDto>(_jsonOptions);
                _logger.LogInformation("Successfully created address with ID: {AddressId}", createdAddress?.Id);
                return createdAddress != null 
                    ? Result.Success(createdAddress) 
                    : Result.Failure<AddressDto>("Failed to deserialize response");
            }
            
            // Handle validation errors (400 BadRequest returns a dictionary)
            var errorMessage = await response.Content.ReadAsStringAsync();
            _logger.LogWarning("Failed to create address. Status: {StatusCode}, Error: {Error}", 
                response.StatusCode, errorMessage);
            return Result.Failure<AddressDto>(errorMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating address");
            return Result.Failure<AddressDto>($"Unexpected error: {ex.Message}");
        }
    }

    public async Task<Result<AddressDto>> UpdateAddressAsync(Guid id, UpdateAddressDto address)
    {
        try
        {
            _logger.LogInformation("Updating address {AddressId}", id);
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", address, _jsonOptions);
            
            if (response.IsSuccessStatusCode)
            {
                var updatedAddress = await response.Content.ReadFromJsonAsync<AddressDto>(_jsonOptions);
                _logger.LogInformation("Successfully updated address {AddressId}", id);
                return updatedAddress != null 
                    ? Result.Success(updatedAddress) 
                    : Result.Failure<AddressDto>("Failed to deserialize response");
            }
            
            var errorMessage = await response.Content.ReadAsStringAsync();
            _logger.LogWarning("Failed to update address {AddressId}. Status: {StatusCode}, Error: {Error}", 
                id, response.StatusCode, errorMessage);
            return Result.Failure<AddressDto>(errorMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while updating address {AddressId}", id);
            return Result.Failure<AddressDto>($"Unexpected error: {ex.Message}");
        }
    }

    public async Task<Result> DeleteAddressAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Deleting address {AddressId}", id);
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully deleted address {AddressId}", id);
                return Result.Success();
            }
            
            var errorMessage = await response.Content.ReadAsStringAsync();
            _logger.LogWarning("Failed to delete address {AddressId}. Status: {StatusCode}, Error: {Error}", 
                id, response.StatusCode, errorMessage);
            return Result.Failure(errorMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while deleting address {AddressId}", id);
            return Result.Failure($"Unexpected error: {ex.Message}");
        }
    }
}
