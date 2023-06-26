using System.ComponentModel.DataAnnotations;

namespace siare.Shared.Entities
{
  /// <summary>
  /// Session Entity
  /// </summary>
  public class Session
  {
    [Key]
    [MaxLength(255)]
    public required string SessionId { get; set; }

    [Required]
    public int UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime LastUpdated { get; set; }

    [Required]
    public DateTime ExpiresAt { get; set; }
  }
}
