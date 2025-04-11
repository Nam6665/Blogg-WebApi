using Blogg.Models;

namespace Blogg.Interface
{
    public interface IAuthService
    {
        Task<string> Login(string username, string password);
        string GenerateJwtToken(User user);
    }
}
