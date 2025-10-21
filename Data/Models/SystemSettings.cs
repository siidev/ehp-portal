using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSOPortalX.Data.Models
{
    [Table("system_settings")]
    public class SystemSettings
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("setting_key")]
        [StringLength(100)]
        public string SettingKey { get; set; } = "";

        [Column("setting_value")]
        [StringLength(1000)]
        public string? SettingValue { get; set; }

        [Column("setting_type")]
        [StringLength(50)]
        public string SettingType { get; set; } = "text"; // text, image, color, boolean

        [Column("description")]
        [StringLength(500)]
        public string? Description { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

