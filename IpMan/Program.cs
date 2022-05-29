using System.Reflection;
using IpMan.Data;
using IpMan.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var dbConnectionString = Environment.GetEnvironmentVariable("IPMAN_DB") ?? "Host=localhost;Database=IpMan;Username=!;Password=!";
builder.Services.AddDbContext<ServerDbContext>(o => o.UseNpgsql(dbConnectionString, options => options.EnableRetryOnFailure()));

builder.Services.AddScoped<BuildingsRepository>();
builder.Services.AddScoped<RackSpaceRepository>();
builder.Services.AddScoped<AdministratorsRepository>();
builder.Services.AddScoped<ServersRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#if DEBUG
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Server management API",
        Description = "API to manage servers"
    });
    var xmlName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = AppContext.BaseDirectory;
    c.IncludeXmlComments(Path.Combine(xmlPath, xmlName));
});
#endif

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
