using System.ComponentModel.DataAnnotations;

namespace siare.Shared.Entities
{
  /// <summary>
  /// Role Entity
  /// </summary>
  public class Role
  {
    [Key]
    public int RoleId { get; set; }

    [Required]
    [MaxLength(255)]
    public required string RoleName { get; set; }
  }
}
