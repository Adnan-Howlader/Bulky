using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Bulky.Models;

public class OrderDetails
{
    public int Id { get; set; }
    [Required] 
    public int OrderHeaderId { get; set; }

    [ForeignKey("OrderHeaderId")]
    [ValidateNever]
    public OrderHeader OrderHeader { get; set; }

    [Required] 
    public int ProductID { get; set; }

    [ForeignKey("ProductID")]
    [ValidateNever]
    public Product Product { get; set; }

    public int Count { get; set; }
    public double Price { get; set;}//Price at the time of purchase so that it doesnt change 
    
    
    
    
}