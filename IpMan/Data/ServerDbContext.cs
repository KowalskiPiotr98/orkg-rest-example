#nullable disable
using IpMan.Models;
using Microsoft.EntityFrameworkCore;

namespace IpMan.Data;

public class ServerDbContext : DbContext
{
    public ServerDbContext(DbContextOptions<ServerDbContext> options) : base(options)
    {
    }

    public DbSet<Server> Servers { get; set; }
    public DbSet<Administrator> Administrators { get; set; }
    public DbSet<RackSpace> RackSpaces { get; set; }
    public DbSet<Building> Buildings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Server>().Property(s => s.Ip).HasConversion<string>();
    }
}
