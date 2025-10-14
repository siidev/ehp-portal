using System.ComponentModel.DataAnnotations;

namespace SSOPortalX.Data.App.Vendor.Dto
{
    public class UpdateVendorDto
    {
        [Required(ErrorMessage = "Vendor ID is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "User is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Vendor name is required")]
        [StringLength(255, ErrorMessage = "Name cannot exceed 255 characters")]
        public string Name { get; set; } = "";

        [StringLength(50, ErrorMessage = "Phone cannot exceed 50 characters")]
        public string? Phone { get; set; }

        [StringLength(500, ErrorMessage = "Street address cannot exceed 500 characters")]
        public string? AddressStreet { get; set; }

        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters")]
        public string? AddressCity { get; set; }

        [StringLength(100, ErrorMessage = "Country cannot exceed 100 characters")]
        public string? AddressCountry { get; set; }

        [StringLength(20, ErrorMessage = "Postal code cannot exceed 20 characters")]
        public string? AddressPostalCode { get; set; }
    }
}

