
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSOPortalX.Data.Models
{
    [Table("apps")]
    public class Application
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        [StringLength(255)]
        public string Name { get; set; } = "";

        [Required]
        [Column("code")]
        [StringLength(100)]
        public string Code { get; set; } = "";

        [Column("description")]
        public string? Description { get; set; }

        [Column("webhook_url")]
        [StringLength(500)]
        public string? WebhookUrl { get; set; }

        [Column("webhook_secret")]
        [StringLength(255)]
        public string? WebhookSecret { get; set; }

        [Column("icon_url")]
        [StringLength(500)]
        public string? IconUrl { get; set; }

        [Column("category_id")]
        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("is_disable")]
        public bool IsDisable { get; set; } = false;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }
    }
}
