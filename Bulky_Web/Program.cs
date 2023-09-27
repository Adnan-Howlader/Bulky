using Bulky_Web.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);// Create a builder with the default services configuration.args is the command line arguments passed to the application.

// Add services to the container.
builder.Services.AddControllersWithViews(); // Add MVC services to the services container.
builder.Services.AddDbContext<ApplicationDbContext>(
    option=>option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


//option.UseSqlServer() means that we are using sql server as the database
//option is the parameter of the lambda expression and option is used to configure the options for the dbcontext
//parameter is the type of the dbcontext
//AddDbcontext is a generic method that is used to add the dbcontext to the services container

//we add applicationdbcontext to the services container so that we can use it in the controller class to access the database


var app = builder.Build();// Build the app.

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();