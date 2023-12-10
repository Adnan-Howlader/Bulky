using Microsoft.EntityFrameworkCore;
using Bulky.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace Bulky.DataAccess.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        base(options) //base is used to call the constructor of the parent class
    {
        //dbcontextoptions is used to configure the options for the dbcontext
    }
    //create onconfiguring method


    //create a category property
    public DbSet<Category> Categories { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    public DbSet<Company> Companies { get; set; }

    public DbSet<ShoppingCart> ShoppingCarts { get; set; }

    public DbSet<OrderHeader> OrderHeaders { get; set; }

    public DbSet<OrderDetails> OrderDetails { get; set; }


    //dbset<> is used to create a table in the database and pass the model class as the parameter


    protected override void
        OnModelCreating(ModelBuilder modelBuilder) //this method is used to configure the model that was created
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Category>().HasData(new Category { Id = 1, Name = "Action", DisplayOrder = 1 });
        modelBuilder.Entity<Category>().HasData(new Category { Id = 2, Name = "Fantasy", DisplayOrder = 2 });
        modelBuilder.Entity<Category>().HasData(new Category { Id = 3, Name = "Romance", DisplayOrder = 3 });
    }
}