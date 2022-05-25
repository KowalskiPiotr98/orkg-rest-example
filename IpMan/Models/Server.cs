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
    public IPAddress Ip { get; private set; }

    public Guid RackSpaceId { get; private set; }
    public RackSpace RackSpace { get; private set; }

    public Guid AdministratorId { get; private set; }
    public Administrator Administrator { get; private set; }
}
