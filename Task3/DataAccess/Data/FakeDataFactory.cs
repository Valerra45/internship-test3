using Task3.Models;

namespace Task3.DataAccess.Data
{
    public static class FakeDataFactory
    {
        public static List<User> Users => new List<User> 
        {
            new User
            {
                Name = "boris",
                Rating = 5000,
                CoinIds = new List<long>()
            },
            new User
            {
                Name = "maria",
                Rating = 1000,
                CoinIds = new List<long>()
            },
            new User
            {
                Name = "oleg",
                Rating = 800,
                CoinIds = new List<long>()
            }
        };
    }
}
