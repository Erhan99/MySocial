using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MySocial.Application.Interfaces.Repositories;
using MySocial.Infrastructure.Data;
using MySocial.Infrastructure.Identity;
using MySocial.Infrastructure.Repositories;
using MySocial.WebUI.Hubs;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

var env = builder.Environment;
var keysFolder = new DirectoryInfo(Path.Combine(env.ContentRootPath, "..", "Keys"));

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(keysFolder)
    .SetApplicationName("MySocial");

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MSDbContext>(options => options.UseSqlServer(connectionString));


builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
}).AddEntityFrameworkStores<MSDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = ".AspNetCore.Identity.Application";
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ILikeInterface, LikeRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

// seed admin
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
    if (userManager.FindByEmailAsync("admin@test.be").Result == null)
    {
        ApplicationUser adminUser = new ApplicationUser();
        adminUser.UserName = "admin";
        adminUser.Email = "admin@test.be";
        adminUser.EmailConfirmed = true; 

        var result = userManager.CreateAsync(adminUser, "Admin123!").Result;
        if (result.Succeeded)
        {
            Claim[] claims = new Claim[] {
                new Claim("IsAdmin", string.Empty)
                };

            userManager.AddClaimsAsync(adminUser, claims).Wait();
        }
    }
}

app.MapRazorPages();
app.MapHub<ChatHub>("/chatHub");

app.Run();
