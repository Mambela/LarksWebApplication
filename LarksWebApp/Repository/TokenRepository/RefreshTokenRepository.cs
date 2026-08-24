using LarksWebApp.DAL;
using LarksWebApp.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace LarksWebApp.Repository.TokenRepository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly LarksContext dbContext;

        public RefreshTokenRepository(LarksContext larksContext)
        {
            dbContext = larksContext;
        }

        public async Task<RefreshToken> CreateAsync(RefreshToken token)
        {
            await dbContext.RefreshTokens.AddAsync(token);

            if (Save())
                return token;

            return null;
        }

        public RefreshToken GetByHash(string tokenHash)
        {
            return dbContext.RefreshTokens.FirstOrDefault(t => t.TokenHash == tokenHash);
        }

        public bool Revoke(RefreshToken token)
        {
            token.IsRevoked = true;
            dbContext.RefreshTokens.Update(token);

            return Save();
        }

        public bool Save()
        {
            return dbContext.SaveChanges() >= 0;
        }
    }
}
