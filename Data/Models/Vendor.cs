using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSOPortalX.Data.Models
{
    [Table("vendors")]
    public class Vendor
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
        [Column("name")]
        [StringLength(255)]
        public string Name { get; set; } = "";

        [Column("phone")]
        [StringLength(50)]
        public string? Phone { get; set; }

        [Column("address_street")]
        [StringLength(500)]
        public string? AddressStreet { get; set; }

        [Column("address_city")]
        [StringLength(100)]
        public string? AddressCity { get; set; }

        [Column("address_country")]
        [StringLength(100)]
        public string? AddressCountry { get; set; }

        [Column("address_postal_code")]
        [StringLength(20)]
        public string? AddressPostalCode { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }
    }
}

