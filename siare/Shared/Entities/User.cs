using System.ComponentModel.DataAnnotations;

namespace siare.Shared.Entities
{
  /// <summary>
  /// User Entity
  /// </summary>
  public class User
  {
    [Key]
    public int UserId { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Username { get; set; }

    [Required]
    [MaxLength(255)]
    public required string PasswordHash { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Email { get; set; }
  }
}
