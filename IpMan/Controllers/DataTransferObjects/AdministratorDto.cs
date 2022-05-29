using System.ComponentModel.DataAnnotations;

namespace IpMan.Controllers.DataTransferObjects;

public class AdministratorDto
{
    /// <summary>
    /// First name od the administrator
    /// </summary>
    [Required, MaxLength(100)]
    public string? FirstName { get; set; }
    /// <summary>
    /// Last name od the administrator
    /// </summary>
    [Required, MaxLength(100)]
    public string? LastName { get; set; }
    /// <summary>
    /// Email address of the administrator
    /// </summary>
    [Required, MaxLength(100)]
    [DataType(DataType.EmailAddress)]
    public string? EmailAddress { get; set; }
}
