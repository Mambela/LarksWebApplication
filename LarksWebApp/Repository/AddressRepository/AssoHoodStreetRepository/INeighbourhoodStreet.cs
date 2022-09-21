using LarksWebApp.Models;

namespace LarksWebApp.Repository.AssoHoodStreetRepository
{
    public interface INeighbourhoodStreet
    {
        Task<NeighbourhoodStreets> AssociateStreetToNeighbourhood(NeighbourhoodStreets neighbourhoodStreets);

        Task<bool> AssociateStreetToNeighBourhoods(IList<NeighbourhoodStreets> neighbourhoodStreets);


        NeighbourhoodStreets GetNeighbourhoodStreet(int hoodId, int StreetId);


        IList<NeighbourhoodStreets> GetStreetsInNeighbourhood(int hoodId);

        bool RemoveNeighbourHoodStreetsAssociation(NeighbourhoodStreets neighbourhood);


        bool Save();
    }
}
