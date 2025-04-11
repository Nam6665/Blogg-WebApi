using Blogg.Interface;
using Blogg.Models;

namespace Blogg.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;

        public AuthService(IConfiguration configuration, IUserRepository userRepository, JwtService jwtService)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<string> Login(string username, string password)
        {
            var user = await _userRepository.ValidateUserCredentials(username, password);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            return _jwtService.GenerateToken(user);
        }
        public string GenerateJwtToken(User user)
        {
            return _jwtService.GenerateToken(user);
        }
    }
}