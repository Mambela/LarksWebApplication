namespace LarksWebApp.DTOs.DTO_Location
{
    public class UpdateLocationDTO
    {
        public Guid Id { get; set; }
        public string Designation { get; set; }

        public string LocationDescription { get; set; }

        public Guid CountryId { get; set; }


        public string? CountryName { get; set; }

        public Guid ProvinceId { get; set; }

        public Guid CityId { get; set; }

        public Guid MunicipalityId { get; set; }

        public Guid CategoryId { get; set; }

        public Guid? DistrictId { get; set; }

        public string? District { get; set; }

        public Guid? SuburbId { get; set; }

        public string? Suburb { get; set; }

        public Guid? StreetId { get; set; }
        public string? Street { get; set; }

        public IList<Guid> SubCategories { get; set; }

        public List<IFormFile>? Files { get; set; }


        /// Objects for the Addres 
        public bool IncludeAddress { get; set; }

        public string? StreetNumber { get; set; }

        public string? Reference { get; set; }

        public string? AdditionalInfo { get; set; }

        public Guid? BuildingTypeId { get; set; }

        public decimal Lat { get; set; }

        public decimal Lng { get; set; }

    }
}
