using System.ComponentModel.DataAnnotations;

namespace IpMan.Controllers.DataTransferObjects;

public class RackSpaceDto
{
    [Required, MaxLength(10)]
    public string? Room { get; set; }
    [Required, Range(1, int.MaxValue)]
    public int? ServerRack { get; set; }
    [Required, Range(1, 100)]
    public int? RackRow { get; set; }
    [Required]
    public Guid? BuildingId { get; set; }
}
