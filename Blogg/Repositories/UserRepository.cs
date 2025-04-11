using Blogg.Data;
using Blogg.Interface;
using Blogg.Models;
using Dapper;
using System.Data;

namespace Blogg.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;

        public UserRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreateUser(User user)
        {
            user.CreatedAt = DateTime.UtcNow;

            var query = "CreateUser";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleAsync<int>(query, new
                {
                    Username = user.Username,
                    PasswordHash = user.PasswordHash,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt
                }, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var query = "GetAllUsers";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<User>(query, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<User> GetUserById(int userId)
        {
            var query = "GetUserById";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<User>(query, new { UserId = userId }, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<bool> UpdateUser(User user)
        {
            var query = "UpdateUser";
            using (var connection = _context.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(query, new
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    PasswordHash = user.PasswordHash,
                    Email = user.Email
                }, commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
        }
        public async Task<bool> DeleteUser(int userId)
        {
            var query = "DeleteUser";
            using (var connection = _context.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(query, new { UserId = userId }, commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
        }
        public async Task<User?> ValidateUserCredentials(string username, string password)
        {
            var query = "ValidateUserCredentials";
            using (var connection = _context.CreateConnection())
            {
                var userId = await connection.QuerySingleOrDefaultAsync<int?>(query, new { Username = username, Password = password }, commandType: CommandType.StoredProcedure);
                return userId.HasValue ? await GetUserById(userId.Value) : null;
            }
        }
        public async Task<User?> GetUserByUsernameAndPassword(string username, string password)
        {
            return await ValidateUserCredentials(username, password);
        }
    }
}