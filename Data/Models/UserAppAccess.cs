using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSOPortalX.Data.Models
{
    [Table("user_apps_access")]
    public class UserAppAccess
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

        [Column("granted_by")]
        public int? GrantedBy { get; set; }

        [ForeignKey("GrantedBy")]
        public virtual User? GrantedByUser { get; set; }

        [Column("granted_at")]
        public DateTime GrantedAt { get; set; } = DateTime.UtcNow;

        [Column("notes")]
        public string? Notes { get; set; }
    }
}