using Bulky_Web.Models;
using Microsoft.EntityFrameworkCore;


namespace Bulky_Web.Data;

public class ApplicationDbContext:DbContext 
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)//base is used to call the constructor of the parent class
    {
        //dbcontextoptions is used to configure the options for the dbcontext
        
    }
    
    //create a category property
    public DbSet<Category> Categories { get; set; }
    
    //dbset<> is used to create a table in the database and pass the model class as the parameter
    
   

    protected override void OnModelCreating(ModelBuilder modelBuilder)//this method is used to configure the model that was created
    {
        
        modelBuilder.Entity<Category>().HasData(new Category{Id = 1,Name = "Action",DisplayOrder = 1});
        modelBuilder.Entity<Category>().HasData(new Category{Id = 2,Name = "Comedy",DisplayOrder = 2});
        modelBuilder.Entity<Category>().HasData(new Category{Id = 3,Name = "Drama",DisplayOrder = 3});
        modelBuilder.Entity<Category>().HasData(new Category{Id = 4,Name = "Horror",DisplayOrder = 4});
      
       
    
           
        
    }
}