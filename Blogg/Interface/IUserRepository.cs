using Blogg.Models;

namespace Blogg.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();      
        Task<User> GetUserById(int userId); 
        Task<int> CreateUser(User user);    
        Task<bool> UpdateUser(User user);            
        Task<bool> DeleteUser(int userId);     
        Task<User?> ValidateUserCredentials(string username, string password);
        Task<User?> GetUserByUsernameAndPassword(string username, string password);


    }
}