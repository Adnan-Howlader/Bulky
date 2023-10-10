using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Bulky_Web.Models;

public class Category
{
    //create ID property
    [Key]//this is the primary key 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//this is used to auto increment the id
    public int Id { get; set; }
    
    //create Name property
    //declare nullable string

    [Required]
    [DisplayName("Category Name")]
    public string Name { get; set; }
    
    [Required]
    //create DisplayOrder property
    [DisplayName("Display Order")]
    public int DisplayOrder { get; set; }
}