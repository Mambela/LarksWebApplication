using LarksWebApp.Models;

namespace LarksWebApp.Repository.HoodRepository
{
    public interface INeighbourhoodRepository
    {
        bool VerifyNeighbourhoodExist(string designation, int municipalityId);

        bool VerifyNeighbourhoodExist(int Id);

        Task<NeighbourhoodModel> CreateNeighbourhood(NeighbourhoodModel neighbourhood);

        NeighbourhoodModel GetNeighbourhood(int neighbourhoodId);

        IList<NeighbourhoodModel> GetNeighbourhoodModels(int provinceId);

        public bool DeleteNeighbourhood(NeighbourhoodModel neighbourhood);


        public bool UpdateNeighbourhoody(NeighbourhoodModel neighbourhood);

        bool Save();
    }
}
