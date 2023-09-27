using Microsoft.EntityFrameworkCore;

namespace Bulky_Web.Data;

public class ApplicationDbContext:DbContext 
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)//base is used to call the constructor of the parent class
    {
        //dbcontextoptions is used to configure the options for the dbcontext
        
    }
    
}