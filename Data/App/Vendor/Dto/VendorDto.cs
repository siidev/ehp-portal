using SSOPortalX.Data.Models;
using VendorModel = SSOPortalX.Data.Models.Vendor;

namespace SSOPortalX.Data.App.Vendor.Dto
{
    public class VendorDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = "";
        public string UserEmail { get; set; } = "";
        public string Name { get; set; } = "";
        public string? Phone { get; set; }
        public string? AddressStreet { get; set; }
        public string? AddressCity { get; set; }
        public string? AddressCountry { get; set; }
        public string? AddressPostalCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string FullAddress
        {
            get
            {
                var addressParts = new List<string>();
                
                if (!string.IsNullOrEmpty(AddressStreet))
                    addressParts.Add(AddressStreet);
                if (!string.IsNullOrEmpty(AddressCity))
                    addressParts.Add(AddressCity);
                if (!string.IsNullOrEmpty(AddressPostalCode))
                    addressParts.Add(AddressPostalCode);
                if (!string.IsNullOrEmpty(AddressCountry))
                    addressParts.Add(AddressCountry);

                return string.Join(", ", addressParts);
            }
        }

        public VendorDto() { }

        public VendorDto(VendorModel vendor)
        {
            Id = vendor.Id;
            UserId = vendor.UserId;
            UserName = vendor.User?.Username ?? "";
            UserEmail = vendor.User?.Email ?? "";
            Name = vendor.Name;
            Phone = vendor.Phone;
            AddressStreet = vendor.AddressStreet;
            AddressCity = vendor.AddressCity;
            AddressCountry = vendor.AddressCountry;
            AddressPostalCode = vendor.AddressPostalCode;
            CreatedAt = vendor.CreatedAt;
            UpdatedAt = vendor.UpdatedAt;
        }
    }
}
