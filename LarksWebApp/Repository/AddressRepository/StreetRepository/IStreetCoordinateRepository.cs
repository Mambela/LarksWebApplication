using LarksWebApp.Models.CoordinatesModel;

namespace LarksWebApp.Repository.AddressRepository.StreetRepository
{
    public interface IStreetCoordinateRepository
    {
        Task  <bool> StoreStreetCoordinates(IList<StreetCoordinates> streetCoordinates);

        IList<StreetCoordinates> GetStreetCoordinates(int streetId);



        bool DeleteStreetCoordinates(StreetCoordinates streetObj);

        bool Save();




    }
}
