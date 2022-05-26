using System.ComponentModel.DataAnnotations;

namespace IpMan.Controllers.DataTransferObjects;

public class AdministratorDto
{
    [Required, MaxLength(100)]
    public string? FirstName { get; set; }
    [Required, MaxLength(100)]
    public string? LastName { get; set; }
    [Required, MaxLength(100)]
    [DataType(DataType.EmailAddress)]
    public string? EmailAddress { get; set; }
}
