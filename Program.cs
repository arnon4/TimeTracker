using MegoTimeTracker.Data;
using MegoTimeTracker.Repos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("db");
builder.Services.AddDbContext<StudentDb>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<StudentDb>();
builder.Services.AddScoped<IDbRepository, DbRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;
    if (app.Environment.IsDevelopment()) {
        services.GetRequiredService<StudentDb>().Database.EnsureDeleted();
    }
    services.GetRequiredService<StudentDb>().Database.EnsureCreated();
    await SeedData.InitializeAsync(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
//app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
