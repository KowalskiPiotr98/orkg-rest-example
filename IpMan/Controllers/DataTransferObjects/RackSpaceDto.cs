using System.ComponentModel.DataAnnotations;

namespace IpMan.Controllers.DataTransferObjects;

public class RackSpaceDto
{
    /// <summary>
    /// Room name of where the rack is located
    /// </summary>
    [Required, MaxLength(10)]
    public string? Room { get; set; }
    /// <summary>
    /// Number of the rack in the room
    /// </summary>
    [Required, Range(1, int.MaxValue)]
    public int? ServerRack { get; set; }
    /// <summary>
    /// Row in the selected rack
    /// </summary>
    [Required, Range(1, 100)]
    public int? RackRow { get; set; }
    /// <summary>
    /// ID of the building where the rack is located
    /// </summary>
    [Required]
    public Guid? BuildingId { get; set; }
}
