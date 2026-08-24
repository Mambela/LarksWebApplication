using LarksWebApp.Models.Users;

namespace LarksWebApp.Repository.TokenRepository
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> CreateAsync(RefreshToken token);

        RefreshToken GetByHash(string tokenHash);

        bool Revoke(RefreshToken token);

        bool Save();
    }
}
