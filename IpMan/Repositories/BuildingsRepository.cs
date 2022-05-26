using IpMan.Data;
using IpMan.Models;
using Microsoft.EntityFrameworkCore;

namespace IpMan.Repositories;

public class BuildingsRepository
{
    private readonly ServerDbContext _context;

    public BuildingsRepository(ServerDbContext context)
    {
        _context = context;
    }

    public IQueryable<Building> GetBuildings() => _context.Buildings;

    public async Task<Building?> GetBuilding(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Buildings.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task CreateBuilding(Building building, CancellationToken cancellationToken = default)
    {
        _context.Buildings.Add(building);
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            if (await _context.Buildings.AsNoTracking().AnyAsync(b => b.Name == building.Name, cancellationToken))
            {
                throw new InvalidOperationException("Building with this name already exists");
            }
            throw;
        }
    }

    public async Task UpdateBuilding(Building building, CancellationToken cancellationToken = default)
    {
        _context.Buildings.Update(building);
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            if (_context.Entry(building).Property(b => b.Name).IsModified && await _context.Buildings.AsNoTracking().AnyAsync(b => b.Name == building.Name, cancellationToken))
            {
                throw new InvalidOperationException("Building with this name already exists");
            }
            throw;
        }
    }

    public async Task RemoveBuilding(Building building, CancellationToken cancellationToken = default)
    {
        _context.Buildings.Remove(building);
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            if (!await _context.Buildings.AsNoTracking().AnyAsync(b => b.Id == building.Id, cancellationToken))
            {
                throw new InvalidOperationException("Building with this ID doesn't exist");
            }
            throw;
        }
    }
}
