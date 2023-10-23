using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bulky.Models;

public class Product
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }
    
    [Required]
    public string Director { get; set; }
    
    [Required]
    [Range(1,1000)]
    public string ListPrice { get; set; }
    
    [Required]
    [Range(1,1000)]
    [DisplayName("Price 1-50")]//this is the name that will be displayed on the page
    public double Price { get; set; }
    
    [Required]
    [Range(1,1000)]
    [DisplayName("Price for 50+")]
    public double Price50 { get; set; }
    
    [Required]
    [Range(1,1000)]
    [DisplayName("Price for 100+")]
    public double Price100 { get; set; }
    
    
    
    
    
   
    
    
    
}