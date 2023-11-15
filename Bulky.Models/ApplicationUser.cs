using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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
    
    public int? CompanyID { get; set; }
    
    //make a foreign key of company
    [ForeignKey("CompanyID")]
    [ValidateNever]
    
    public Company Company { get; set; }
    
    
    
    
}