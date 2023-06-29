using System.ComponentModel.DataAnnotations;

namespace siare.Shared.Entities
{
  /// <summary>
  /// User Entity
  /// </summary>
  public class User
  {
    [Key]
    [Required]
    [Range(1, 99999, ErrorMessage = "UserId must be between 1 and 99999.")]
    public int UserId { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Username { get; set; }

    [Required]
    [MaxLength(255)]
    public required string PasswordHash { get; set; }

    [MaxLength(255)]
    public string? Salt { get; set; }

    [Required]
    [MaxLength(255)]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public required string Email { get; set; }
  }
}
