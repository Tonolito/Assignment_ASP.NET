using Business.Interfaces;
using Business.Services;
using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("AlphaDB")));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IStatusService, StatusService>();


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
    .AddEntityFrameworkStores<DataContext>()
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


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

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
    var user = new MemberEntity { UserName = "admin@domain.com", Email = "admin@domain.com" };

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
