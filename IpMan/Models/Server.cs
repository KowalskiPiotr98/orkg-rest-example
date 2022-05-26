using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace IpMan.Models;

[Index(nameof(Ip), IsUnique = true)]
public class Server
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; private set; }
    [JsonConverter(typeof(IpAddressJsonConverter))]
    public IPAddress Ip { get; private set; } = null!;

    public Guid RackSpaceId { get; private set; }
    public RackSpace RackSpace { get; private set; } = null!;

    public Guid AdministratorId { get; private set; }
    public Administrator Administrator { get; private set; } = null!;

    public Server()
    {
    }

    public Server(IPAddress ip, Guid rackSpaceId, Guid administratorId)
    {
        AssignIp(ip);
        AssignRackSpace(rackSpaceId);
        AssignAdministrator(administratorId);
    }

    public void AssignIp(IPAddress ipAddress) => Ip = ipAddress;

    public void AssignRackSpace(Guid rackSpaceId) => RackSpaceId = rackSpaceId;
    public void AssignRackSpace(RackSpace rackSpace)
    {
        RackSpace = rackSpace;
        AssignRackSpace(rackSpace.Id);
    }

    public void AssignAdministrator(Guid adminId) => AdministratorId = adminId;
    public void AssignAdministrator(Administrator admin)
    {
        Administrator = admin;
        AssignAdministrator(admin.Id);
    }
}
