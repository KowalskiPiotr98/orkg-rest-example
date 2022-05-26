using IpMan.Data;
using IpMan.Models;
using Microsoft.EntityFrameworkCore;

namespace IpMan.Repositories;

public class ServersRepository
{
    private readonly ServerDbContext _context;

    public ServersRepository(ServerDbContext context)
    {
        _context = context;
    }

    public async Task<Server?> GetServer(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Servers.Include(s => s.Administrator).Include(s => s.RackSpace).ThenInclude(r => r.Building).AsNoTracking().FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task CreateServer(Server server, CancellationToken cancellationToken = default)
    {
        _context.Servers.Add(server);
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            if (await _context.Servers.AsNoTracking().AnyAsync(b => b.Ip == server.Ip, cancellationToken))
            {
                throw new InvalidOperationException("Server with this IP already exists");
            }
            throw;
        }
    }

    public async Task UpdateServer(Server server, CancellationToken cancellationToken = default)
    {
        _context.Servers.Update(server);
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            if (_context.Entry(server).Property(b => b.Ip).IsModified && await _context.Servers.AsNoTracking().AnyAsync(b => b.Ip == server.Ip, cancellationToken))
            {
                throw new InvalidOperationException("Server with this IP already exists");
            }
            throw;
        }
    }

    public async Task RemoveServer(Server server, CancellationToken cancellationToken = default)
    {
        _context.Servers.Remove(server);
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            if (!await _context.Servers.AsNoTracking().AnyAsync(b => b.Id == server.Id, cancellationToken))
            {
                throw new InvalidOperationException("Server with this ID doesn't exist");
            }
            throw;
        }
    }
}
