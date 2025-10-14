namespace SSOPortalX.Data.Others.AccountSettings.Dto
{
    public class VendorInformationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = "";
        public string Phone { get; set; } = "";
        public string AddressStreet { get; set; } = "";
        public string AddressCity { get; set; } = "";
        public string AddressCountry { get; set; } = "";
        public string AddressPostalCode { get; set; } = "";

        public VendorInformationDto()
        {
        }

        public VendorInformationDto(int id, int userId, string name, string phone, string addressStreet, 
            string addressCity, string addressCountry, string addressPostalCode)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Phone = phone;
            AddressStreet = addressStreet;
            AddressCity = addressCity;
            AddressCountry = addressCountry;
            AddressPostalCode = addressPostalCode;
        }
    }
}