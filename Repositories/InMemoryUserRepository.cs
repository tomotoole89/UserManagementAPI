using System.Xml.Linq;
using UserManagementAPI.Models;

namespace UserManagementAPI.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = new();
        private int _nextId = 1;

        public IEnumerable<User> GetAll() => _users;

        public User? GetById(int id) => _users.FirstOrDefault(u => u.Id == id);

        public User Add(User user)
        {
            var newUser = user with { Id = _nextId++ };
            _users.Add(newUser);
            return newUser;
        }

        public User? Update(int id, User user)
        {
            var existing = GetById(id);
            if (existing is null) return null;

            var updated = user with { Id = id };
            _users[_users.FindIndex(u => u.Id == id)] = updated;
            return updated;
        }

        public bool Delete(int id)
        {
            var user = GetById(id);
            if (user is null) return false;
            _users.Remove(user);
            return true;
        }
    }
}




