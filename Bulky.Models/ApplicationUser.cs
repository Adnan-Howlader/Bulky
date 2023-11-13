using System.ComponentModel.DataAnnotations;

namespace Bulky.Models;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser:IdentityUser
{
    [Required]
    public string Name { get; set; } //? means
    
    
    public string? StreetAddress { get; set; }
    
    public string? City { get; set; }
    
    public string? State { get; set; }
    
    public string? PostalCode { get; set; }
    
    
    
    
}