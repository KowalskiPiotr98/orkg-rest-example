using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace IpMan.Models;

[Index(nameof(Ip), IsUnique = true)]
public class Server
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; private set; }
    public IPAddress Ip { get; private set; } = null!;

    public Guid RackSpaceId { get; private set; }
    public RackSpace RackSpace { get; private set; } = null!;

    public Guid AdministratorId { get; private set; }
    public Administrator Administrator { get; private set; } = null!;

    public Server()
    {
    }

    public Server(IPAddress ip, RackSpace rackSpace, Administrator administrator)
    {
        AssignIp(ip);
        AssignRackSpace(rackSpace);
        AssignAdministrator(administrator);
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
