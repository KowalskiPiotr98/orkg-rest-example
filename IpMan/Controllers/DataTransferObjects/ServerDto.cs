using System.ComponentModel.DataAnnotations;
using System.Net;

namespace IpMan.Controllers.DataTransferObjects;

public class ServerDto
{
    [Required]
    public string? Ip { get; set; }
    [Required]
    public Guid? RackSpaceId { get; set; }
    [Required]
    public Guid? AdministratorId { get; set; }

    public bool TryGetIp(out IPAddress ip) => IPAddress.TryParse(Ip, out ip!);
}
