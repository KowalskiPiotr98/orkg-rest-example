using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IpMan.Models;

public class Administrator
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; private set; }
    [MaxLength(100)]
    public string FirstName { get; private set; } = null!;
    [MaxLength(100)]
    public string LastName { get; private set; } = null!;
    [MaxLength(100)]
    [DataType(DataType.EmailAddress)]
    public string EmailAddress { get; private set; } = null!;

    public Administrator()
    {
    }

    public Administrator(string firstName, string lastName, string email)
    {
        SetPersonalData(firstName, lastName, email);
    }

    public void SetPersonalData(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = email;
    }
}
