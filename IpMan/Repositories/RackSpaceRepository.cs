using IpMan.Data;
using IpMan.Models;
using Microsoft.EntityFrameworkCore;

namespace IpMan.Repositories;

public class RackSpaceRepository
{
    private readonly ServerDbContext _context;

    public RackSpaceRepository(ServerDbContext context)
    {
        _context = context;
    }

    public async Task<RackSpace?> GetRackSpace(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.RackSpaces.AsNoTracking().Include(r => r.Building).FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task CreateRackSpace(RackSpace rackSpace, CancellationToken cancellationToken = default)
    {
        _context.RackSpaces.Add(rackSpace);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateRackSpace(RackSpace rackSpace, CancellationToken cancellationToken = default)
    {
        _context.RackSpaces.Update(rackSpace);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveRackSpace(RackSpace rackSpace, CancellationToken cancellationToken = default)
    {
        _context.RackSpaces.Remove(rackSpace);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
