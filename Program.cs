using Backend_VideoGamesCatalogue.Data;
using Backend_VideoGamesCatalogue.Endpoints;
using Backend_VideoGamesCatalogue.Model;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"),
    sql => sql.EnableRetryOnFailure()));

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy            
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();            
    });
});


var app = builder.Build();

app.UseCors("CorsPolicy");

// Seed Data, first-time only
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDBContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    await StarterData.EnsureSeedDataAsync(db, logger);
}

app.MapVideoGameEndpoints();

app.Run();

public partial class Program { }