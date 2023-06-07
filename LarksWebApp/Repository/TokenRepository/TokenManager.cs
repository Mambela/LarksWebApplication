using LarksWebApp.DAL;
using LarksWebApp.Models.Users;

namespace LarksWebApp.Repository.TokenRepository
{
    public class TokenManager : ITokenManager
    {

        private readonly LarksContext dbContext;

        public TokenManager(LarksContext _larksContext)
        {
            dbContext = _larksContext;
        }


        public async Task<bool> CreateTokenManager(AuthToken userToken)
        {
            await dbContext.Tokens.AddAsync(userToken);

            return Save();
        }

        public bool DeleteToken(int Id)
        {
            dbContext.Tokens.Remove(GetUserToken(Id));

            return Save();
        }

        public IList<AuthToken> GetAllTokens(string userId  )
        {
            return dbContext.Tokens.Where(u =>u.UserId.ToLower().Trim() == userId.ToLower().Trim()).ToList();
        }

        public AuthToken GetUserToken(int Id)
        {
            return dbContext.Tokens.FirstOrDefault(t => t.Id == Id);
        }

        public bool Save()
        {
            return dbContext.SaveChanges() >= 0;
        }

        public int TokenCount(string UserId)
        {
            return dbContext.Tokens.Count(t => t.UserId.ToLower().Trim() == UserId.ToLower().Trim());
        }
    }
}
