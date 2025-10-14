
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSOPortalX.Data.Models
{
    [Table("password_reset_tokens")]
    public class PasswordResetToken
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [Required]
        [Column("token")]
        [StringLength(500)]
        public string Token { get; set; } = "";

        [Required]
        [Column("expires_at")]
        public DateTime ExpiresAt { get; set; }

        [Column("is_used")]
        public bool IsUsed { get; set; } = false;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("used_at")]
        public DateTime? UsedAt { get; set; }
    }
}
