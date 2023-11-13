using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.iRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);// Create a builder with the default services configuration.args is the command line arguments passed to the application.

// Add services to the container.
builder.Services.AddControllersWithViews(); // Add MVC services to the services container.
builder.Services.AddDbContext<ApplicationDbContext>(
    option=>option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
//AddIdentity is a generic method that is used to add the identity to the services container

builder.Services.AddScoped<iCategoryRepository, CategoryRepository>();//AddScoped means that the object will be created once per request within the scope
//here we are addding dependency injection of implementation of icategoryrepository by categoryrepository

builder.Services.AddScoped<iProductRepository, ProductRepository>();
//option.UseSqlServer() means that we are using sql server as the database
//option is the parameter of the lambda expression and option is used to configure the options for the dbcontext
//parameter is the type of the dbcontext
//AddDbcontext is a generic method that is used to add the dbcontext to the services container

//we add applicationdbcontext to the services container so that we can use it in the controller class to access the database

//add service for razor page 

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddRazorPages();



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

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages(); 

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();