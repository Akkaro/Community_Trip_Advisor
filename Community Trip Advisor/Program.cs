using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Community_Trip_Advisor.Data;
using Community_Trip_Advisor.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AuthDbContextConnection");builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(connectionString));builder.Services.AddDefaultIdentity<Community_Trip_AdvisorUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AuthDbContext>();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();


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
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=ListPlaces}/{id?}");
app.MapRazorPages();
app.Run();
