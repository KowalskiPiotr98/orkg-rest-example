using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IpMan.Models;

[Index(nameof(Name), IsUnique = true)]
public class Building
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; private set; }

    [MaxLength(100)]
    public string Name { get; private set; } = null!;

    [MaxLength(100)]
    public string AddressLine1 { get; private set; } = null!;
    [MaxLength(100)]
    public string? AddressLine2 { get; private set; }
}
