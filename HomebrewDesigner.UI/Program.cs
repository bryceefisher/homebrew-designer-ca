using System.Reflection;
using HomebrewDesigner.Core.Domain.Identity;
using HomebrewDesigner.Core.RepositoryContracts;
using HomebrewDesigner.Core.ServiceContracts;
using HomebrewDesigner.Core.Services;
using HomebrewDesigner.Infrastructure.DbContext;
using HomebrewDesigner.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

// using RepoContracts;
// using Repositories;

var builder = WebApplication.CreateBuilder(args);


//******************Hot-reload workaround for dev only*****************************************

string appDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation(options => options.FileProviders.Add(
        new PhysicalFileProvider(appDirectory)));

//Add services to DI container
builder.Services.AddScoped<IHopService, HopService>();
builder.Services.AddScoped<IFermentableService, FermentableService>();
builder.Services.AddScoped<IYeastService, YeastService>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<IHopRepository, HopRepository>();
builder.Services.AddScoped<IFermentableRepository, FermentableRepository>();
builder.Services.AddScoped<IYeastRepository, YeastRepository>();
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddTransient<SeedData>();


// Retrieve the connection string from your configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add Entity Framework Core DbContext to the DI container

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.Parse("mysql-8.0"), mysqlOptions =>
        mysqlOptions.MigrationsAssembly("HomebrewDesigner.UI").EnableRetryOnFailure().UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));


builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = true;
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        // Configure password complexity and security settings
        options.Password.RequiredLength = 8; // Minimum password length
        options.Password.RequireNonAlphanumeric = true; // Requires non-alphanumeric character
        options.Password.RequireUppercase = true; // Requires at least one uppercase letter
        options.Password.RequireLowercase = true; // Requires at least one lowercase letter
        options.Password.RequireDigit = true; // Requires at least one digit
        options.Password.RequiredUniqueChars = 4; // Requires a number of unique characters in the password

        // Require each user to have a unique email
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>() // Specify the database context to store user and role data
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    // Set the fallback authorization policy
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser() // Require user to be authenticated for all requests unless explicitly stated otherwise
        .Build(); // Build the policy
});

// Configure cookie settings for handling user authentication
builder.Services.ConfigureApplicationCookie(options =>
{
    // Set the path for the login page
    // This is used when an unauthenticated user tries to access a restricted resource
    options.LoginPath = "/account/login";
});


//Uncomment before pushing to GH
// builder.Services.AddControllersWithViews();

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

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var seedData = services.GetRequiredService<SeedData>();
    seedData.SeedRoles().GetAwaiter().GetResult();
    seedData.SeedUsers().GetAwaiter().GetResult();
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}

app.Run();