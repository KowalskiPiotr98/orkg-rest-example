using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IpMan.Models;

public class RackSpace
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; private set; }
    [MaxLength(10)]
    public string Room { get; private set; }
    [Range(1, int.MaxValue)]
    public int ServerRack { get; private set; }
    [Range(1, 100)]
    public int RackRow { get; private set; }

    public Guid BuildingId { get; private set; }
    public Building Building { get; private set; }
}
