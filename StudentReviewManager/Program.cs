using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyLeaveManagement.Mappings;
using StudentReviewManager.DAL.Data;
using StudentReviewManager.DAL.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionstring =
    builder.Configuration.GetConnectionstring("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string  'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionstring)
);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddAutoMapper(typeof(Maps));
builder
    .Services.AddDefaultIdentity<User>( /*options => { options.SignIn.RequireConfirmedAccount = false; options.Password =}*/
    )
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
;
builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await SeedData.Seedasync(userManager, roleManager);
}
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
