using UserManagementAPI.Models;

namespace UserManagementAPI.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll(); 
        User? GetById(int id); 
        User Add(User user);
        User? Update(int id, User user); 
        bool Delete(int id);
    }
}
