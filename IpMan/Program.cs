using IpMan.Data;
using IpMan.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var dbConnectionString = Environment.GetEnvironmentVariable("IPMAN_DB") ?? "Host=localhost;Database=IpMan;Username=!;Password=!";
builder.Services.AddDbContext<ServerDbContext>(o => o.UseNpgsql(dbConnectionString, options => options.EnableRetryOnFailure()));

builder.Services.AddScoped<BuildingsRepository>();
builder.Services.AddScoped<RackSpaceRepository>();
builder.Services.AddScoped<AdministratorsRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await using (var scope = app.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ServerDbContext>();
    await context.Database.MigrateAsync();
}

app.Run();
