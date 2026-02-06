using FEMEE.Application.Interfaces.Common;

namespace FEMEE.Infrastructure.Security
{
    public class BcryptPasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password) => PasswordHasher.HashPassword(password);
        public bool VerifyPassword(string password, string hash) => PasswordHasher.VerifyPassword(password, hash);
        public bool NeedsRehash(string hash) => PasswordHasher.NeedsRehash(hash);
    }
}
