using Business.Interfaces;
using Business.Services;
using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using WebApp.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();


builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("AlphaDB")));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IStatusService, StatusService>();
builder.Services.AddScoped<INotificationService, NotificationService>();


builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();





builder.Services.AddIdentity<MemberEntity, IdentityRole>(opions =>
{
    opions.SignIn.RequireConfirmedAccount = false;
    opions.User.RequireUniqueEmail = true;
    opions.Password.RequiredLength = 8;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/auth/signin";
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
    options.AccessDeniedPath = "/denied";
});

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("Administrators", policy => policy.RequireRole("Administrator"));
//    options.AddPolicy("Authenticated", policy => policy.RequireRole("Administrator", "User"));
//});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

})
    .AddCookie()
    .AddGitHub(options =>
    {
        options.ClientId = builder.Configuration["Authentication:GitHub:ClientId"]!;
        options.ClientSecret = builder.Configuration["Authentication:GitHub:ClientSecret"]!;
        options.Scope.Add("user:email");
        options.Scope.Add("read:user");

        options.Events.OnCreatingTicket = async context =>
        {
            await Task.Delay(0);

            if (context.User.TryGetProperty("name", out var name))
            {
                var fullName = name.GetString();
                if (!string.IsNullOrEmpty(fullName))
                {
                    var names = fullName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                    if (names.Length > 0)
                    {
                        context.Identity?.AddClaim(new Claim(ClaimTypes.GivenName, names[0]));
                    }

                    if (names.Length > 1)
                    {
                        context.Identity?.AddClaim(new Claim(ClaimTypes.Surname, names[1]));
                    }
                }
            }
        };

    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.MapHub<ChatHub>("/chathub");
app.MapHub<NotificationHub>("/notificationHub");

app.UseRewriter(new Microsoft.AspNetCore.Rewrite.RewriteOptions().AddRedirect("^$", "projects"));
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();
using (var scope = app.Services.CreateScope())
{
    var roleManger = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roleNames = { "Administrator", "User" };

    foreach (var roleName in roleNames)
    {
        var roleExists = await roleManger.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            await roleManger.CreateAsync(new IdentityRole(roleName));
        }
    }

    var userManger = scope.ServiceProvider.GetRequiredService<UserManager<MemberEntity>>();
    var user = new MemberEntity { UserName = "admin@domain.com", Email = "admin@domain.com", FirstName = "Admin"};

    var userExists = await userManger.Users.AnyAsync(x => x.Email == user.Email);
    if (!userExists)
    {
        var result = await userManger.CreateAsync(user, "BytMig123!");
        if (result.Succeeded)
        {
            await userManger.AddToRoleAsync(user, "Administrator");
        }
    }
}


app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Projects}/{action=Projects}")
    .WithStaticAssets();

app.Run();
