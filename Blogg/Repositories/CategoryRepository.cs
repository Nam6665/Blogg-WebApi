using Blogg.Data;
using Blogg.Interface;
using Blogg.Models;
using Dapper;
using System.Data;

namespace Blogg.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DapperContext _context;

        public CategoryRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            var query = "GetAllCategories"; 
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Category>(query, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Category> GetCategoryById(int categoryId)
        {
            var query = "GetCategoryById"; 
            var parameters = new DynamicParameters();
            parameters.Add("CategoryId", categoryId);

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Category>(query, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> CreateCategory(Category category)
        {
            var query = "CreateCategory";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleAsync<int>(query, new { Name = category.Name }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<bool> UpdateCategory(Category category)
        {
            var query = "UpdateCategory"; 
            using (var connection = _context.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(query, new
                {
                    CategoryId = category.CategoryId,
                    Name = category.Name
                }, commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteCategory(int categoryId)
        {
            var query = "DeleteCategory";
            using (var connection = _context.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(query, new { CategoryId = categoryId }, commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
        }
    }
}