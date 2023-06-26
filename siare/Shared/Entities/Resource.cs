using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace siare.Shared.Entities
{
  /// <summary>
  /// Resource Entity
  /// </summary>
  public class Resource
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ResourceId { get; set; }

    [Required]
    [MaxLength(255)]
    public required string ResourceName { get; set; }
  }
}
