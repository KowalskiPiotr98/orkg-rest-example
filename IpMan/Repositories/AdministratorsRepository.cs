using IpMan.Data;
using IpMan.Models;
using Microsoft.EntityFrameworkCore;

namespace IpMan.Repositories;

public class AdministratorsRepository
{
    private readonly ServerDbContext _context;

    public AdministratorsRepository(ServerDbContext context)
    {
        _context = context;
    }

    public async Task<Administrator?> GetAdministrator(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Administrators.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task CreateAdministrator(Administrator administrator, CancellationToken cancellationToken = default)
    {
        _context.Administrators.Add(administrator);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAdministrator(Administrator administrator, CancellationToken cancellationToken = default)
    {
        _context.Administrators.Update(administrator);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAdministrator(Administrator administrator, CancellationToken cancellationToken = default)
    {
        _context.Administrators.Remove(administrator);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
