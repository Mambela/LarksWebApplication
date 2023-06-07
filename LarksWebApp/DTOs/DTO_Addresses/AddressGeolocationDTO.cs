namespace LarksWebApp.DTOs.DTO_Addresses
{
    public class AddressGeolocationDTO
    {
        public int? OptionalId { get; set; }
        public Decimal Lat { get; set; }
        public Decimal Lng { get; set; }

        public string HookName { get; set; }

        public int responseId { get; set; }

        public string? UserId { get; set; }

    }
}
