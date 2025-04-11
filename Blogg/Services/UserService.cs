using Blogg.Data;
using Blogg.Models;
using Dapper;

namespace Blogg.Services
{
    public class UserService
    {
        private readonly DapperContext _context;

        public UserService(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var query = "SELECT * FROM Users";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<User>(query);
            }
        }
    }
}
