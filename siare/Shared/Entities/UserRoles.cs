using System.ComponentModel.DataAnnotations;

namespace siare.Shared.Entities
{
  /// <summary>
  /// UserRoles Entity
  /// </summary>
  public class UserRoles
  {
    [Key]
    public int UserId { get; set; }

    [Key]
    public int RoleId { get; set; }
  }
}
