using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSOPortalX.Data.Models
{
    [Table("site_settings")]
    public class SiteSettings
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("site_name")]
        [StringLength(255)]
        public string SiteName { get; set; } = "EHP Portal";

        [Column("site_description")]
        [StringLength(500)]
        public string? SiteDescription { get; set; }

        [Column("logo_url")]
        [StringLength(500)]
        public string? LogoUrl { get; set; }

        [Column("logo_small_url")]
        [StringLength(500)]
        public string? LogoSmallUrl { get; set; }

        [Column("favicon_url")]
        [StringLength(500)]
        public string? FaviconUrl { get; set; }

        // Theme color removed from model

        [Column("keywords")]
        [StringLength(1000)]
        public string? Keywords { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}



