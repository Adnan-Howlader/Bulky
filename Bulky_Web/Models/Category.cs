using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Bulky_Web.Models;

public class Category
{
    //create ID property
    [Key]//this is the primary key 
    public int Id { get; set; }
    
    //create Name property
    //declare nullable string

    [Required]
    public string Name { get; set; }
    
    //create DisplayOrder property
    public int DisplayOrder { get; set; }
}