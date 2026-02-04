using System.Xml.Linq;
using UserManagementAPI.Models;

namespace UserManagementAPI.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly Dictionary<int, User> _users = new();
        private int _nextId = 1;

        public InMemoryUserRepository()
        {
            // Seed sample users
            var sampleUsers = new List<User>
            {
                new User { Id = 1, FirstName = "Alice", LastName = "Johnson", Email = "alice@company.com" },
                new User { Id = 2, FirstName = "Bob", LastName = "Smith", Email = "bob@company.com" },
                new User { Id = 3, FirstName = "Charlie", LastName = "Brown", Email = "charlie@company.com" }
            };

            foreach (var user in sampleUsers)
            {
                _users[user.Id] = user;
            }

            _nextId = _users.Keys.Max() + 1;
        }

        public IEnumerable<User> GetAll() => _users.Values;

        public User? GetById(int id) =>
            _users.TryGetValue(id, out var user) ? user : null;

        public User Add(User user)
        {
            var newUser = new User
            {
                Id = _nextId++,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            _users[newUser.Id] = newUser;
            return newUser;
        }


        public User? Update(int id, User user)
        {
            if (!_users.ContainsKey(id))
                return null;

            var updated = new User
            {
                Id = id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            _users[id] = updated;
            return updated;
        }


        public bool Delete(int id) =>
        _users.Remove(id);
    }


}




