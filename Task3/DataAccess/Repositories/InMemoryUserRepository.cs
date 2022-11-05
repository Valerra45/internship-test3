using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task3.Abstractions;
using Task3.Models;

namespace Task3.DataAccess.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        protected List<User> Users { get; set; }

        public InMemoryUserRepository(List<User> users)
        {
            Users = users;
        }

        public  Task<List<User>> GetAllAsync()
        {
            return Task.FromResult(Users);
        }

        public Task<User> GetByName(string name)
        {
            var user = Users.FirstOrDefault(x => x.Name!.Equals(name));

            return Task.FromResult(user!);
        }
    }
}
