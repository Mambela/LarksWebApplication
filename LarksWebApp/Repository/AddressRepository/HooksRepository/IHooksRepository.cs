using LarksWebApp.Models;

namespace LarksWebApp.Repository.AddressRepository.HooksRepository
{
    public interface IHooksRepository
    {

        Task<bool> CreateHookAsync(HooksModel model);

        HooksModel GetHook(Guid id);

        bool DeleteWebHook(Guid id);

        bool UpdateWebHook(HooksModel model);

        IList<HooksModel> GetHooksList();
        IList<HooksModel> GetHooksList(string userId);

        HooksModel GetHookByDesignation(string designation);

        bool Save();

    }
}
