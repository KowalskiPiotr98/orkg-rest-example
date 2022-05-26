using System.ComponentModel.DataAnnotations;

namespace IpMan.Controllers.DataTransferObjects;

public class BuildingDto
{
    [Required, MaxLength(100)]
    public string? Name { get; set; }
    [Required, MaxLength(100)]
    public string? AddressLine1 { get; set; }
    [MaxLength(100)]
    public string? AddressLine2 { get; set; }
}
