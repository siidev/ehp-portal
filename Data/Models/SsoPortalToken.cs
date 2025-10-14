using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSOPortalX.Data.Models
{
    [Table("sso_portal_tokens")]
    public class SsoPortalToken
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
        [Column("app_id")]
        public int AppId { get; set; }

        [ForeignKey("AppId")]
        public virtual Application App { get; set; } = null!;

        [Required]
        [Column("token")]
        [StringLength(500)]
        public string Token { get; set; } = "";

        [Column("refresh_token")]
        [StringLength(500)]
        public string? RefreshToken { get; set; }

        [Required]
        [Column("expires_at")]
        public DateTime ExpiresAt { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}