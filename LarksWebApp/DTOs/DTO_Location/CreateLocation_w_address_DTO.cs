namespace LarksWebApp.DTOs.DTO_Location
{
    public class CreateLocation_w_address_DTO
    {
        public string Designation { get; set; }

        public string LocationDescription { get; set; }

        public int CountryId { get; set; }


        public string? CountryName { get; set; }

        public int ProvinceId { get; set; }

        public int CityId{ get; set; }

        public int MunicipalityId { get; set; }

        public int CategoryId { get; set; }

        public int? DistrictId { get; set; }

        public string? District { get; set; }

        public int? SuburbId  { get; set; }

        public string? Suburb { get; set; }

        public int? StreetId { get; set; }
        public string?  Street { get; set; }

        public IList<int> SubCategories { get; set; }

        public List<IFormFile>? Files { get; set; }


        /// Objects for the Addres 
        public bool IncludeAddress { get; set; }

        public string? StreetNumber { get; set; }

        public string? Reference { get; set; }

        public string? AdditionalInfo { get; set; }

        public int? BuildingTypeId { get; set; }

        public decimal Lat { get; set; }

        public decimal Lng { get; set; }

   


    }
}
