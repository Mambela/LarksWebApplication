using LarksWebApp.Models;

namespace LarksWebApp.Repository.AssoHoodStreetRepository
{
    public interface INeighbourhoodStreet
    {
        Task<SuburbStreets> AssociateStreetToNeighbourhood(SuburbStreets neighbourhoodStreets);

        Task<bool> AssociateStreetToNeighBourhoods(IList<SuburbStreets> neighbourhoodStreets);


        SuburbStreets GetNeighbourhoodStreet(Guid hoodId, Guid StreetId);


        IList<SuburbStreets> GetStreetsInNeighbourhood(Guid hoodId);

        bool RemoveNeighbourHoodStreetsAssociation(SuburbStreets neighbourhood);


        bool Save();
    }
}
