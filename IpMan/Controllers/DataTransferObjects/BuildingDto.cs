using System.ComponentModel.DataAnnotations;

namespace IpMan.Controllers.DataTransferObjects;

public class BuildingDto
{
    /// <summary>
    /// Name of the building
    /// </summary>
    [Required, MaxLength(100)]
    public string? Name { get; set; }
    /// <summary>
    /// Address of the building, line 1
    /// </summary>
    [Required, MaxLength(100)]
    public string? AddressLine1 { get; set; }
    /// <summary>
    /// Address of the building, line 2
    /// </summary>
    [MaxLength(100)]
    public string? AddressLine2 { get; set; }
}
