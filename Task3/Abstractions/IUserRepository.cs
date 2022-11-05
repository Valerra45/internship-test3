using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task3.Models;

namespace Task3.Abstractions
{
    public interface IUserRepository
    {
        public Task<List<User>> GetAllAsync();

        public Task<User> GetByName(string name);
    }
}
