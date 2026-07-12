namespace LarksWebApp.DTOs.DTO_Addresses
{
    public class AddressGeolocationDTO
    {
        public Guid? OptionalId { get; set; }
        public Decimal Lat { get; set; }
        public Decimal Lng { get; set; }

        public string HookName { get; set; }

        public Guid responseId { get; set; }

        public string? UserId { get; set; }

    }
}
