using LarksWebApp.Models;

namespace LarksWebApp.Repository.AssoHoodStreetRepository
{
    public interface INeighbourhoodStreet
    {
        Task<SuburbStreets> AssociateStreetToNeighbourhood(SuburbStreets neighbourhoodStreets);

        Task<bool> AssociateStreetToNeighBourhoods(IList<SuburbStreets> neighbourhoodStreets);


        SuburbStreets GetNeighbourhoodStreet(int hoodId, int StreetId);


        IList<SuburbStreets> GetStreetsInNeighbourhood(int hoodId);

        bool RemoveNeighbourHoodStreetsAssociation(SuburbStreets neighbourhood);


        bool Save();
    }
}
