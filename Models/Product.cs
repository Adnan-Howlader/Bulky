using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Bulky.Models;

public class Product
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    
    
    [Required]
    public string Director { get; set; }
    
    [Required]
    [Range(1,1000)]
    public double Price { get; set; }
    
    //make a foreign key of category
    [Required]
    public int CategoryId { get; set; }
    
    [ForeignKey("CategoryId")]
    [ValidateNever]
    public Category Category { get; set; }
    
    [DefaultValue('s')]
    public string ImageUrl { get; set; }
    
   
    
    
    

    
   
    
    
    
    

    
    
}