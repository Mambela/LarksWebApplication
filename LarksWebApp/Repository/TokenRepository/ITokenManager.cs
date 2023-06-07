using LarksWebApp.Models.Users;

namespace LarksWebApp.Repository.TokenRepository
{
    public interface ITokenManager
    {

        Task<bool> CreateTokenManager(AuthToken userToken);

        IList<AuthToken> GetAllTokens(string userId);

        int TokenCount(string UserId);

        AuthToken GetUserToken(int id);

        bool Save();

        bool DeleteToken(int Id);
    }
}
