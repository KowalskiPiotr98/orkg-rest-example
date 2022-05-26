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

    public Building()
    {
    }

    public Building(string name, string addressLine1, string? addressLine2 = null)
    {
        Rename(name);
        ChangeAddress(addressLine1, addressLine2);
    }

    public void Rename(string name)
    {
        Name = name;
    }

    public void ChangeAddress(string addressLine1, string? addressLine2 = null)
    {
        AddressLine1 = addressLine1;
        AddressLine2 = addressLine2;
    }
}
