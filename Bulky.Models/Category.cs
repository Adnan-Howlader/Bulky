using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bulky.Models;


    public class Category
    {
        //create ID property
        [Key] //this is the primary key 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //this is used to auto increment the id
        public int Id { get; set; }

        //create Name property
        //declare nullable string

        [Required]
        [DisplayName("Category Name")]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        //create DisplayOrder property
        [DisplayName("Display Order")]
        //change the error message

        [Range(1, 100, ErrorMessage = "Display Order for category must be from 1 to 100")]

        public int DisplayOrder { get; set; }
        //set the display order to 1 by default


    }
