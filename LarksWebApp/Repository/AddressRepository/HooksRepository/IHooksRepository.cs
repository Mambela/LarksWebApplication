using LarksWebApp.Models;

namespace LarksWebApp.Repository.AddressRepository.HooksRepository
{
    public interface IHooksRepository
    {

        Task<bool> CreateHookAsync(HooksModel model);

        HooksModel GetHooksModel(int id);

        IList<HooksModel> GetHooksList();
        IList<HooksModel> GetHooksList(string userId);

        HooksModel GetHookByDesignation(string designation);

        bool Save();

    }
}
