using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task3.Abstractions;
using Task3.Models;

namespace Task3.DataAccess.Repositories
{
    public class InMemoryCoinRepository : ICoinRepository
    {
        protected List<BillingCoin> Coins { get; set; }

        public InMemoryCoinRepository()
        {
            Coins = new List<BillingCoin>();
        }

        public Task Add(BillingCoin coin)
        {
            if (Coins.Count == 0)
            {
                coin.Id = 1;

                Coins.Add(coin);
            }
            else
            {
                var id = Coins.Max(x => x.Id);

                coin.Id = ++id;

                Coins.Add(coin);
            }

            return Task.CompletedTask;
        }

        public Task<BillingCoin> GetById(long id)
        {
            var coin = Coins.FirstOrDefault(x => x.Id == id);

            return Task.FromResult(coin!);
        }

        public Task<List<BillingCoin>> GetAllAsync()
        {
            return Task.FromResult(Coins);
        }
    }
}
