using Grpc.Core;
using Task3.Abstractions;
using Task3.Models;

namespace Task3.Services
{
    public class BillingService : Billing.BillingBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ICoinRepository _coinRepository;

        public BillingService(IUserRepository userRepository,
            ICoinRepository coinRepository)
        {
            _userRepository = userRepository;
            _coinRepository = coinRepository;
        }

        public override async Task ListUsers(None request,
            IServerStreamWriter<UserProfile> responseStream,
            ServerCallContext context)
        {
            var users = await _userRepository.GetAllAsync();

            var userProfiles = users.Select(x => new UserProfile
            {
                Name = x.Name,
                Amount = x.CoinIds!.Count,
            }).ToList();

            foreach (var userProfile in userProfiles)
            {
                await responseStream.WriteAsync(userProfile);
            }
        }

        public override async Task<Response> CoinsEmission(EmissionAmount request, ServerCallContext context)
        {
            if (request.Amount == 0)
            {
                return new Response
                {
                    Status = Response.Types.Status.Failed,
                    Comment = "zero for amount"
                };
            }

            var users = await _userRepository.GetAllAsync();

            users = users.OrderBy(x => x.Rating).ToList();

            var allRaiting = users.Sum(x => x.Rating);

            if (allRaiting == 0)
            {
                return new Response
                {
                    Status = Response.Types.Status.Failed,
                    Comment = "not users rating"
                };
            }

            var amountCoins = request.Amount;

            var k = (double)amountCoins / allRaiting;

            int userAmount = 0;

            for (int i = 0; i < users.Count - 1; i++)
            {
                userAmount = (int)(users[i].Rating * k);

                userAmount = userAmount == 0 ? 1 : userAmount;

                for (int j = 0; j < userAmount; j++)
                {
                    var coin = new BillingCoin();

                    await _coinRepository.Add(coin!);

                    users[i].CoinIds!.Add(coin.Id);

                    coin.History = new List<string>();

                    coin.History.Add($"amount {coin.Id}");
                }

                amountCoins -= userAmount;
            }

            for (int j = 0; j < amountCoins; j++)
            {
                var coin = new BillingCoin();

                await _coinRepository.Add(coin!);

                users[users.Count - 1].CoinIds!.Add(coin.Id);


                coin.History = new List<string>();

                coin.History.Add($"amount {coin.Id}");
            }

            return new Response
            {
                Status = Response.Types.Status.Ok,
                Comment = "coins amount"
            };
        }

        public override async Task<Response> MoveCoins(MoveCoinsTransaction request, ServerCallContext context)
        {
            BillingCoin coin;

            var dstUser = await _userRepository.GetByName(request.DstUser);

            var srcUser = await _userRepository.GetByName(request.SrcUser);

            if (srcUser.CoinIds!.Count < request.Amount)
            {
                return new Response
                {
                    Status = Response.Types.Status.Failed,
                    Comment = "enough coins on the balance"
                };
            }

            for (int i = 0; i < request.Amount; i++)
            {
                coin = await _coinRepository.GetById(srcUser!.CoinIds[i]);

                coin.History!.Add($"move {srcUser.Name} to {dstUser.Name}");
          
                dstUser.CoinIds!.Add(coin.Id);
            }

            srcUser.CoinIds!.RemoveRange(0, (int)request!.Amount);

            return new Response
            {
                Status = Response.Types.Status.Ok,
                Comment = "coins moved"
            };
        }

        public override async Task<Coin> LongestHistoryCoin(None request, ServerCallContext context)
        {
            var coins = await _coinRepository.GetAllAsync();

            coins = coins.OrderByDescending(x => x.History!.Count).ToList();

            return new Coin
            {
                Id = coins[0].Id,
                History = string.Join(",", coins[0].History!.ToArray())
            };
        }
    }
}
