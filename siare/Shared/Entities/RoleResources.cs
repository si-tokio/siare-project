using System.ComponentModel.DataAnnotations;

namespace siare.Shared.Entities
{
  /// <summary>
  /// RoleResources Entity
  /// </summary>
  public class RoleResources
  {
    [Key]
    public int RoleId { get; set; }

    [Key]
    public int ResourceId { get; set; }
  }
}
