using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IpMan.Models;

public class RackSpace
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; private set; }
    [MaxLength(10)]
    public string Room { get; private set; } = null!;
    [Range(1, int.MaxValue)]
    public int ServerRack { get; private set; }
    [Range(1, 100)]
    public int RackRow { get; private set; }

    public Guid BuildingId { get; private set; }
    public Building Building { get; private set; } = null!;

    public RackSpace()
    {
    }

    public RackSpace(Guid buildingId, string room, int rack, int row)
    {
        SetBuilding(buildingId);
        SetLocationInBuilding(room, rack, row);
    }

    public void SetLocationInBuilding(string room, int rack, int row)
    {
        Room = room;
        ServerRack = rack;
        RackRow = row;
    }

    public void SetBuilding(Building building)
    {
        SetBuilding(building.Id);
        Building = building;
    }

    public void SetBuilding(Guid buildingId) => BuildingId = buildingId;
}
