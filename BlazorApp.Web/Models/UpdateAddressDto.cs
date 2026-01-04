using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlazorApp.Web.Models;

public class UpdateAddressDto
{
    [JsonPropertyName("street")]
    [Required(ErrorMessage = "Street address is required")]
    [StringLength(200, ErrorMessage = "Street address must not exceed 200 characters")]
    public string Street { get; set; } = string.Empty;
    
    [JsonPropertyName("city")]
    [Required(ErrorMessage = "City is required")]
    [StringLength(100, ErrorMessage = "City must not exceed 100 characters")]
    public string City { get; set; } = string.Empty;
    
    [JsonPropertyName("state")]
    [Required(ErrorMessage = "State/Province is required")]
    [StringLength(100, ErrorMessage = "State/Province must not exceed 100 characters")]
    public string State { get; set; } = string.Empty;
    
    [JsonPropertyName("zipCode")]
    [Required(ErrorMessage = "Zip/Postal code is required")]
    [StringLength(20, ErrorMessage = "Zip/Postal code must not exceed 20 characters")]
    public string ZipCode { get; set; } = string.Empty;
    
    [JsonPropertyName("country")]
    [Required(ErrorMessage = "Country is required")]
    [StringLength(100, ErrorMessage = "Country must not exceed 100 characters")]
    public string Country { get; set; } = string.Empty;
}
