using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentReviewManager.BLL.Services.interfaces;
using StudentReviewManager.BLL.Services.Realization;
using StudentReviewManager.DAL.Data;
using StudentReviewManager.DAL.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionstring =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string  'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionstring)
);

builder
    .Services.AddDefaultIdentity<User>( /*options => { options.SignIn.RequireConfirmedAccount = false; options.Password =}*/
    )
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ISchoolService, SchoolService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
//builder.Services.AddAuthentication(); 
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
app.UseAuthentication();
app.UseAuthorization();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var context = services.GetRequiredService<ApplicationDbContext>();
    await SeedData.Seed(userManager, roleManager, context);
}
IApplicationBuilder applicationBuilder = app.UseEndpoints(endpoints =>
{
    app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
    app.MapRazorPages();
});
app.Run();
