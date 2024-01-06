using System.Reflection;
using HomebrewDesigner.Core.RepositoryContracts;
using HomebrewDesigner.Core.ServiceContracts;
using HomebrewDesigner.Core.Services;
using HomebrewDesigner.Infrastructure.DbContext;
using HomebrewDesigner.Infrastructure.Repositories;
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


// Retrieve the connection string from your configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add Entity Framework Core DbContext to the DI container

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.Parse("mysql-8.0"), mysqlOptions =>
        mysqlOptions.MigrationsAssembly("MVC Homebrew Recipe Designer").EnableRetryOnFailure().UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));


builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = true;
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

app.UseAuthorization();

app.MapControllers();


app.Run();