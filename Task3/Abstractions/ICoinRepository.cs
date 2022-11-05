using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task3.Models;

namespace Task3.Abstractions
{
    public interface ICoinRepository
    {
        public Task<List<BillingCoin>> GetAllAsync();

        public Task Add(BillingCoin coin);

        public Task<BillingCoin> GetById(long id);
    }
}
