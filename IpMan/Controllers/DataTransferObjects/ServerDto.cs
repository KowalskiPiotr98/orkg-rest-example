using System.ComponentModel.DataAnnotations;
using System.Net;

namespace IpMan.Controllers.DataTransferObjects;

public class ServerDto
{
    /// <summary>
    /// IP address of a server
    /// </summary>
    [Required]
    public string? Ip { get; set; }
    /// <summary>
    /// ID of a rack space where the server is located
    /// </summary>
    [Required]
    public Guid? RackSpaceId { get; set; }
    /// <summary>
    /// ID of an administrator managing the server
    /// </summary>
    [Required]
    public Guid? AdministratorId { get; set; }

    public bool TryGetIp(out IPAddress ip) => IPAddress.TryParse(Ip, out ip!);
}
