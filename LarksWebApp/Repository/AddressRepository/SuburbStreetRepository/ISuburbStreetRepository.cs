using LarksWebApp.Models;

namespace LarksWebApp.Repository.SuburbStreetRepository
{
    public interface ISuburbStreetRepository
    {
        Task<SuburbStreets> AssociateStreetToSuburb(SuburbStreets suburbStreets);

        Task<bool> AssociateStreetToSuburbs(IList<SuburbStreets> suburbStreets);


        SuburbStreets GetSuburbStreet(Guid suburbId, Guid StreetId);


        IList<SuburbStreets> GetStreetsInSuburb(Guid suburbId);

        bool RemoveSuburbStreetAssociation(SuburbStreets suburbStreets);


        bool Save();
    }
}
